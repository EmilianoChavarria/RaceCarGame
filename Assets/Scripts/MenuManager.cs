using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public FadePanel fadePanel;

    public void PlayGame()
    {
        fadePanel.FadeOutToScene("RacetrackSelectScene");
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
