using UnityEngine;

public class PersistentMusic : MonoBehaviour
{
    private static PersistentMusic instance;

    void Awake()
    {
        // Si ya existe una instancia de este objeto, destruir el duplicado
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Esta es la primera instancia, marcarla como persistente
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
