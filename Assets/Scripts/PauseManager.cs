using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;           // Panel raíz (el que activas/desactivas)
    public GameObject firstSelectedButton;  // botón que se selecciona al abrir (para gamepad/teclado)

    public static bool GameIsPaused { get; private set; } = false;

    // Guarda estado previo del Cursor para restaurarlo
    private CursorLockMode previousLockState;
    private bool previousCursorVisible;

    void Start()
    {
        // Asegurar que el panel esté oculto al inicio
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (GameIsPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (pausePanel == null) return;

        // Guardar estado del cursor
        previousLockState = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        pausePanel.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Pausar audio global (opcional)
        AudioListener.pause = true;

        // Mostrar cursor y permitir UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Seleccionar primer botón para gamepad/teclado
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void Resume()
    {
        if (pausePanel == null) return;

        pausePanel.SetActive(false);

        // Unfreeze
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Reactivar audio global (opcional)
        AudioListener.pause = false;

        // Restaurar cursor
        Cursor.lockState = previousLockState;
        Cursor.visible = previousCursorVisible;

        // Limpia selección del EventSystem
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void RestartLevel()
    {
        // Asegúrate de reactivar tiempo antes de cambiar de escena
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameIsPaused = false;

        SceneManagementReloadCurrent();
    }

    void SceneManagementReloadCurrent()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void LoadMainMenu(string sceneName = "MenuScene")
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameIsPaused = false;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
