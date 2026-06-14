using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UI;
using UnityEngine.InputSystem;

public class PauseMenuPanel : UIPanel
{
    [Header("UI Elements")]
    [SerializeField] private Slider _volumeSlider;

    private CanvasGroup _canvasGroup;
    private bool _isPaused = false;

    private void Awake()
    {
        // Получаем компонент CanvasGroup при старте
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        // При старте уровня убеждаемся, что пауза выключена
        Hide();
    }

    public override void Show()
    {
        // base.Show() нам не нужен, мы управляем видимостью через CanvasGroup
        _canvasGroup.alpha = 1f; // Делаем видимым
        _canvasGroup.interactable = true; // Включаем нажатие кнопок
        _canvasGroup.blocksRaycasts = true; // Включаем перехват кликов
        
        _volumeSlider.value = AudioManager.Instance.Volume;
    }

    public override void Hide()
    {
        _canvasGroup.alpha = 0f; // Делаем невидимым
        _canvasGroup.interactable = false; // Отключаем нажатие кнопок
        _canvasGroup.blocksRaycasts = false; // Отключаем перехват кликов
    }

    private void Update()
    {
        // Проверяем, нажата ли кнопка Esc в НОВОЙ системе ввода
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        Show();
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        Hide();
    }

    public void OnVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(value);
    }

    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}