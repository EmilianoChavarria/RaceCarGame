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
    public float bodyTiltAmount = 15f;
    public float tiltSpeed = 5f;
    public float driftAmount = 0.95f;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float verticalInput;
    private float horizontalInput;
    private float currentTilt = 0f;
    private Transform visualBody;

    // ← Necesario para Countdown
    private bool canDrive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configuración SIMPLE
        rb.mass = 1000f;
        rb.linearDamping = 1f;
        rb.angularDamping = 3f;
        rb.useGravity = true;

        visualBody = transform.Find("ModelRotationFix");

        // Desactivar conducción al inicio
        DisableDriving();
    }

    // --------- MÉTODOS PARA COUNTDOWN ---------

    public void EnableDriving()
    {
        canDrive = true;
        Debug.Log("CarController: Conducción habilitada.");
    }

    public void DisableDriving()
    {
        canDrive = false;
        verticalInput = 0;
        horizontalInput = 0;
        currentSpeed = 0;

        if (rb != null)
            rb.linearVelocity = Vector3.zero;

        Debug.Log("CarController: Conducción DESHABILITADA.");
    }

    // ------------------------------------------

    void Update()
    {
        if (!canDrive)
        {
            verticalInput = 0;
            horizontalInput = 0;
            return;
        }

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if (!canDrive)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Move();
        Turn();
        ApplyDrift();
    }

    void LateUpdate()
    {
        ApplyBodyTilt();
    }

    void Move()
    {
        if (verticalInput != 0)
        {
            currentSpeed += verticalInput * acceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeSpeed * Time.fixedDeltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeSpeed * 2f * Time.fixedDeltaTime);
        }

        Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void Turn()
    {
        if (Mathf.Abs(currentSpeed) > 0.5f)
        {
            float speedFactor = Mathf.Abs(currentSpeed) / maxSpeed;
            speedFactor = Mathf.Clamp(speedFactor, 0.3f, 1f);

            float turnReduction = 1f - (speedFactor * 0.4f);

            float turn = horizontalInput * turnSpeed * turnReduction * Time.fixedDeltaTime;

            if (currentSpeed < 0)
            {
                turn = -turn;
            }

            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    void ApplyDrift()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);

        localVelocity.x *= driftAmount;

        rb.linearVelocity = transform.TransformDirection(localVelocity);
    }

    void ApplyBodyTilt()
    {
        if (visualBody != null && Mathf.Abs(currentSpeed) > 1f)
        {
            float targetTilt = horizontalInput * bodyTiltAmount * (Mathf.Abs(currentSpeed) / maxSpeed);

            currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

            visualBody.localRotation = Quaternion.Euler(0, 180, currentTilt);
        }
        else if (visualBody != null)
        {
            currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * tiltSpeed);
            visualBody.localRotation = Quaternion.Euler(0, 180, currentTilt);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Busca una referencia al RaceManager en la escena
        RaceManager raceManager = FindObjectOfType<RaceManager>();

        if (other.CompareTag("FinishLine") && raceManager != null)
        {
            // Llama al método del gestor de carrera cuando el coche cruza la meta
            raceManager.FinishLap();
             Debug.Log("Trigger detectado: Meta cruzada.");
        }
    }

    // Para UI o debug
    public float GetSpeed() => currentSpeed;
    public float GetSpeedKPH() => currentSpeed * 3.6f;
}
