using UnityEngine;
using UnityEngine.UI;
using UI;

public class SettingsPanel : UIPanel
{
    [Header("UI Elements")]
    [SerializeField] private Slider _volumeSlider;
    
    [Header("Navigation")]
    [SerializeField] private UIPanel _parentPanel; // Заменили MainMenuPanel на UIPanel

    public override void Show()
    {
        base.Show();
        _volumeSlider.value = AudioManager.Instance.Volume;
    }

    public void OnVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(value);
    }

    public void OnBackClicked()
    {
        Hide();
        _parentPanel.Show(); // Теперь возвращаемся туда, откуда пришли
    }
}