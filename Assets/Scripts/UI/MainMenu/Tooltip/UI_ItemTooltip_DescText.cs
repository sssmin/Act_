using System;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip_DescText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descText;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init(string desc)
    {
        rectTransform.localScale = Vector3.one;
        string[] sub = desc.Split("Y");

        if (sub.Length > 1)
        {
            descText.text = sub[1];
            descText.color = Color.yellow;
        }
        else
        {
            descText.text = desc;
            descText.color = Color.white;
        }

    }
}
