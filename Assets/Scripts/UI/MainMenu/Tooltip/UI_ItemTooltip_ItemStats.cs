using TMPro;
using UnityEngine;

public class UI_ItemTooltip_ItemStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statNameText;
    [SerializeField] TextMeshProUGUI statValueText;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    public void Init(string name, string value)
    {
        rectTransform.localScale = Vector3.one;
        statNameText.text = name;
        statValueText.text = value;
    }
    
}
