using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Inven_PopupButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] private TextMeshProUGUI buttonLabelText;
    private UnityAction call;
    public void Init(string buttonLabel, Item item, Action<Item> callback)
    {
        buttonLabelText.text = buttonLabel;
        if (call != null)
            button.onClick.RemoveListener(call);
        call = () => CallbackInvoke(item, callback);
        button.onClick.AddListener(call);
    }

    void CallbackInvoke(Item item, Action<Item> callback)
    {
        GI.Inst.UIManager.ClosePopup();
        callback?.Invoke(item);
    }
}
