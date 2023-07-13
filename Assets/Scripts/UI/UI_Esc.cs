using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Esc : UI_Popup
{
    [SerializeField] private Button optionButton;
    [SerializeField] private Button saveGameButton;
    [SerializeField] private TextMeshProUGUI saveText;
    [SerializeField] private Button goToTitleButton;
    [SerializeField] private Button gameExitButton;
    [SerializeField] private Button closeButton;
    
    void Start()
    {
        optionButton.onClick.AddListener(OnClickOptionButton);
        saveGameButton.onClick.AddListener(OnClickSaveGameButton);
        goToTitleButton.onClick.AddListener(OnClickGoToTitleButton);
        gameExitButton.onClick.AddListener(OnClickGameExitButton);
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "Town")
        {
            saveGameButton.interactable = false;
            saveText.color = new Color(224f / 255f, 224f / 255f, 224f / 255f, 100f / 255f);
        }
        else
        {
            saveGameButton.interactable = true;
            saveText.color = new Color(224f / 255f, 224f / 255f, 224f / 255f, 255f / 255f);
        }
    }

    public void OnClickOptionButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.UIManager.VisibleOption(Define.EOptionType.Sound);
    }
    
    public void OnClickSaveGameButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.SaveGameData();
    }
    
    public void OnClickGoToTitleButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.UIManager.InvisibleEsc(false);
        GI.Inst.SceneLoadManager.GoToTitle();
    }
    
    public void OnClickGameExitButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        Application.Quit();
    }
    
    public void OnClickCloseButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
        GI.Inst.UIManager.InvisibleEsc(true);
    }
}
