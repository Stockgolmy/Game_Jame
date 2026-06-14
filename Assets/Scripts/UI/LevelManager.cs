using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    public GameObject[] levelPrefabs; // Сюда закинь префабы Level1_Clocks, Level2_Clocks
    public Transform spawnPoint;      // Пустышка на полке

    private int currentLevel = 0;
    private GameObject currentLevelInstance;
    
    // ФЛАГ ЗАЩИТЫ: не дает Update вызвать следующий уровень дважды
    private bool waitingForNextLevel = false; 

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        LoadLevel(0);
    }

    private void Update()
    {
        // Если мы уже идем на следующий уровень или уровня нет — выходим
        if (waitingForNextLevel || currentLevelInstance == null) return;

        bool hasActive = false;

        // Проверяем детей текущего уровня
        foreach (Transform child in currentLevelInstance.transform)
        {
            // Если будильник просто скрывается (SetActive(false))
            if (child.gameObject.activeSelf)
            {
                hasActive = true;
                break;
            }
        }

        // Если будильники УДАЛЯЮТСЯ (Destroy), то количество детей станет 0
        if (currentLevelInstance.transform.childCount == 0)
        {
            hasActive = false;
        }

        // Если живых будильников не осталось
        if (!hasActive)
        {
            waitingForNextLevel = true; // БЛОКИРУЕМ повторный вызов!
            NextLevel();
        }
    }

    public void LoadLevel(int levelIndex)
    {
        // Если уровни закончились — Победа!
        if (levelIndex >= levelPrefabs.Length)
        {
            Debug.Log("ПОБЕДА! ВСЕ УРОВНИ ПРОЙДЕНЫ!");
            return;
        }

        currentLevel = levelIndex;

        // Скрываем/Удаляем старый уровень
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        // Раскрываем/Спавним новый уровень
        currentLevelInstance = Instantiate(levelPrefabs[currentLevel], spawnPoint.position, spawnPoint.rotation, spawnPoint);

        // Сбрасываем WakeMeter, чтобы глаза снова закрылись
        if (WakeMeter.Instance != null)
        {
            WakeMeter.Instance.currentValue = 0f;
            WakeMeter.Instance.timerEnded = false; 
        }

        waitingForNextLevel = false; // Снимаем блокировку
    }

    public void NextLevel()
    {
        LoadLevel(currentLevel + 1);
    }
}