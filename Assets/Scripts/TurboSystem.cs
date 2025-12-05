using UnityEngine;

public class TurboSystem : MonoBehaviour
{
    [Header("Configuración de Turbo")]
    [Tooltip("Multiplicador de velocidad durante el turbo")]
    public float turboSpeedMultiplier = 1.5f;
    
    [Tooltip("Fuerza de aceleración extra durante turbo")]
    public float turboAccelerationBoost = 5f;
    
    [Tooltip("Cantidad máxima de turbo disponible")]
    public float maxTurboFuel = 100f;
    
    [Tooltip("Velocidad de consumo de turbo por segundo")]
    public float turboConsumptionRate = 33.3f;
    
    [Tooltip("Velocidad de recarga del turbo por segundo (cuando no está en uso)")]
    public float turboRechargeRate = 10f;
    
    [Tooltip("Tiempo de recarga del turbo en segundos")]
    public float turboCooldown = 5f;
    
    [Tooltip("Duración máxima del turbo en segundos (límite de seguridad)")]
    public float turboDuration = 10f;

    [Header("Efectos Visuales (Opcional)")]
    [Tooltip("Partículas que se activan durante el turbo")]
    public ParticleSystem turboParticles;
    
    [Tooltip("Color de emisión durante turbo")]
    public Color turboEmissionColor = Color.cyan;

    public void AddTurboFuel(float amount)
    {
        currentTurboFuel = Mathf.Min(currentTurboFuel + amount, maxTurboFuel);
        Debug.Log($"[TURBO PICKUP] Se agregó {amount} de turbo. Total: {currentTurboFuel}/{maxTurboFuel}");
    }


    // Estados del turbo
    private enum TurboState
    {
        Ready,      // Listo para usar
        Active,     // Turbo activo
        Cooldown    // Recargando
    }

    private TurboState currentState = TurboState.Ready;
    private float stateTimer = 0f;
    private float currentTurboFuel = 100f;
    private CarController carController;
    
    // Valores originales del carro
    private float originalMaxSpeed;
    private float originalAcceleration;
    
    // Propiedades públicas para UI
    public bool IsTurboReady => currentState == TurboState.Ready && currentTurboFuel > 0f;
    public bool IsTurboActive => currentState == TurboState.Active;
    public float GetCooldownProgress => currentState == TurboState.Cooldown ? stateTimer / turboCooldown : 0f;
    public float GetTurboProgress => currentState == TurboState.Active ? stateTimer / turboDuration : 0f;
    public float GetTurboFuelPercent => currentTurboFuel / maxTurboFuel;

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
        // Detectar tecla R para activar turbo (solo si hay combustible)
        if (Input.GetKeyDown(KeyCode.R) && currentState == TurboState.Ready && currentTurboFuel > 0f)
        {
            ActivateTurbo();
        }
    }

    void UpdateStateMachine()
    {
        switch (currentState)
        {
            case TurboState.Ready:
                // Recargar turbo cuando está listo y no en uso
                if (currentTurboFuel < maxTurboFuel)
                {
                    currentTurboFuel = Mathf.Min(currentTurboFuel + turboRechargeRate * Time.deltaTime, maxTurboFuel);
                }
                break;

            case TurboState.Active:
                // Consumir turbo
                currentTurboFuel -= turboConsumptionRate * Time.deltaTime;
                stateTimer += Time.deltaTime;
                
                // Detener turbo si se acabó el combustible o se alcanzó el tiempo máximo
                if (currentTurboFuel <= 0f || stateTimer >= 10f) // 10s como límite de seguridad
                {
                    currentTurboFuel = Mathf.Max(0f, currentTurboFuel);
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

        // Aplicar boost al carro
        carController.maxSpeed = originalMaxSpeed * turboSpeedMultiplier;
        carController.acceleration = originalAcceleration + turboAccelerationBoost;

        // Activar efectos visuales
        if (turboParticles != null)
        {
            turboParticles.Play();
        }

        Debug.Log($"[TURBO] Estado: READY → ACTIVE | Velocidad: {originalMaxSpeed} → {carController.maxSpeed} | Duración: {turboDuration}s");
    }

    void DeactivateTurbo()
    {
        currentState = TurboState.Cooldown;
        stateTimer = 0f;

        // Restaurar valores originales
        carController.maxSpeed = originalMaxSpeed;
        carController.acceleration = originalAcceleration;

        // Desactivar efectos visuales
        if (turboParticles != null)
        {
            turboParticles.Stop();
        }

        Debug.Log($"[TURBO] Estado: ACTIVE → COOLDOWN | Velocidad restaurada: {carController.maxSpeed} | Tiempo de recarga: {turboCooldown}s");
    }

    void ResetTurbo()
    {
        currentState = TurboState.Ready;
        stateTimer = 0f;
        
        Debug.Log("[TURBO] Estado: COOLDOWN → READY | ¡Turbo disponible! Presiona R para activar");
    }

    // Método opcional para cancelar turbo manualmente
    public void CancelTurbo()
    {
        if (currentState == TurboState.Active)
        {
            DeactivateTurbo();
        }
    }

    // Método para UI - obtener tiempo restante
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

    // Debug visual en el editor
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
