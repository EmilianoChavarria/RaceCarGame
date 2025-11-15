using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform spawnPoint;

    public GameObject carro1Prefab;
    public GameObject carro2Prefab;

    public Speedometer speedometer;
    public FollowCar cameraFollow; // referencia a la cámara

    void Start()
    {

        Debug.Log("Carro seleccionado: " + GameData.selectedCar);

        GameObject prefabSeleccionado = null;

        if (GameData.selectedCar == "Carro1")
            prefabSeleccionado = carro1Prefab;

        if (GameData.selectedCar == "Carro2")
            prefabSeleccionado = carro2Prefab;

        Debug.Log("Prefab seleccionado: " + prefabSeleccionado);


        if (prefabSeleccionado != null)
        {
            GameObject carroInstanciado =
                Instantiate(prefabSeleccionado, spawnPoint.position, spawnPoint.rotation);

            carroInstanciado.transform.Rotate(0f, 180f, 0f);


            Debug.Log("Prefab root rotation (prefabSeleccionado): " + prefabSeleccionado.transform.rotation.eulerAngles);
            Debug.Log("Prefab root localRotation (prefabSeleccionado): " + prefabSeleccionado.transform.localRotation.eulerAngles);
            Debug.Log("SpawnPoint rotation: " + spawnPoint.rotation.eulerAngles);


            Rigidbody rb = carroInstanciado.GetComponent<Rigidbody>();
            speedometer.carRigidbody = rb;

            // 👉 ASIGNAR EL TARGET A LA CÁMARA DINÁMICAMENTE
            cameraFollow.target = carroInstanciado.transform;
        }
    }
}
