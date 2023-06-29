using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip_DescText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descText;

    public void Init(string desc)
    {
        descText.text = desc;
    }
}
