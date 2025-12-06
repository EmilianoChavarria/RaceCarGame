using UnityEngine;

public class TurboPickup : MonoBehaviour
{
    [Header("Configuración Visual")]
    [Tooltip("Escala del pickup en estado inactivo")]
    public float inactiveScale = 1f;
    
    [Tooltip("Escala del pickup en estado activo/recogido")]
    public float activeScale = 1.3f;
    
    [Tooltip("Velocidad de rotación en estado inactivo")]
    public float rotationSpeed = 50f;
    
    [Tooltip("Efecto visual opcional al recoger")]
    public GameObject pickupEffect;
    
    [Tooltip("Duración de la animación de recogida en segundos")]
    public float pickupDuration = 0.5f;

    // Estados del pickup
    public enum PickupState
    {
        Idle,       // Esperando ser recogido
        Collected,  // Siendo recogido
        Destroyed   // Destruido
    }

    // Propiedades públicas para debugging
    public PickupState CurrentState => currentState;
    public float StateTimer => stateTimer;

    private PickupState currentState = PickupState.Idle;
    private float stateTimer = 0f;
    private Vector3 originalScale;
    private Collider pickupCollider;

    void Start()
    {
        // Guardar escala original del modelado
        originalScale = transform.localScale;
        
        // Aplicar escala inactiva
        transform.localScale = originalScale * inactiveScale;
        
        // Obtener collider y asegurar que sea trigger
        pickupCollider = GetComponent<Collider>();
        if (pickupCollider != null)
        {
            pickupCollider.isTrigger = true;
            
            // Si es SphereCollider, aumentar radio para mejor detección
            if (pickupCollider is SphereCollider sphereCol)
            {
                sphereCol.radius = 3f; // Radio amplio para mejor detección
            }
            
            Debug.Log($"[TURBO PICKUP] Collider configurado como trigger. Tipo: {pickupCollider.GetType().Name}");
        }
        else
        {
            Debug.LogWarning("[TURBO PICKUP] No hay Collider en el pickup! Agregando SphereCollider...");
            SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = 3f; // Radio amplio para detección mejor
            pickupCollider = sphere;
        }
        
        // Inicializar estado
        EnterState(PickupState.Idle);
    }

    void Update()
    {
        UpdateStateMachine();
    }

    private void UpdateStateMachine()
    {
        switch (currentState)
        {
            case PickupState.Idle:
                UpdateIdleState();
                break;

            case PickupState.Collected:
                UpdateCollectedState();
                break;

            case PickupState.Destroyed:
                // Estado final, no hacer nada
                break;
        }
    }

    private void UpdateIdleState()
    {
        // Rotación continua del modelado
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void UpdateCollectedState()
    {
        stateTimer += Time.deltaTime;
        
        // Calcular progreso de la animación (0 a 1)
        float progress = Mathf.Clamp01(stateTimer / pickupDuration);
        
        // Animar escala: de inactiva a activa
        float currentScale = Mathf.Lerp(inactiveScale, activeScale, progress);
        transform.localScale = originalScale * currentScale;
        
        // Rotación más rápida durante la recogida
        transform.Rotate(0, rotationSpeed * 2f * Time.deltaTime, 0);

        // Cuando se completa la animación
        if (stateTimer >= pickupDuration)
        {
            ExitState(PickupState.Collected);
            EnterState(PickupState.Destroyed);
            DestroyPickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Solo recoger si estamos en estado Idle
        if (currentState != PickupState.Idle)
        {
            return;
        }

        Debug.Log($"[TURBO PICKUP] Colisión detectada con: {other.gameObject.name}");

        // Buscar TurboSystem en el objeto que chocó o en su padre
        TurboSystem turbo = other.GetComponent<TurboSystem>();
        if (turbo == null)
        {
            turbo = other.GetComponentInParent<TurboSystem>();
        }

        if (turbo != null)
        {
            // Agregar combustible de turbo
            turbo.AddTurboFuel(50f);
            Debug.Log($"[TURBO PICKUP] ¡Turbo recargado correctamente! Total: {turbo.GetTurboFuelPercent * 100:F0}%");

            // Cambiar estado a Collected
            ChangeState(PickupState.Collected);
        }
        else
        {
            Debug.LogWarning($"[TURBO PICKUP] {other.gameObject.name} no tiene componente TurboSystem");
        }
    }

    /// <summary>
    /// Cambia el estado actual del pickup
    /// </summary>
    private void ChangeState(PickupState newState)
    {
        ExitState(currentState);
        EnterState(newState);
    }

    /// <summary>
    /// Se ejecuta al entrar en un estado
    /// </summary>
    private void EnterState(PickupState state)
    {
        currentState = state;
        stateTimer = 0f;

        switch (state)
        {
            case PickupState.Idle:
                Debug.Log("[TURBO PICKUP] → Estado: IDLE (Esperando ser recogido)");
                transform.localScale = originalScale * inactiveScale;
                break;

            case PickupState.Collected:
                Debug.Log("[TURBO PICKUP] → Estado: COLLECTED (Siendo recogido)");
                break;

            case PickupState.Destroyed:
                Debug.Log("[TURBO PICKUP] → Estado: DESTROYED (Destruido)");
                break;
        }
    }

    /// <summary>
    /// Se ejecuta al salir de un estado
    /// </summary>
    private void ExitState(PickupState state)
    {
        switch (state)
        {
            case PickupState.Idle:
                Debug.Log("[TURBO PICKUP] ← Saliendo de: IDLE");
                break;

            case PickupState.Collected:
                Debug.Log("[TURBO PICKUP] ← Saliendo de: COLLECTED");
                break;

            case PickupState.Destroyed:
                Debug.Log("[TURBO PICKUP] ← Saliendo de: DESTROYED");
                break;
        }
    }

    private void DestroyPickup()
    {
        // Instanciar efecto visual si existe
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Debug.Log("[TURBO PICKUP] Efecto visual instanciado");
        }

        // Destruir el GameObject después de un frame
        Destroy(gameObject);
        Debug.Log("[TURBO PICKUP] TurboBoost_PowerUp destruido");
    }

    /// <summary>
    /// Resetea el pickup a su estado inicial (para respawn)
    /// </summary>
    public void ResetState()
    {
        currentState = PickupState.Idle;
        stateTimer = 0f;
        transform.localScale = originalScale * inactiveScale;
        
        Debug.Log("[TURBO PICKUP] Estado reseteado a IDLE");
    }

    // Debug visual en editor
    private void OnGUI()
    {
        if (Debug.isDebugBuild)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 16;
            style.normal.textColor = Color.white;

            string statusText = $"TurboBoost Estado: {currentState}\nTiempo: {stateTimer:F2}s";
            GUI.Label(new Rect(10, 100, 300, 80), statusText, style);
        }
    }
}