using UnityEngine;

public class WheelAnimator : MonoBehaviour
{
    [Header("Ruedas (Wheels)")]
    public Transform wheelFrontLeft;
    public Transform wheelFrontRight;
    public Transform wheelBackLeft;
    public Transform wheelBackRight;
    
    [Header("Configuración")]
    public float wheelRadius = 0.35f;
    public float maxSteerAngle = 30f; 
    public float steerSpeed = 5f; 
    
    private CarController carController;
    private float wheelRotation = 0f;
    private float currentSteerAngle = 0f; 
    
    private bool isInitialized = false;

    void Start()
    {
        carController = GetComponent<CarController>();
        
        if (carController == null)
        {
            Debug.LogError("WheelAnimator: No se encontró CarController en el mismo GameObject!");
            return;
        }
        
        isInitialized = true;
        
        if (wheelFrontLeft != null) Debug.Log("✓ Rueda FL encontrada");
        if (wheelFrontRight != null) Debug.Log("✓ Rueda FR encontrada");
        if (wheelBackLeft != null) Debug.Log("✓ Rueda BL encontrada");
        if (wheelBackRight != null) Debug.Log("✓ Rueda BR encontrada");
    }

    void Update()
    {
        if (!isInitialized || carController == null) return;
        
        AnimateWheels();
    }

    void AnimateWheels()
    {
        float speed = carController.GetSpeed();
        float horizontalInput = Input.GetAxis("Horizontal");

        float wheelRPM = (speed / (2 * Mathf.PI * wheelRadius)) * 360f * Time.deltaTime;
        wheelRotation += wheelRPM;
        
        wheelRotation = wheelRotation % 360f;
        
        float targetSteerAngle = horizontalInput * maxSteerAngle;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * steerSpeed);
        
        
        if (wheelFrontLeft != null)
        {
            Quaternion steerRotation = Quaternion.Euler(0, 0, currentSteerAngle);
            Quaternion wheelRoll = Quaternion.Euler(wheelRotation, 0, 0);
            
            wheelFrontLeft.localRotation = steerRotation * wheelRoll;
        }
        
        if (wheelFrontRight != null)
        {
            Quaternion steerRotation = Quaternion.Euler(0, 0, currentSteerAngle);
            Quaternion wheelRoll = Quaternion.Euler(wheelRotation, 0, 0);
            
            wheelFrontRight.localRotation = steerRotation * wheelRoll;
        }
        
        if (wheelBackLeft != null)
        {
            wheelBackLeft.localRotation = Quaternion.Euler(wheelRotation, 0, 0);
        }
        
        if (wheelBackRight != null)
        {
            wheelBackRight.localRotation = Quaternion.Euler(wheelRotation, 0, 0);
        }
    }
    
    void OnGUI()
    {
        if (!isInitialized) return;
        
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 14;
        style.normal.textColor = Color.white;
        
        GUI.Label(new Rect(10, 10, 400, 25), $"Velocidad: {carController.GetSpeed():F1} m/s ({carController.GetSpeedKPH():F0} km/h)", style);

    }
}