using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip_ItemStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statNameText;
    [SerializeField] TextMeshProUGUI statValueText;

    public void Init(string name, string value)
    {
        statNameText.text = name;
        statValueText.text = value;
    }
    
}
