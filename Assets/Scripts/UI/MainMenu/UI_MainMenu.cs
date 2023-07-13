using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_MainMenu : UI_Popup
{
    [SerializeField] private Transform contentUIParent; 
    [SerializeField] private Button closeButton; 
    [SerializeField] private Button inventoryButton; 
    [SerializeField] private Button skillButton; 
    private UI_Inven_StatsContent InvenStatsContentUI { get; set; }
    private UI_SkillContent SkillContentUI { get; set; }
    

    public void InitOnce()
    {
        closeButton.onClick.AddListener(CloseMainMenu);
        
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_StatsContent", contentUIParent);
        InvenStatsContentUI = go.GetComponent<UI_Inven_StatsContent>();
        InvenStatsContentUI.InitOnce();
       
        go = GI.Inst.ResourceManager.Instantiate("UI_Skill_SkillContent", contentUIParent);
        SkillContentUI = go.GetComponent<UI_SkillContent>();
        SkillContentUI.InitOnce();
    }

    public void OnVisible(Define.EMainMenuType type)
    {
        InvenStatsContentUI.transform.SetParent(null);
        SkillContentUI.transform.SetParent(null);
        
        inventoryButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);
        skillButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);

        switch (type)
        {
            case Define.EMainMenuType.Inventory:
                InvenStatsContentUI.transform.SetParent(contentUIParent);
                InvenStatsContentUI.AttachToInventory();
                inventoryButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                break;
            case Define.EMainMenuType.Skill:
                Debug.Log("스킬창3");
                SkillContentUI.transform.SetParent(contentUIParent);
                skillButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                break;
        }
    }

    private void CloseMainMenu()
    {
        GI.Inst.UIManager.PressedCloseButtonMainMenu();
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
    }
    
    public void RefreshEquipPassiveSkillUI(List<SO_Skill> skills)
    {
        SkillContentUI.RefreshEquipPassiveSkillUI(skills);
    }
    
    public void RefreshPassiveSkillUI(List<SO_PassiveSkill> skills)
    {
        SkillContentUI.RefreshPassiveSkillUI(skills);
    }
}
