using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _musicSource;
    
    public float Volume => _musicSource.volume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ВОТ ОНА, ЭТА СТРОЧКА! Запрещаем паузе останавливать эту музыку
        _musicSource.ignoreListenerPause = true; 
    }

    public void PlayMusic()
    {
        if (_musicSource.isPlaying) return;
        _musicSource.Play();
    }

    public void SetVolume(float volume)
    {
        _musicSource.volume = volume;
    }
}