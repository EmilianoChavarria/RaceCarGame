using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCarkUI : MonoBehaviour
{
    public void CargarCarro1()
    {
        GameData.selectedCar = "Carro1";
        SceneManager.LoadScene("SampleScene");
    }

    public void CargarCarro2()
    {
        GameData.selectedCar = "Carro2";
        SceneManager.LoadScene("SampleScene");
    }

    public void RegresarMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
