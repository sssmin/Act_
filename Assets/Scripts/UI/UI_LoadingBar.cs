using TMPro;
using UnityEngine;

public class UI_LoadingBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillImageRectTransform;
    [SerializeField] private TextMeshProUGUI percentageText;

    public void SetBar(float per)
    {
        fillImageRectTransform.sizeDelta = new Vector2(per * 1000f, fillImageRectTransform.sizeDelta.y);
        int floor = Mathf.FloorToInt(per * 100f);
        percentageText.text = $"{floor}%";
    }
}
