using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Velocidad")]
    public float acceleration = 15f;
    public float maxSpeed = 20f;
    public float turnSpeed = 70f;

    [Header("Física suave")]
    public float drag = 2f;
    public float driftFactor = 0.95f;

    private float moveInput;
    private float turnInput;
    private float currentSpeed = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Si no tiene Rigidbody, lo agrega automáticamente
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        rb.linearDamping = 0f;
        rb.angularDamping = 0.5f;
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        // Aceleración suave
        if (moveInput > 0)
            currentSpeed += acceleration * Time.fixedDeltaTime;
        else if (moveInput < 0)
            currentSpeed -= acceleration * 0.5f * Time.fixedDeltaTime;
        else
            currentSpeed = Mathf.Lerp(currentSpeed, 0, drag * Time.fixedDeltaTime);

        // Limitar velocidad
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed * 0.5f, maxSpeed);

        // Aplicar movimiento
        rb.linearVelocity = transform.forward * currentSpeed;

        // Girar según velocidad
        float turn = turnInput * turnSpeed * Time.fixedDeltaTime * Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed);
        transform.Rotate(0, turn, 0, Space.World);

        // Reducir derrape lateral
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        localVelocity.x *= driftFactor;
        rb.linearVelocity = transform.TransformDirection(localVelocity);
    }
}
