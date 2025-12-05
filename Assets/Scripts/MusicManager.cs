using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public enum MusicType
    {
        Menu,
        Race
    }

    [Header("Music Type")]
    public MusicType musicType;

    private static MusicManager menuMusicInstance;
    private static MusicManager raceMusicInstance;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Gestionar música según el tipo
        if (musicType == MusicType.Menu)
        {
            // Si ya existe música de menú, destruir esta nueva
            if (menuMusicInstance != null && menuMusicInstance != this)
            {
                Debug.Log("Ya existe música de menú, destruyendo duplicado");
                Destroy(gameObject);
                return;
            }

            // Si existe música de carrera, destruir este objeto de menú
            if (raceMusicInstance != null)
            {
                Debug.Log("Música de carrera activa, no iniciando música de menú");
                Destroy(gameObject);
                return;
            }

            menuMusicInstance = this;
            Debug.Log("Música de MENÚ iniciada");
        }
        else if (musicType == MusicType.Race)
        {
            // Si ya existe música de carrera, destruir esta nueva
            if (raceMusicInstance != null && raceMusicInstance != this)
            {
                Debug.Log("Ya existe música de carrera, destruyendo duplicado");
                Destroy(gameObject);
                return;
            }

            raceMusicInstance = this;
            
            // IMPORTANTE: Detener y destruir la música del menú
            if (menuMusicInstance != null)
            {
                Debug.Log("Deteniendo música de menú y iniciando música de CARRERA");
                Destroy(menuMusicInstance.gameObject);
                menuMusicInstance = null;
            }
            else
            {
                Debug.Log("Música de CARRERA iniciada");
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        // Limpiar referencias al destruirse
        if (menuMusicInstance == this)
        {
            menuMusicInstance = null;
            Debug.Log("Música de menú destruida");
        }
        if (raceMusicInstance == this)
        {
            raceMusicInstance = null;
            Debug.Log("Música de carrera destruida");
        }
    }

    // Método para detener la música del menú desde otros scripts
    public static void StopMenuMusic()
    {
        if (menuMusicInstance != null)
        {
            Destroy(menuMusicInstance.gameObject);
            menuMusicInstance = null;
        }
    }

    // Método para detener la música de carrera y volver al menú
    public static void StopRaceMusic()
    {
        if (raceMusicInstance != null)
        {
            Destroy(raceMusicInstance.gameObject);
            raceMusicInstance = null;
        }
    }

    // Método útil para debugging
    public static void LogCurrentMusic()
    {
        Debug.Log($"Música de Menú: {(menuMusicInstance != null ? "ACTIVA" : "INACTIVA")}");
        Debug.Log($"Música de Carrera: {(raceMusicInstance != null ? "ACTIVA" : "INACTIVA")}");
    }
}
