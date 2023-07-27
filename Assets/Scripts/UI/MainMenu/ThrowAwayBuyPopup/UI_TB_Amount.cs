using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_TB_Amount : MonoBehaviour
{
    [SerializeField] Button minButton;
    [SerializeField] Button maxButton;
    [SerializeField] Button subButton;
    [SerializeField] Button addButton;
    [SerializeField] TMP_InputField amountInputField;
    private SO_Item tempItem;
    private GoldInvenCapacity _goldInvenCapacity;
    public int Amount { get; private set; }
    private EThrowawayBuyPopupType popupType;
    private Action<string> buyCallback;

    public void Init(SO_Item item, EThrowawayBuyPopupType type)
    {
        tempItem = item;
        popupType = type;
        RemoveListener();
        minButton.onClick.AddListener(OnClickMinButton);
        maxButton.onClick.AddListener(OnClickMaxButton);
        subButton.onClick.AddListener(OnClickSubButton);
        addButton.onClick.AddListener(OnClickAddButton);
        amountInputField.text = "0";
        amountInputField.onValueChanged.AddListener(OnValueChanged);
        amountInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        _goldInvenCapacity = GI.Inst.ListenerManager.GetGoldInvenCapacity();
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    private void RemoveListener()
    {
        minButton.onClick.RemoveListener(OnClickMinButton);
        maxButton.onClick.RemoveListener(OnClickMaxButton);
        subButton.onClick.RemoveListener(OnClickSubButton);
        addButton.onClick.RemoveListener(OnClickAddButton);
        amountInputField.onValueChanged.RemoveListener(OnValueChanged);
    }

    public void BuyInit(SO_Item item, EThrowawayBuyPopupType type, Action<string> callback)
    {
        Init(item, type);
        buyCallback = callback;
    }

    private void OnClickMinButton()
    {
        amountInputField.text = "0";
        Amount = 0;
    }
    private void OnClickMaxButton()
    {
        switch (popupType)
        {
            case EThrowawayBuyPopupType.ThrowAway:
                amountInputField.text = tempItem.amount.ToString("#,0");
                Amount = tempItem.amount;
                break;
            case EThrowawayBuyPopupType.Buy:
                int price = ((SO_Consumable)tempItem).storeSellPrice;
                int canBuyAmount = _goldInvenCapacity.gold / price;
                amountInputField.text = canBuyAmount.ToString("#,0");

                int maxPrice = canBuyAmount * price;
                buyCallback?.Invoke(maxPrice.ToString("#,0"));
                Amount = canBuyAmount;
                break;
        }
    }
    private void OnClickSubButton()
    {
        Amount = Mathf.Clamp(--Amount, 0, tempItem.amount);
        amountInputField.text = Amount.ToString("#,0");
        
        int price = ((SO_Consumable)tempItem).storeSellPrice;
        int maxPrice = Amount * price;
        buyCallback?.Invoke(maxPrice.ToString("#,0"));
        
    }
    private void OnClickAddButton()
    {
        switch (popupType)
        {
            case EThrowawayBuyPopupType.ThrowAway:
                Amount = Mathf.Clamp(++Amount, 0, tempItem.amount);
                amountInputField.text = Amount.ToString("#,0");
                break;
            case EThrowawayBuyPopupType.Buy:
                int tempAmount = Amount + 1;
                
                int price = ((SO_Consumable)tempItem).storeSellPrice;
                if ((price * tempAmount) > _goldInvenCapacity.gold)
                {
                    int canBuyAmount = _goldInvenCapacity.gold / price;
                    amountInputField.text = canBuyAmount.ToString("#,0");
                    Amount = canBuyAmount;
                }
                else
                {
                    ++Amount;
                    amountInputField.text = Amount.ToString("#,0");
                }
                
                int maxPrice = Amount * price;
                buyCallback?.Invoke(maxPrice.ToString("#,0"));
                break;
        }
    }

    private void OnValueChanged(string value)
    {
        bool success = int.TryParse(value, out int valueToInt);
        Amount = valueToInt;
        if (success)
        {
            switch (popupType)
            {
                case EThrowawayBuyPopupType.ThrowAway:
                    //가진 수량에 맞춰야 함
                    if (valueToInt > tempItem.amount)
                    {
                        amountInputField.text = tempItem.amount.ToString("#,0");
                        Amount = tempItem.amount;
                    }
                    break;
                case EThrowawayBuyPopupType.Buy:
                    //골드에 맞춰야함
                    int price = ((SO_Consumable)tempItem).storeSellPrice;
                    if ((price * tempItem.amount) > _goldInvenCapacity.gold)
                    {
                        int canBuyAmount = _goldInvenCapacity.gold / price;
                        amountInputField.text = canBuyAmount.ToString("#,0");
                        Amount = canBuyAmount;
                    }
                    
                    int maxPrice = Amount * price;
                    buyCallback?.Invoke(maxPrice.ToString("#,0"));
                    break;
            }
        }
    }
}
