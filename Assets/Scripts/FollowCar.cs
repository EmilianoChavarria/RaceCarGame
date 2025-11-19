using UnityEngine;

public class FollowCar : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // el coche
    
    [Header("Camera Position")]
    public float height = 5f; // altura sobre el coche
    public float distance = 10f; // distancia detrás del coche
    public float smoothSpeed = 8f; // velocidad de suavizado (más alto = más rápido)
    
    [Header("Camera Look")]
    public float lookAheadDistance = 3f; // qué tan adelante mirar del coche
    public float rotationSpeed = 5f; // velocidad de rotación de la cámara
    
    [Header("Opciones")]
    public bool lockYPosition = true; // mantener altura fija (recomendado)
    public float minHeight = 2f; // altura mínima de la cámara
    
    private Vector3 currentVelocity;
    
    void Start()
    {
        if (target == null)
        {
            Debug.LogError("FollowCar: ¡Debes asignar el Target (Car) en el Inspector!");
        }
    }
    
    void LateUpdate()
    {
        if (target == null) 
        {
            Debug.LogWarning("FollowCar: No hay target asignado!");
            return;
        }

        // Calcular posición deseada DETRÁS del coche
        Vector3 desiredPosition = target.position - (target.forward * distance);
        desiredPosition.y = lockYPosition ? target.position.y + height : desiredPosition.y + height;
        
        // Asegurar altura mínima
        if (desiredPosition.y < target.position.y + minHeight)
        {
            desiredPosition.y = target.position.y + minHeight;
        }
        
        // Suavizar movimiento de la cámara usando SmoothDamp
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            desiredPosition, 
            ref currentVelocity, 
            1f / smoothSpeed
        );
        
        // Punto al que mira la cámara (ligeramente adelante del coche)
        Vector3 lookAtPosition = target.position + (target.forward * lookAheadDistance);
        lookAtPosition.y = target.position.y + 1f; // mirar un poco arriba del centro
        
        // Suavizar rotación de la cámara
        Vector3 direction = lookAtPosition - transform.position;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                Time.deltaTime * rotationSpeed
            );
        }
    }
    
    // Visualización en el editor
    void OnDrawGizmosSelected()
    {
        if (target == null) return;
        
        // Dibujar línea desde cámara al target
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, target.position);
        
        // Dibujar posición deseada
        Vector3 desiredPosition = target.position - (target.forward * distance);
        desiredPosition.y = target.position.y + height;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(desiredPosition, 0.5f);
        
        // Dibujar punto de mirada
        Vector3 lookAtPosition = target.position + (target.forward * lookAheadDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lookAtPosition, 0.3f);
    }
}
