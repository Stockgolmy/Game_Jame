using UnityEngine;
using UnityEngine.UI; // Нужен для Slider
using UI;

public class SettingsPanel : UIPanel
{
    [Header("UI Elements")]
    [SerializeField] private Slider _volumeSlider;
    
    [Header("Panels")]
    [SerializeField] private UIPanel _mainMenuPanel;

    // Переопределяем метод Show, чтобы при открытии слайдер отображал актуальную громкость
    public override void Show()
    {
        base.Show();
        _volumeSlider.value = AudioManager.Instance.Volume;
    }

    // Метод, который привяжем к слайдеру (On Value Changed)
    public void OnVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(value);
    }

    // Метод, который привяжем к кнопке "Back"
    public void OnBackClicked()
    {
        Hide();
        _mainMenuPanel.Show();
    }
}