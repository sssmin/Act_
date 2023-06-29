using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Merchant_ItemLine : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Button buyButton;
    private Item tempItem;
    
    public void Init(Item item)
    {
        tempItem = item;
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;
        buyButton.onClick.AddListener(OnClickBuyButton);
    }

    public void OnClickBuyButton()
    {
        GI.Inst.UIManager.VisibleTBPopup(EThrowawayBuyPopupType.Buy, tempItem);
    }
}
