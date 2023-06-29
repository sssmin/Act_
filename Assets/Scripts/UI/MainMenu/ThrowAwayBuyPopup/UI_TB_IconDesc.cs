using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TB_IconDesc : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI descText;
    
    public void Init(Sprite icon, string desc)
    {
        iconImage.sprite = icon;
        descText.text = desc;
    }
}
