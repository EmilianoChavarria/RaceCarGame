using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Car Prefabs")]
    public GameObject car1Prefab;
    public GameObject car2Prefab;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    private void Start()
    {
        // Verificar que ya tengas datos guardados
        Debug.Log("Pista seleccionada: " + GameData.selectedTrack);
        Debug.Log("Carro seleccionado: " + GameData.selectedCar);

        SpawnSelectedCar();
    }

    void SpawnSelectedCar()
    {
        GameObject carToSpawn = null;

        // Elegir carro según GameData
        switch (GameData.selectedCar)
        {
            case "Carro1":
                carToSpawn = car1Prefab;
                break;

            case "Carro2":
                carToSpawn = car2Prefab;
                break;

            default:
                Debug.LogWarning("No se seleccionó carro, usando Carro1 por defecto");
                carToSpawn = car1Prefab;
                break;
        }

        // Instanciar el carro
        Instantiate(carToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}
