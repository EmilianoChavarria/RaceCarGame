using UnityEngine;
using UnityEngine.SceneManagement;

public class SeleccionPistaUI : MonoBehaviour
{
    public void CargarPista1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void CargarPista2()
    {
        SceneManager.LoadScene("Racetrack2");
    }

    public void RegresarMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
