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

        // Asignar Rigidbody al Speedometer
        Rigidbody rb = carroInstanciado.GetComponent<Rigidbody>();
        speedometer.carRigidbody = rb;

        // Asignar cámara dinámica
        cameraFollow.target = carroInstanciado.transform;

        // Obtener el CarController del carro recién creado
        CarController controller = carroInstanciado.GetComponent<CarController>();

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
    }
}
