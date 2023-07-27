using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform currentHealthBar;
    [SerializeField] private RectTransform lateHealthBar;
    [SerializeField] private Image healthBarCurrentImage;
    [SerializeField] private Image healthBarLateImage;
    [SerializeField] private Image healthBarBgImage;
    private const float HEALTH_BAR_MAX_WIDTH = 380f;
    private Coroutine CoLate;
    
    
    public void SetBar(float ratio)
    {
        float xValue = HEALTH_BAR_MAX_WIDTH * ratio * 0.01f;
        currentHealthBar.sizeDelta = new Vector2(xValue, currentHealthBar.sizeDelta.y);
        if (CoLate != null)
            StopCoroutine(CoLate);
        CoLate = StartCoroutine(CoHpBarLateUpdate(xValue));
    }

    IEnumerator CoHpBarLateUpdate(float xValue)
    {
        while (true)
        {
            lateHealthBar.sizeDelta = new Vector2(Mathf.Lerp(lateHealthBar.sizeDelta.x, xValue, Time.deltaTime),
                lateHealthBar.sizeDelta.y);
            yield return null;

            if (lateHealthBar.sizeDelta.x - xValue <= 5f)
                break;
        }    
        lateHealthBar.sizeDelta = new Vector2(xValue, currentHealthBar.sizeDelta.y);
        CoLate = null;
    }

    public void VisibleUI()
    {
        healthBarCurrentImage.color = new Color(healthBarCurrentImage.color.r, healthBarCurrentImage.color.g,
            healthBarCurrentImage.color.b, 1f);
        healthBarLateImage.color = new Color(healthBarLateImage.color.r, healthBarLateImage.color.g,
            healthBarLateImage.color.b, 1f);
        healthBarBgImage.color = new Color(healthBarBgImage.color.r, healthBarBgImage.color.g,
            healthBarBgImage.color.b, 1f);
    }

    public void InvisibleUI()
    {
        healthBarCurrentImage.color = new Color(healthBarCurrentImage.color.r, healthBarCurrentImage.color.g,
            healthBarCurrentImage.color.b, 0f);
        healthBarLateImage.color = new Color(healthBarLateImage.color.r, healthBarLateImage.color.g,
            healthBarLateImage.color.b, 0f);
        healthBarBgImage.color = new Color(healthBarBgImage.color.r, healthBarBgImage.color.g,
            healthBarBgImage.color.b, 0f);
    }
}
