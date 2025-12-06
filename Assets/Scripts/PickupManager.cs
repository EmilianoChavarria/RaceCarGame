using UnityEngine;

public class PickupManager : MonoBehaviour
{
    // Almacena las posiciones y rotaciones de los pickups iniciales
    private struct PickupSpawnData
    {
        public Vector3 position;
        public Quaternion rotation;
        public GameObject prefab;
    }
    
    private PickupSpawnData[] spawnDataArray;
    private GameObject[] spawnedPickups;
    
    void Start()
    {
        // Encontrar todos los pickups en la escena
        TurboPickup[] pickups = FindObjectsOfType<TurboPickup>();
        
        if (pickups.Length == 0)
        {
            Debug.LogWarning("[PickupManager] No se encontraron pickups en la escena!");
            return;
        }
        
        // Guardar posiciones, rotaciones y prefabs de los pickups
        spawnDataArray = new PickupSpawnData[pickups.Length];
        spawnedPickups = new GameObject[pickups.Length];
        
        for (int i = 0; i < pickups.Length; i++)
        {
            spawnDataArray[i].position = pickups[i].transform.position;
            spawnDataArray[i].rotation = pickups[i].transform.rotation;
            spawnDataArray[i].prefab = pickups[i].gameObject; // Guardar el prefab original
            spawnedPickups[i] = pickups[i].gameObject;
        }
        
        Debug.Log($"[PickupManager] Se registraron {pickups.Length} pickups para respawn");
    }
    
    /// <summary>
    /// Respawnea todos los pickups en sus posiciones originales
    /// </summary>
    public void RespawnAllPickups()
    {
        if (spawnDataArray == null || spawnDataArray.Length == 0)
        {
            Debug.LogWarning("[PickupManager] No hay datos de respawn disponibles!");
            return;
        }
        
        for (int i = 0; i < spawnDataArray.Length; i++)
        {
            // Si el pickup fue destruido, instanciar uno nuevo
            if (spawnedPickups[i] == null)
            {
                // Clonar el prefab original guardado
                if (spawnDataArray[i].prefab != null)
                {
                    spawnedPickups[i] = Instantiate(
                        spawnDataArray[i].prefab,
                        spawnDataArray[i].position,
                        spawnDataArray[i].rotation
                    );
                    Debug.Log($"[PickupManager] Pickup {i + 1} clonado en posición original");
                }
                else
                {
                    Debug.LogError("[PickupManager] Prefab no disponible!");
                }
            }
            else
            {
                // Si existe, solo restaurar su posición y rotación, y reiniciar su estado
                spawnedPickups[i].transform.position = spawnDataArray[i].position;
                spawnedPickups[i].transform.rotation = spawnDataArray[i].rotation;
                
                // Resetear el componente TurboPickup
                TurboPickup pickup = spawnedPickups[i].GetComponent<TurboPickup>();
                if (pickup != null)
                {
                    pickup.ResetState();
                }
                
                Debug.Log($"[PickupManager] Pickup {i + 1} reseteado");
            }
        }
        
        Debug.Log("[PickupManager] Todos los pickups han sido respawneados");
    }
}
