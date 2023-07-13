using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Inven_GoldInvenCapacityParent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldAmountText;
    [SerializeField] private TextMeshProUGUI currentCapacityText;
    [SerializeField] private TextMeshProUGUI maxCapacityText;

    public void RefreshGoldInvenCapacityUI()
    {
        GoldInvenCapacity goldInvenCapacity = GI.Inst.ListenerManager.GetGoldInvenCapacity();
        goldAmountText.text = goldInvenCapacity.gold.ToString("#,0");
        currentCapacityText.text = goldInvenCapacity.currentInvenNum.ToString("#,0");
        maxCapacityText.text = goldInvenCapacity.maxInvenNum.ToString("#,0");
    }
}
