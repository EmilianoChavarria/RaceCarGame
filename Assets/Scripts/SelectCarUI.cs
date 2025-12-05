using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCarUI : MonoBehaviour
{
    public void SeleccionarCarro1()
    {
        GameData.selectedCar = "Carro1";
        SceneManager.LoadScene("RacetrackSelectScene");
    }

    public void SeleccionarCarro2()
    {
        GameData.selectedCar = "Carro2";
        SceneManager.LoadScene("RacetrackSelectScene");
    }

    public void RegresarMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
