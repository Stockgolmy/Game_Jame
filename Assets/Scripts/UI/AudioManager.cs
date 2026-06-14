using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Синглтон для удобного доступа из любого места
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _musicSource;
    
    public float Volume => _musicSource.volume; // Свойство для чтения текущей громкости

    private void Awake()
    {
        // Реализация синглтона
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Чтобы музыка не прерывалась при загрузке уровня
    }

    // Метод для изменения громкости (вызывается из UI)
    public void SetVolume(float volume)
    {
        _musicSource.volume = volume;
    }
}