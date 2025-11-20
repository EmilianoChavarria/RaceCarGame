using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public Rigidbody carRigidbody; // Asigna el Rigidbody del coche
    public TextMeshProUGUI speedText; // Asigna el texto TMP desde el Canvas

    void Update()
    {
        if (carRigidbody != null && speedText != null)
        {
            float speed = carRigidbody.linearVelocity.magnitude * 3.6f; // Convierte de m/s a km/h
            speedText.text = Mathf.RoundToInt(speed) + " km/h";
        }
    }
}
