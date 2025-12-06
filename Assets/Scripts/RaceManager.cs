using UnityEngine;
using TMPro; // Asegúrate de tener instalado TextMeshPro

public class RaceManager : MonoBehaviour
{
    [Header("Configuración de Carrera")]
    public int totalLaps = 3;
    private int currentLap = 0;
    private bool raceStarted = false;

    [Header("Tiempos")]
    private float startTime;
    private float currentTime;
    private float bestLapTime = float.MaxValue; // Inicializa con un valor muy alto
    private float lastLapTime = 0f;

    [Header("Referencias de UI")]
    public TextMeshProUGUI lapText; // Muestra "Vuelta X/Y"
    public TextMeshProUGUI timeText; // Muestra el tiempo actual
    public TextMeshProUGUI bestTimeText; // Muestra el mejor tiempo

    // Referencia al coche para habilitar/deshabilitar la conducción si es necesario
    public CarController playerCar;
    private TurboSystem turboSystem;
    private PickupManager pickupManager; 

    void Start()
    {
        // En un juego completo, esto se llamaría después del Countdown
        // Por ahora, lo llamamos directamente para que funcione.
        StartRace();
        UpdateUITexts();
        
        // Obtener referencia al TurboSystem del carro
        if (playerCar == null)
        {
            playerCar = FindFirstObjectByType<CarController>();
        }
        
        if (playerCar != null)
        {
            turboSystem = playerCar.GetComponent<TurboSystem>();
            if (turboSystem != null)
            {
                Debug.Log("[RaceManager] TurboSystem encontrado. Se reiniciará con cada vuelta.");
            }
        }
        
        // Obtener referencia al PickupManager
        pickupManager = FindFirstObjectByType<PickupManager>();
        if (pickupManager == null)
        {
            Debug.LogWarning("[RaceManager] PickupManager no encontrado. Los pickups no se respawnearán.");
        }
    }

    void Update()
    {
        if (raceStarted)
        {
            // Acumula el tiempo desde el inicio de la carrera
            currentTime = Time.time - startTime;
            UpdateUITexts(); 
        }
    }

    // ------------------- LÓGICA DE TIEMPO Y VUELTAS -------------------

    public void StartRace()
    {
        currentLap = 1;
        startTime = Time.time;
        raceStarted = true;
        // playerCar.EnableDriving(); // Descomentar si usas un script Countdown
        Debug.Log("¡Carrera iniciada!");
    }

    public void FinishLap()
    {
        if (!raceStarted) return;

        // 1. Calcula el tiempo de la vuelta
        float newLapTime = currentTime; 

        // 2. Actualiza el Mejor Tiempo
        if (newLapTime < bestLapTime)
        {
            bestLapTime = newLapTime;
        }

        lastLapTime = newLapTime; 

        // 3. Incrementa la vuelta y chequea si la carrera terminó
        currentLap++;

        if (currentLap > totalLaps)
        {
            EndRace();
        }
        else
        {
            // Reinicia el turbo para la nueva vuelta
            if (turboSystem != null)
            {
                turboSystem.ResetTurboForNewLap();
                Debug.Log("[RaceManager] Turbo reiniciado para nueva vuelta.");
            }
            
            // Respawnear todos los pickups
            if (pickupManager != null)
            {
                pickupManager.RespawnAllPickups();
                Debug.Log("[RaceManager] Pickups respawneados para nueva vuelta.");
            }
            
            // Reinicia el contador de tiempo para la nueva vuelta
            startTime = Time.time; 
            UpdateUITexts();
            Debug.Log($"Vuelta completada: {lastLapTime:F2}s. Mejor tiempo: {bestLapTime:F2}s.");
        }
    }

    public void EndRace()
    {
        raceStarted = false;
        // playerCar.DisableDriving(); // Deshabilitar si la carrera termina

        Debug.Log("¡Carrera Terminada!");
        // Aquí podrías guardar el tiempo final, mostrar una pantalla de resultados, etc.
    }

    // -------------------------- UI --------------------------

    void UpdateUITexts()
    {
        // Muestra la vuelta actual
        if (lapText != null)
            lapText.text = $"Vuelta: {currentLap}/{totalLaps}";

        // Muestra el tiempo actual (minutos:segundos.milisegundos)
        if (timeText != null)
            timeText.text = "Tiempo: " + FormatTime(currentTime);

        // Muestra el mejor tiempo
        if (bestTimeText != null)
            bestTimeText.text = "Mejor: " + FormatTime(bestLapTime);
    }

    string FormatTime(float time)
    {
        if (time == float.MaxValue) return "---";

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time * 1000) % 1000);

        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }
}