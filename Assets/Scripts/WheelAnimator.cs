using UnityEngine;

public class WheelAnimator : MonoBehaviour
{
    [Header("Ruedas (Wheels)")]
    public Transform wheelFrontLeft;
    public Transform wheelFrontRight;
    public Transform wheelBackLeft;
    public Transform wheelBackRight;
    
    [Header("Configuración")]
    public float wheelRadius = 0.35f; // Radio de la rueda
    public float maxSteerAngle = 30f; // Ángulo máximo de dirección
    public float steerSpeed = 5f; // Velocidad de transición de dirección (más realista)
    
    private CarController carController;
    private float wheelRotation = 0f;
    private float currentSteerAngle = 0f; // Para suavizar el giro
    
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
        
        // Log para verificar las ruedas
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

        // 1. CALCULAR ROTACIÓN DE RUEDAS (giro hacia adelante/atrás)
        float wheelRPM = (speed / (2 * Mathf.PI * wheelRadius)) * 360f * Time.deltaTime;
        wheelRotation += wheelRPM;
        
        // Mantener el ángulo en rango razonable
        wheelRotation = wheelRotation % 360f;
        
        // 2. CALCULAR ÁNGULO DE DIRECCIÓN - SUAVIZADO
        float targetSteerAngle = horizontalInput * maxSteerAngle;
        currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * steerSpeed);
        
        // 3. APLICAR ROTACIONES DE FORMA SEPARADA
        // Usamos quaterniones multiplicados en el orden correcto
        
        // RUEDAS DELANTERAS (con dirección)
        if (wheelFrontLeft != null)
        {
            // Primero: Dirección en Z (volante)
            // Segundo: Giro en X (rueda rodando)
            Quaternion steerRotation = Quaternion.Euler(0, 0, currentSteerAngle);
            Quaternion wheelRoll = Quaternion.Euler(wheelRotation, 0, 0);
            
            // Aplicar en orden: primero dirección, luego giro
            wheelFrontLeft.localRotation = steerRotation * wheelRoll;
        }
        
        if (wheelFrontRight != null)
        {
            Quaternion steerRotation = Quaternion.Euler(0, 0, currentSteerAngle);
            Quaternion wheelRoll = Quaternion.Euler(wheelRotation, 0, 0);
            
            wheelFrontRight.localRotation = steerRotation * wheelRoll;
        }
        
        // RUEDAS TRASERAS (solo giro, sin dirección)
        if (wheelBackLeft != null)
        {
            wheelBackLeft.localRotation = Quaternion.Euler(wheelRotation, 0, 0);
        }
        
        if (wheelBackRight != null)
        {
            wheelBackRight.localRotation = Quaternion.Euler(wheelRotation, 0, 0);
        }
    }
    
    // DEBUG: Ver valores en tiempo real
    void OnGUI()
    {
        if (!isInitialized) return;
        
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 14;
        style.normal.textColor = Color.white;
        
        GUI.Label(new Rect(10, 10, 400, 25), $"Velocidad: {carController.GetSpeed():F1} m/s ({carController.GetSpeedKPH():F0} km/h)", style);

    }
}