using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Spawn")]
    public Transform spawnPoint;

    [Header("Car Prefabs")]
    public GameObject carro1Prefab;
    public GameObject carro2Prefab;

    [Header("Components")]
    public Speedometer speedometer;
    public FollowCar cameraFollow;
    public CountdownManager countdownManager;
    // ¡NUEVA REFERENCIA! - Asegúrate de arrastrar el objeto RaceManager aquí
    public RaceManager raceManager; 

    void Start()
    {
        Debug.Log("Carro seleccionado: " + GameData.selectedCar);

        GameObject prefabSeleccionado = null;

        // Selección del carro dinámico
        if (GameData.selectedCar == "Carro1")
            prefabSeleccionado = carro1Prefab;
        else if (GameData.selectedCar == "Carro2")
            prefabSeleccionado = carro2Prefab;

        if (prefabSeleccionado == null)
        {
            Debug.LogError("GameManager: No se seleccionó ningún prefab de carro.");
            return;
        }

        Debug.Log("Prefab seleccionado: " + prefabSeleccionado.name);

        // Instanciar el carro seleccionado
        GameObject carroInstanciado =
            Instantiate(prefabSeleccionado, spawnPoint.position, spawnPoint.rotation);

        // Rotación correcta
        carroInstanciado.transform.Rotate(0f, 180f, 0f);

        Debug.Log("Carro instanciado con rotación: " + carroInstanciado.transform.rotation.eulerAngles);

        // Obtener el CarController del carro recién creado
        CarController controller = carroInstanciado.GetComponent<CarController>();

        // 1. Conexión de componentes estándar
        Rigidbody rb = carroInstanciado.GetComponent<Rigidbody>();
        speedometer.carRigidbody = rb;
        cameraFollow.target = carroInstanciado.transform;

        // 2. Conexión con CountdownManager
        if (countdownManager != null)
        {
            countdownManager.carController = controller;
            countdownManager.StartCountdown();
            Debug.Log("CarController asignado al CountdownManager correctamente.");
        }
        else
        {
            Debug.LogError("GameManager: falta asignar CountdownManager en el inspector.");
        }

        // 3. ¡NUEVA CONEXIÓN! Asignar el CarController dinámico al RaceManager
        if (raceManager != null)
        {
            raceManager.playerCar = controller; 
            Debug.Log("CarController asignado al RaceManager dinámicamente.");
        }
        else
        {
            // Opcional: si RaceManager no está en el GameManager, lo buscamos.
            RaceManager foundRaceManager = FindObjectOfType<RaceManager>();
            if (foundRaceManager != null)
            {
                foundRaceManager.playerCar = controller;
                Debug.Log("CarController asignado al RaceManager vía FindObjectOfType.");
            }
            else
            {
                Debug.LogError("GameManager: Falta asignar RaceManager en el inspector y no se pudo encontrar en la escena.");
            }
        }
    }
}