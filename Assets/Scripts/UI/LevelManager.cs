using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    public GameObject[] levelPrefabs; // Массив наших префабов (Level1, Level2, Level3)
    public Transform spawnPoint;      // Точка, где будут появляться будильники (поставь на полку)

    [Header("References")]
    public WakeMeter wakeMeter;       // Наша шкала бодрости

    private int currentLevel = 0;
    private GameObject currentLevelInstance;

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
        StartLevel(0); // Начинаем с нулевого уровня
    }

    public void StartLevel(int levelIndex)
    {
        // Если уровни закончились
        if (levelIndex >= levelPrefabs.Length)
        {
            Debug.Log("ИГРА ПРОЙДЕНА!");
            // Тут можно вызвать экран победы
            return;
        }

        currentLevel = levelIndex;

        // Удаляем старые будильники, если они были
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        // Спавним новые будильники
        currentLevelInstance = Instantiate(levelPrefabs[currentLevel], spawnPoint.position, spawnPoint.rotation, spawnPoint);

        // Сбрасываем бодрость в 0, чтобы игрок снова "уснул"
        if (wakeMeter != null)
        {
            wakeMeter.currentValue = 0f;
        }
    }

    // Метод для перехода на следующий уровень
    public void NextLevel()
    {
        StartLevel(currentLevel + 1);
    }
}