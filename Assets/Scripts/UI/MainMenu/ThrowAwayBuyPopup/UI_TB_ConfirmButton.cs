using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_TB_ConfirmButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    private UnityAction action;
    
    public void Init(Item item, Action<Item> callback, string inButtonText)
    {
        buttonText.text = inButtonText;
        action = () => CallbackInvoke(item, callback);
        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(action);
    }

    void CallbackInvoke(Item item, Action<Item> callback)
    {
        GI.Inst.UIManager.ClosePopup();
        callback?.Invoke(item);
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
    }
}
