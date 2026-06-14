using UnityEngine;
using UnityEngine.UI;

public class SleepyEyeEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WakeMeter wakeMeter;
    
    [Header("Eyelids UI")]
    [SerializeField] private RectTransform topEyelid;   // Верхнее веко
    [SerializeField] private RectTransform bottomEyelid; // Нижнее веко
    
    [Header("Effect Settings")]
    [SerializeField] private float minOpenness = 0.1f;   // Насколько закрыт глаз при 0% бара (0.1 = 10% открыт)
    [SerializeField] private float maxLidHeight = 500f;  // Максимальная высота века в пикселях (при полностью закрытом глазе)
    
    [Header("Blink Settings")]
    [SerializeField] private float blinkSpeed = 10f;     // Скорость моргания
    [SerializeField] private float closedDuration = 0.1f;// Сколько времени глаз закрыт при моргании
    [SerializeField] private float minBlinkInterval = 1f;// Интервал моргания уставшего (0% бара)
    [SerializeField] private float maxBlinkInterval = 8f;// Интервал моргания бодрого (100% бара)

    private float blinkTimer;
    private float blinkStateTimer;
    
    // Состояния моргания: 0 - открыт, ждет моргания; 1 - закрывается; 2 - закрыт; 3 - открывается
    private int blinkState = 0; 

    private void Update()
    {
        if (wakeMeter == null || topEyelid == null || bottomEyelid == null) return;

        // Считываем прогресс (от 0 до 1)
        float progress = Mathf.Clamp01(wakeMeter.currentValue / wakeMeter.maxValue);

        // Базовая открытость глаза (без учета моргания)
        // При progress = 0, openness = minOpenness. При progress = 1, openness = 1.
        float baseOpenness = Mathf.Lerp(minOpenness, 1f, progress);

        // Логика моргания
        float currentOpenness = HandleBlinking(progress, baseOpenness);

        // Применяем открытость к UI
        UpdateEyelids(currentOpenness);
    }

    private float HandleBlinking(float progress, float baseOpenness)
{
    // Если бодрость максимальная — не моргаем вообще
    if (progress >= 0.99f)
    {
        blinkState = 0;
        return baseOpenness;
    }

    // Вычисляем интервал один раз до switch, чтобы компилятор был спокоен
    // Чем больше бодрости (progress), тем реже моргаем
    float currentInterval = Mathf.Lerp(minBlinkInterval, maxBlinkInterval, progress);

    float currentOpenness = baseOpenness;

    switch (blinkState)
    {
        case 0: // Ждем момента для моргания
            blinkTimer -= Time.deltaTime;
            
            if (blinkTimer <= 0f)
            {
                blinkState = 1; // Начинаем закрывать
                blinkStateTimer = 0f;
            }
            break;

        case 1: // Закрываем глаз
            blinkStateTimer += Time.deltaTime * blinkSpeed;
            currentOpenness = Mathf.Lerp(baseOpenness, 0f, blinkStateTimer);
            
            if (blinkStateTimer >= 1f)
            {
                blinkState = 2; // Глаз полностью закрыт
                blinkStateTimer = 0f;
                currentOpenness = 0f;
            }
            break;

        case 2: // Держим глаз закрытым
            blinkStateTimer += Time.deltaTime;
            currentOpenness = 0f;
            
            if (blinkStateTimer >= closedDuration)
            {
                blinkState = 3; // Начинаем открывать
                blinkStateTimer = 0f;
            }
            break;

        case 3: // Открываем глаз обратно
            blinkStateTimer += Time.deltaTime * blinkSpeed;
            currentOpenness = Mathf.Lerp(0f, baseOpenness, blinkStateTimer);
            
            if (blinkStateTimer >= 1f)
            {
                blinkState = 0; // Вернулись в ожидание
                // Сбрасываем таймер на рандомную дельту, чтобы моргание не было как метроном
                blinkTimer = currentInterval + Random.Range(-0.5f, 0.5f); 
                currentOpenness = baseOpenness;
            }
            break;
    }

    return currentOpenness;
}

    private void UpdateEyelids(float openness)
    {
        // openness 1 = веки спрятаны (высота 0), openness 0 = веки вытянуты на всю ширину
        float targetHeight = maxLidHeight * (1f - openness);

        topEyelid.sizeDelta = new Vector2(topEyelid.sizeDelta.x, targetHeight);
        bottomEyelid.sizeDelta = new Vector2(bottomEyelid.sizeDelta.x, targetHeight);
    }
}