using UnityEngine;

public class SimpleProgressBar : MonoBehaviour
{
    public WakeMeter wakeMeter;
    public float maxWidth = 500f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        maxWidth = rectTransform.sizeDelta.x;
    }

    private void Update()
    {
        if (wakeMeter == null) return;

        SetProgress(wakeMeter.currentValue, wakeMeter.maxValue);
    }

    public void SetProgress(float currentValue, float maxValue)
    {
        if (rectTransform == null) return;

        float progress = maxValue <= 0f ? 0f : Mathf.Clamp01(currentValue / maxValue);

        Vector2 size = rectTransform.sizeDelta;
        size.x = maxWidth * progress;
        rectTransform.sizeDelta = size;
    }
}