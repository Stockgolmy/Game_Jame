using UnityEngine;
using UnityEngine.SceneManagement;

public class WakeMeter : MonoBehaviour
{
    public static WakeMeter Instance;

    public float speed = 1f;
    public float multiplier = 1f;
    public float maxValue = 100f;
    public float minValue = 0f;
    public float currentValue = 0f;

    public bool timerEnded = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetMeter();
    }

    private void Update()
    {
        if (timerEnded) return;

        currentValue += speed * multiplier * Time.deltaTime;
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);

        Debug.Log($"multiplier={multiplier}, currentValue={currentValue}");

        if (multiplier <= 1.01f && currentValue > 3f)
        {
            Debug.Log("УСЛОВИЕ NEXT LEVEL СРАБОТАЛО");

            ResetMeter();

            if (LevelManager.Instance != null)
            {
                Debug.Log("LevelManager OK, вызываю NextLevel");
                LevelManager.Instance.NextLevel();
            }
            else
            {
                Debug.LogError("LevelManager.Instance == null");
            }

            return;
        }

        if (currentValue >= maxValue)
        {
            EndTimer();
        }
    }

    public void IncreaseMultiplier(float amount = 1f)
    {
        multiplier += amount;
    }

    public void DecreaseMultiplier(float amount = 1f)
    {
        multiplier = Mathf.Max(1f, multiplier - amount);
    }

    public void AddValue(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, minValue, maxValue);

        if (currentValue >= maxValue)
        {
            EndTimer();
        }
    }

    public void RemoveValue(float amount)
    {
        currentValue = Mathf.Clamp(currentValue - amount, minValue, maxValue);
    }

    public void ResetMeter()
    {
        currentValue = minValue;
        timerEnded = false;
    }

    public void EndTimer()
    {
        if (timerEnded) return;

        timerEnded = true;
        SceneManager.LoadScene("MainMenu");
    }
}