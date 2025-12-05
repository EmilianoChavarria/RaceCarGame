using UnityEngine;

public class TurboPickup : MonoBehaviour
{
    [Tooltip("Efecto visual opcional al recoger")]
    public GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        TurboSystem turbo = other.GetComponent<TurboSystem>();

        if (turbo != null)
        {
            turbo.AddTurboCharge();

            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            Destroy(gameObject); // Eliminar pickup
        }
    }
}
