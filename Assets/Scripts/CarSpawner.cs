using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Carros Disponibles")]
    public GameObject carro1Prefab;
    public GameObject carro2Prefab;

    [Header("Punto de Spawn")]
    public Transform spawnPoint;

    void Start()
    {
        SpawnSelectedCar();
    }

    void SpawnSelectedCar()
    {
        GameObject carroASpawnear = null;

        // Seleccionar el carro según GameData
        switch (GameData.selectedCar)
        {
            case "Carro1":
                carroASpawnear = carro1Prefab;
                break;
            case "Carro2":
                carroASpawnear = carro2Prefab;
                break;
            default:
                carroASpawnear = carro1Prefab; // Por defecto
                break;
        }

        if (carroASpawnear != null && spawnPoint != null)
        {
            GameObject carroInstanciado = Instantiate(carroASpawnear, spawnPoint.position, spawnPoint.rotation);
            
            // Asignar cámara al carro
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
                if (cameraFollow != null)
                {
                    cameraFollow.target = carroInstanciado.transform;
                }
            }
        }
        else
        {
            Debug.LogError("Falta asignar el prefab del carro o el spawn point!");
        }
    }
}