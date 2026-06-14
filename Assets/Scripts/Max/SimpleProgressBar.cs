using UnityEngine;

public class SimpleProgressBar : MonoBehaviour
{
    public float maxWidth = 500f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        maxWidth = rectTransform.sizeDelta.x;
    }

    public void SetProgress(float currentTime, float maxTime)
    {
        if (rectTransform == null) return;

        float progress = maxTime <= 0f ? 0f : Mathf.Clamp01(currentTime / maxTime);

        Vector2 size = rectTransform.sizeDelta;
        size.x = maxWidth * progress;
        rectTransform.sizeDelta = size;
    }
}