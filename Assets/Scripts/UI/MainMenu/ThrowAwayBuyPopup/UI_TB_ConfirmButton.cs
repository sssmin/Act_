using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TB_ConfirmButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    
    public void Init(Item item, Action<Item> callback, string inButtonText)
    {
        buttonText.text = inButtonText;
        button.onClick.AddListener(() => CallbackInvoke(item, callback));
    }
   
    void CallbackInvoke(Item item, Action<Item> callback)
    {
        GI.Inst.UIManager.ClosePopup();
        callback?.Invoke(item);
    }
}
