using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RestartManager : MonoBehaviour
{
    [Header("UI Text")]
    public TMP_Text restartText;
    
    [Header("Settings")]
    public KeyCode restartKey = KeyCode.R;
    public string restartMessage = "Presiona R para reiniciar";
    
    void Start()
    {
        if (restartText != null)
        {
            restartText.text = restartMessage;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(restartKey))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        // Asegurar que el tiempo esté normal antes de reiniciar
        Time.timeScale = 1f;
        AudioListener.pause = false;
        
        // Recargar la escena actual
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
