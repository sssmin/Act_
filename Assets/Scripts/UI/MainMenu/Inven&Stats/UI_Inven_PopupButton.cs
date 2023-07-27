using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Inven_PopupButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] private TextMeshProUGUI buttonLabelText;
    private UnityAction call;
    public void Init(string buttonLabel, SO_Item item, Action<SO_Item> callback)
    {
        buttonLabelText.text = buttonLabel;
        if (call != null)
            button.onClick.RemoveListener(call);
        call = () => CallbackInvoke(item, callback);
        button.onClick.AddListener(call);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(call);
    }

    void CallbackInvoke(SO_Item item, Action<SO_Item> callback)
    {
        GI.Inst.UIManager.ClosePopup();
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        callback?.Invoke(item);
    }
}
