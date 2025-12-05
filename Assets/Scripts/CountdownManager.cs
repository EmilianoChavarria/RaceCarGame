using UnityEngine;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text countdownText;

    [Header("Settings")]
    public int startNumber = 3;
    public float showDurationPerNumber = 1f;
    public string goText = "GO!";
    public float goDuration = 0.8f;

    [Header("Car")]
    public CarController carController;   // Lo asigna el GameManager

    private int currentNumber;
    private bool countdownFinished = false;

    // No hacemos nada aquí
    void Start() {}

    // 🔥 MÉTODO QUE SE LLAMA DESDE GAMEMANAGER
    public void StartCountdown()
    {
        currentNumber = startNumber;
        StartCoroutine(CountdownRoutine());
    }

    private System.Collections.IEnumerator CountdownRoutine()
    {
        // 3, 2, 1...
        while (currentNumber > 0)
        {
            countdownText.text = currentNumber.ToString();
            yield return new WaitForSeconds(showDurationPerNumber);
            currentNumber--;
        }

        countdownText.text = goText;

        // 🔥 Activar conducción ahora que ya existe el carro
        if (carController != null)
        {
            carController.EnableDriving();
        }
        else
        {
            Debug.LogError("CountdownManager: NO se asignó carController antes del GO!");
        }

        yield return new WaitForSeconds(goDuration);
        countdownText.gameObject.SetActive(false);

        countdownFinished = true;
    }

    public bool IsFinished() => countdownFinished;
}
