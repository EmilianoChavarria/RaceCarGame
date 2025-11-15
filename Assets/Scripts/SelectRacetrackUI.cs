using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRacetrackUI : MonoBehaviour
{
    public void CargarPista1()
    {
        GameData.selectedTrack = "SampleScene";
        SceneManager.LoadScene("CarSelectScene");
    }

    public void CargarPista2()
    {
        GameData.selectedTrack = "SampleScene";
        SceneManager.LoadScene("CarSelectScene");
    }

    public void RegresarMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
