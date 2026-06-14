using UnityEngine;
using UnityEngine.UI;

public class SimpleProgressBar : MonoBehaviour
{
    public Image fillImage;

    public void SetProgress(float currentTime, float maxTime)
    {
        if (fillImage == null) return;

        if (maxTime <= 0f)
        {
            fillImage.fillAmount = 0f;
            return;
        }

        fillImage.fillAmount = Mathf.Clamp01(currentTime / maxTime);
    }
}