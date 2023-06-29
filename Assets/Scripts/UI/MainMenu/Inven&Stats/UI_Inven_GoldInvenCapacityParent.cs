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
        GoldInvenCapacty goldInvenCapacty = GI.Inst.ListenerManager.GetGoldInvenCapacity();
        goldAmountText.text = goldInvenCapacty.gold.ToString("#,0");
        currentCapacityText.text = goldInvenCapacty.currentInvenNum.ToString("#,0");
        maxCapacityText.text = goldInvenCapacty.maxInvenNum.ToString("#,0");
    }
}
