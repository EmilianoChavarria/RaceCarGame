using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public FadePanel fadePanel;


    public void PlayGame()
    {
        fadePanel.FadeOutToScene("CarSelectScene");
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CreditScene()
    {
        fadePanel.FadeOutToScene("CreditScene");
    }
}
