using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishRaceUI : MonoBehaviour
{
    public GameObject finishPanel;
    public string menuSceneName = "MenuScene";

    public void ShowFinishPanel()
    {
        finishPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RetryRace()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
