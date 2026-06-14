using UnityEngine;

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
        //multiplier = 1f;
        timerEnded = false;
    }

    public void EndTimer()
    {
        if (timerEnded) return;

        timerEnded = true;
        
    //    if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }
}