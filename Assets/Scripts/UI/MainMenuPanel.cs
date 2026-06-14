using UnityEngine;
using UnityEngine.SceneManagement;
using UI; // Подключаем пространство имен, где лежит UIPanel

public class MainMenuPanel : UIPanel
{
    [Header("Panels")]
    [SerializeField] private UIPanel _settingsPanel; // Ссылка на панель настроек

    // Метод, который привяжем к кнопке "Start Game"
    public void OnStartGameClicked()
    {
        // Загружаем сцену с индексом 1 (0 - это обычно главное меню)
        SceneManager.LoadScene(1);
    }

    // Метод, который привяжем к кнопке "Settings"
    public void OnSettingsClicked()
    {
        Hide();            // Прячем главное меню
        _settingsPanel.Show(); // Показываем настройки
    }
}