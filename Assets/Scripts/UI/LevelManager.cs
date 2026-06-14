using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    public GameObject[] levels;

    private int currentLevel = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ActivateOnlyLevel(0);
    }

    public void NextLevel()
    {
        LoadLevel(currentLevel + 1);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= levels.Length)
        {
            Debug.Log("ПОБЕДА! ВСЕ УРОВНИ ПРОЙДЕНЫ!");
            return;
        }

        if (currentLevel >= 0 && currentLevel < levels.Length && levels[currentLevel] != null)
        {
            levels[currentLevel].SetActive(false);
        }

        currentLevel = levelIndex;

        if (levels[currentLevel] != null)
        {
            levels[currentLevel].SetActive(true);
        }

        if (WakeMeter.Instance != null)
        {
            WakeMeter.Instance.ResetMeter();
        }

        Debug.Log("Загружен уровень: " + levels[currentLevel].name);
    }

    private void ActivateOnlyLevel(int levelIndex)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] != null)
            {
                levels[i].SetActive(i == levelIndex);
            }
        }

        currentLevel = levelIndex;
    }
}