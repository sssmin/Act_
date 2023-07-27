using System.Collections;
using UnityEngine;

public class UI_Main_HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform HealthBarLate;
    [SerializeField] private RectTransform HealthBarCurrent;
    private const float HEALTH_BAR_MAX_WIDTH = 380f;
    private Coroutine CoLate;
    
    //몬스터는 자신한테 붙어있는 UI에 HP 정보를 보내야함. 
    
    public void SetBar(float ratio)
    {
        float xValue = HEALTH_BAR_MAX_WIDTH * ratio * 0.01f;
        HealthBarCurrent.sizeDelta = new Vector2(xValue, HealthBarCurrent.sizeDelta.y);
        if (CoLate != null)
            StopCoroutine(CoLate);
        CoLate = StartCoroutine(CoHpBarLateUpdate(xValue));
    }

    IEnumerator CoHpBarLateUpdate(float xValue)
    {
        while (true)
        {
            HealthBarLate.sizeDelta = new Vector2(Mathf.Lerp(HealthBarLate.sizeDelta.x, xValue, Time.deltaTime),
                HealthBarLate.sizeDelta.y);
            yield return null;

            if (HealthBarLate.sizeDelta.x - xValue <= 5f)
                break;
        }    
        HealthBarLate.sizeDelta = new Vector2(xValue, HealthBarCurrent.sizeDelta.y);
        CoLate = null;
    }
}
