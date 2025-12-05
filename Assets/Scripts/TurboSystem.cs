using UnityEngine; 

public class TurboSystem : MonoBehaviour
{
    [Header("Configuración de Turbo")]
    [Tooltip("Multiplicador de velocidad durante el turbo")]
    public float turboSpeedMultiplier = 1.5f;
    
    [Tooltip("Duración del turbo en segundos")]
    public float turboDuration = 3f;
    
    [Tooltip("Tiempo de recarga del turbo en segundos")]
    public float turboCooldown = 5f;
    
    [Tooltip("Fuerza de aceleración extra durante turbo")]
    public float turboAccelerationBoost = 5f;

    [Header("Pickup de Turbo (Opcional)")]
    [Tooltip("¿Puede el jugador obtener turbo mediante pickups?")]
    public bool allowTurboPickup = true;

    // --- NUEVO ---
    private bool hasTurboCharge = false;   // Pickup recogido pero no usado
    // -------------

    [Header("Efectos Visuales (Opcional)")]
    [Tooltip("Partículas que se activan durante el turbo")]
    public ParticleSystem turboParticles;
    
    [Tooltip("Color de emisión durante turbo")]
    public Color turboEmissionColor = Color.cyan;

    private enum TurboState
    {
        Ready,
        Active,
        Cooldown
    }

    private TurboState currentState = TurboState.Ready;
    private float stateTimer = 0f;
    private CarController carController;
    
    private float originalMaxSpeed;
    private float originalAcceleration;
    
    public bool IsTurboReady => currentState == TurboState.Ready;
    public bool IsTurboActive => currentState == TurboState.Active;
    public float GetCooldownProgress => currentState == TurboState.Cooldown ? stateTimer / turboCooldown : 0f;
    public float GetTurboProgress => currentState == TurboState.Active ? stateTimer / turboDuration : 0f;

    void Start()
    {
        carController = GetComponent<CarController>();
        
        if (carController == null)
        {
            Debug.LogError("TurboSystem requiere un CarController en el mismo GameObject!");
            enabled = false;
            return;
        }

        originalMaxSpeed = carController.maxSpeed;
        originalAcceleration = carController.acceleration;
        
        currentState = TurboState.Ready;
        Debug.Log("[TURBO] Sistema inicializado - Estado: READY");
    }

    void Update()
    {
        HandleInput();
        UpdateStateMachine();
    }

    void HandleInput()
    {
        // Detectar tecla R para activar turbo
        if (Input.GetKeyDown(KeyCode.R) && currentState == TurboState.Ready)
        {
            ActivateTurbo();
        }
    }

    void UpdateStateMachine()
    {
        switch (currentState)
        {
            case TurboState.Ready:
                break;

            case TurboState.Active:
                stateTimer += Time.deltaTime;
                
                if (stateTimer >= turboDuration)
                {
                    DeactivateTurbo();
                }
                break;

            case TurboState.Cooldown:
                stateTimer += Time.deltaTime;
                
                if (stateTimer >= turboCooldown)
                {
                    ResetTurbo();
                }
                break;
        }
    }

    void ActivateTurbo()
    {
        currentState = TurboState.Active;
        stateTimer = 0f;

        carController.maxSpeed = originalMaxSpeed * turboSpeedMultiplier;
        carController.acceleration = originalAcceleration + turboAccelerationBoost;

        if (turboParticles != null)
        {
            turboParticles.Play();
        }

        Debug.Log($"[TURBO] Estado: READY → ACTIVE | Velocidad: {originalMaxSpeed} → {carController.maxSpeed}");
    }

    void DeactivateTurbo()
    {
        currentState = TurboState.Cooldown;
        stateTimer = 0f;

        carController.maxSpeed = originalMaxSpeed;
        carController.acceleration = originalAcceleration;

        if (turboParticles != null)
        {
            turboParticles.Stop();
        }

        Debug.Log($"[TURBO] ACTIVE → COOLDOWN | Velocidad restaurada: {carController.maxSpeed}");
    }

    void ResetTurbo()
    {
        currentState = TurboState.Ready;
        stateTimer = 0f;

        Debug.Log("[TURBO] COOLDOWN → READY | Turbo disponible!");
    }

    public void CancelTurbo()
    {
        if (currentState == TurboState.Active)
        {
            DeactivateTurbo();
        }
    }

    public float GetRemainingTime()
    {
        switch (currentState)
        {
            case TurboState.Active:
                return turboDuration - stateTimer;
            case TurboState.Cooldown:
                return turboCooldown - stateTimer;
            default:
                return 0f;
        }
    }

    // --------------------------
    //   NUEVA FUNCIÓN PICKUP  
    // --------------------------
    public void AddTurboCharge()
    {
        if (!allowTurboPickup)
            return;

        hasTurboCharge = true;

        Debug.Log("[TURBO PICKUP] Recogido. Turbo listo para usar (tecla R)");
    }
    // --------------------------

    void OnGUI()
    {
        if (Debug.isDebugBuild)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.white;

            string statusText = $"Turbo: {currentState}";
            
            if (currentState == TurboState.Active)
            {
                style.normal.textColor = Color.green;
                statusText += $"\nTiempo: {(turboDuration - stateTimer):F1}s";
            }
            else if (currentState == TurboState.Cooldown)
            {
                style.normal.textColor = Color.yellow;
                statusText += $"\nRecarga: {(turboCooldown - stateTimer):F1}s";
            }
            else
            {
                style.normal.textColor = Color.cyan;
                statusText += "\n¡Presiona R!";
            }

            GUI.Label(new Rect(10, 10, 300, 100), statusText, style);
        }
    }
}
