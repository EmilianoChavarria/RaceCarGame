using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectRacetrackUI : MonoBehaviour
{
    public void CargarPista1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void CargarPista2()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void RegresarMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
