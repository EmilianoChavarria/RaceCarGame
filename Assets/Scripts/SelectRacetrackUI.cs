using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRacetrackUI : MonoBehaviour
{
    public void CargarPista1()
    {
        GameData.selectedTrack = "F8DisplayScene";
        SceneManager.LoadScene("F8DisplayScene");
    }

    public void CargarPista2()
    {
        GameData.selectedTrack = "CoastalTrackDisplayScene";
        SceneManager.LoadScene("CoastalTrackDisplayScene");
    }

    public void RegresarMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
