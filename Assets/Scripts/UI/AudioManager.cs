using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _musicSource;
    
    // Теперь мы читаем громкость с "ушей" игры (AudioListener), а не с плеера
    public float Volume => AudioListener.volume; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Оставляем это, чтобы музыка не останавливалась при паузе
        _musicSource.ignoreListenerPause = true; 
    }

    public void PlayMusic()
    {
        if (_musicSource.isPlaying) return;
        _musicSource.Play();
    }

    public void SetVolume(float volume)
    {
        // Меняем МАСТЕР-громкость всей игры! Теперь она влияет на всё.
        AudioListener.volume = volume;
    }
}