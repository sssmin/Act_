using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TB_Price : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;

    public void Init(string price)
    {
        priceText.text = price;
    }
   
}
