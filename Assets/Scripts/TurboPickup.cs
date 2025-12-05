using UnityEngine;

public class TurboPickup : MonoBehaviour
{
    [Tooltip("Efecto visual opcional al recoger el turbo")]
    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        // Buscar el TurboSystem del objeto que nos tocó (el carro)
        TurboSystem turbo = other.GetComponent<TurboSystem>();

        if (turbo != null)
        {
            // Recargar turbo (nuevo método que sí existe)
            turbo.ForceReady();

            Debug.Log("[TURBO PICKUP] Turbo recargado por recoger el objeto!");

            // Instanciar efecto visual si existe
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Destruir el pickup
            Destroy(gameObject);
        }
    }
}
