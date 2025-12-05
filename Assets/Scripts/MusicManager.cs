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

    void Awake()
    {
        // Gestionar música según el tipo
        if (musicType == MusicType.Menu)
        {
            if (menuMusicInstance != null && menuMusicInstance != this)
            {
                Destroy(gameObject);
                return;
            }
            menuMusicInstance = this;
        }
        else if (musicType == MusicType.Race)
        {
            if (raceMusicInstance != null && raceMusicInstance != this)
            {
                Destroy(gameObject);
                return;
            }
            raceMusicInstance = this;
            
            // Detener la música del menú cuando empieza la carrera
            if (menuMusicInstance != null)
            {
                Destroy(menuMusicInstance.gameObject);
                menuMusicInstance = null;
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    // Método para detener la música del menú desde otros scripts si es necesario
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
}
