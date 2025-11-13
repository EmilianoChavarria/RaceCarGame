using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Movimiento Simple")]
    public float moveSpeed = 20f;
    public float maxSpeed = 50f;
    public float turnSpeed = 100f;
    public float acceleration = 10f;
    public float brakeSpeed = 15f;
    
    [Header("Realismo Visual")]
    public float bodyTiltAmount = 15f; // Inclinación del carro al girar
    public float tiltSpeed = 5f; // Velocidad de inclinación
    public float driftAmount = 0.95f; // Cuánto se desliza lateralmente
    
    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float verticalInput;
    private float horizontalInput;
    private float currentTilt = 0f; // Inclinación actual
    private Transform visualBody; // El modelo visual del carro

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configuración SIMPLE
        rb.mass = 1000f;
        rb.linearDamping = 1f; // Frena naturalmente
        rb.angularDamping = 3f; // No gira locamente
        rb.useGravity = true;
        
        // Buscar el ModelRotationFix para la inclinación
        visualBody = transform.Find("ModelRotationFix");
    }

    void Update()
    {
        // Leer inputs
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        Move();
        Turn();
        ApplyDrift(); // Nuevo: drift lateral
    }
    
    void LateUpdate()
    {
        ApplyBodyTilt(); // Nuevo: inclinación visual
    }

    void Move()
    {
        // Acelerar o frenar
        if (verticalInput != 0)
        {
            currentSpeed += verticalInput * acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed);
        }
        else
        {
            // Desacelerar naturalmente
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeSpeed * Time.fixedDeltaTime);
        }

        // Freno manual
        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeSpeed * 2f * Time.fixedDeltaTime);
        }

        // Mover el carro hacia adelante
        Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void Turn()
    {
        // Solo girar si se está moviendo
        if (Mathf.Abs(currentSpeed) > 0.5f)
        {
            // Calcular velocidad de giro basada en la velocidad del carro
            float speedFactor = Mathf.Abs(currentSpeed) / maxSpeed;
            speedFactor = Mathf.Clamp(speedFactor, 0.3f, 1f); // Mínimo 30% de giro
            
            // Giro proporcional a la velocidad (más lento a alta velocidad = más realista)
            float turnReduction = 1f - (speedFactor * 0.4f); // Reducir giro hasta 40% a máxima velocidad
            
            // INVERTIR la dirección con el signo negativo
            float turn = -horizontalInput * turnSpeed * turnReduction * Time.fixedDeltaTime;
            
            // Si va en reversa, invertir giro OTRA VEZ (como un carro real)
            if (currentSpeed < 0)
            {
                turn = -turn;
            }
            
            // Aplicar rotación suave
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }
    
    void ApplyDrift()
    {
        // Simular derrape lateral (drift) - como carros reales
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        
        // Reducir velocidad lateral (hace que el carro "se agarre" a la dirección)
        localVelocity.x *= driftAmount;
        
        // Aplicar la velocidad corregida
        rb.linearVelocity = transform.TransformDirection(localVelocity);
    }
    
    void ApplyBodyTilt()
    {
        // Inclinación visual del carro al girar (como carros reales)
        if (visualBody != null && Mathf.Abs(currentSpeed) > 1f)
        {
            // Calcular inclinación objetivo basada en el giro (INVERTIR TAMBIÉN)
            float targetTilt = horizontalInput * bodyTiltAmount * (Mathf.Abs(currentSpeed) / maxSpeed);
            
            // Suavizar la inclinación
            currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
            
            // La rotación base es (0, 180, 0), agregamos inclinación en Z
            visualBody.localRotation = Quaternion.Euler(0, 180, currentTilt);
        }
        else if (visualBody != null)
        {
            // Volver a posición base cuando está parado
            currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * tiltSpeed);
            visualBody.localRotation = Quaternion.Euler(0, 180, currentTilt);
        }
    }

    // Para UI o debug
    public float GetSpeed() => currentSpeed;
    public float GetSpeedKPH() => currentSpeed * 3.6f;
}
