using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option : UI_Popup
{
    [SerializeField] private Transform contentUIParent; 
    [SerializeField] private Button closeButton; 
    [SerializeField] private Button soundButton; 
    [SerializeField] private Button displayButton; 
    [SerializeField] private Button bindKeyButton; 
    
    private UI_Option_SoundContent SoundContentUI { get; set; }
    private UI_Option_DisplayContent DisplayContentUI { get; set; }
    private UI_Option_BindKeyContent BindKeyContentUI { get; set; }
    
    public void InitOnce()
    {
        closeButton.onClick.AddListener(CloseOption);
        
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_SoundContent", contentUIParent);
        SoundContentUI = go.GetComponent<UI_Option_SoundContent>();
        SoundContentUI.InitOnce();
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Option_DisplayContent", contentUIParent);
        DisplayContentUI = go.GetComponent<UI_Option_DisplayContent>();
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKeyContent", contentUIParent);
        BindKeyContentUI = go.GetComponent<UI_Option_BindKeyContent>();
        BindKeyContentUI.InitOnce();
    }

    public void OnVisible(Define.EOptionType type)
    {
        SoundContentUI.transform.SetParent(null);
        DisplayContentUI.transform.SetParent(null);
        BindKeyContentUI.transform.SetParent(null);
        
        soundButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);
        displayButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);
        bindKeyButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);

        switch (type)
        {
            case Define.EOptionType.Sound:
                SoundContentUI.transform.SetParent(contentUIParent);
                soundButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                break;
            case Define.EOptionType.Display:
                DisplayContentUI.transform.SetParent(contentUIParent);
                displayButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                break;
            case Define.EOptionType.BindKey:
                BindKeyContentUI.transform.SetParent(contentUIParent);
                bindKeyButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                break;
        }
    }
    
    private void CloseOption()
    {
        GI.Inst.UIManager.PressedCloseButtonOption();
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
    }

    public void OnDisable()
    {
        GI.Inst.SaveSoundData();
        GI.Inst.SaveDisplayData();
        //바인드키 모두 저장
    }

    public void SetVisibleBindKeyPopup(bool isVisible)
    {
        BindKeyContentUI.SetVisibleBindKeyPopup(isVisible);
    }
}
