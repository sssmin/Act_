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

    private bool IsMainMenu { get; set; }
    
    public void InitOnce()
    {
        closeButton.onClick.RemoveListener(CloseOption);
        closeButton.onClick.AddListener(CloseOption);
        
        GI.Inst.UIManager.SetNormalButtonColorPreset(soundButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(displayButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(bindKeyButton);
        
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Option_SoundContent", contentUIParent);
        SoundContentUI = go.GetComponent<UI_Option_SoundContent>();
        SoundContentUI.InitOnce();
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Option_DisplayContent", contentUIParent);
        DisplayContentUI = go.GetComponent<UI_Option_DisplayContent>();
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Option_BindKeyContent", contentUIParent);
        BindKeyContentUI = go.GetComponent<UI_Option_BindKeyContent>();
        BindKeyContentUI.InitOnce();
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(CloseOption);
    }

    public void OnVisible(Define.EOptionType type)
    {
        OnVisible(type, IsMainMenu);
    }

    public void OnVisible(Define.EOptionType type, bool isMainMenu)
    {
        IsMainMenu = isMainMenu;
        closeButton.gameObject.SetActive(IsMainMenu);
        SoundContentUI.transform.SetParent(null);
        DisplayContentUI.transform.SetParent(null);
        BindKeyContentUI.transform.SetParent(null);

        soundButton.interactable = true;
        displayButton.interactable = true;
        bindKeyButton.interactable = true;

        switch (type)
        {
            case Define.EOptionType.Sound:
                SoundContentUI.transform.SetParent(contentUIParent);
                soundButton.interactable = false;
                break;
            case Define.EOptionType.Display:
                DisplayContentUI.transform.SetParent(contentUIParent);
                displayButton.interactable = false;
                break;
            case Define.EOptionType.BindKey:
                BindKeyContentUI.transform.SetParent(contentUIParent);
                bindKeyButton.interactable = false;
                break;
        }
    }
    
    private void CloseOption()
    {
        GI.Inst.UIManager.ClosePopup();
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
    }

    public void OnDisable()
    {
        GI.Inst.SaveSoundData();
        GI.Inst.SaveDisplayData();
    }

    public void SetVisibleBindKeyPopup(bool isVisible)
    {
        BindKeyContentUI.SetVisibleBindKeyPopup(isVisible);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
