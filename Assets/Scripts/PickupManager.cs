using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [Header("Prefab del Turbo Pickup")]
    public GameObject turboPickupPrefab;  // ← ¡nuevo!

    private struct PickupSpawnData
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    private PickupSpawnData[] spawnDataArray;
    private GameObject[] spawnedPickups;

    void Start()
    {
        TurboPickup[] pickups = FindObjectsOfType<TurboPickup>();

        if (pickups.Length == 0)
        {
            Debug.LogWarning("[PickupManager] No se encontraron pickups en la escena!");
            return;
        }

        spawnDataArray = new PickupSpawnData[pickups.Length];
        spawnedPickups = new GameObject[pickups.Length];

        for (int i = 0; i < pickups.Length; i++)
        {
            spawnDataArray[i].position = pickups[i].transform.position;
            spawnDataArray[i].rotation = pickups[i].transform.rotation;

            spawnedPickups[i] = pickups[i].gameObject;
        }

        Debug.Log($"[PickupManager] Se registraron {pickups.Length} pickups para respawn");
    }

    public void RespawnAllPickups()
    {
        for (int i = 0; i < spawnDataArray.Length; i++)
        {
            if (spawnedPickups[i] == null)
            {
                // Instanciar el prefab real
                spawnedPickups[i] = Instantiate(
                    turboPickupPrefab,
                    spawnDataArray[i].position,
                    spawnDataArray[i].rotation
                );

                Debug.Log($"[PickupManager] Pickup {i + 1} respawneado");
            }
            else
            {
                // Reiniciar estado y posicionar
                spawnedPickups[i].transform.position = spawnDataArray[i].position;
                spawnedPickups[i].transform.rotation = spawnDataArray[i].rotation;

                var pickup = spawnedPickups[i].GetComponent<TurboPickup>();
                if (pickup != null)
                {
                    pickup.ResetState();
                }

                Debug.Log($"[PickupManager] Pickup {i + 1} reseteado");
            }
        }
    }
}
