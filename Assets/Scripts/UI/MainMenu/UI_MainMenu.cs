using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    //Inventory/StatsContent_UI
    //SkillContent_UI
    //OptionContent_UI 3개 생성 후 초기화만 진행
    [SerializeField] private Transform contentUIParent; 
    [SerializeField] private Button closeButton; 
    [SerializeField] private Button InventoryButton; 
    [SerializeField] private Button SkillButton; 
    [SerializeField] private Button OptionButton; 
    private UI_Inven_StatsContent InvenStatsContent { get; set; }
    private UI_SkillContent SkillContent { get; set; }
    
    
    public void InitOnce()
    {
        closeButton.onClick.AddListener(CloseMainMenu);
        
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_StatsContent", contentUIParent);
        InvenStatsContent = go.GetComponent<UI_Inven_StatsContent>();
        InvenStatsContent.InitOnce();
       
        go = GI.Inst.ResourceManager.Instantiate("UI_Skill_SkillContent");
        SkillContent = go.GetComponent<UI_SkillContent>();
        SkillContent.InitOnce();
    }

    public void OnVisible(Define.EMainMenuType type)
    {
        InvenStatsContent.transform.SetParent(null);
        SkillContent.transform.SetParent(null);
        
        InventoryButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);
        SkillButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);

        switch (type)
        {
            case Define.EMainMenuType.Inventory:
                InvenStatsContent.transform.SetParent(contentUIParent);
                InvenStatsContent.AttachToInventory();
                InventoryButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                break;
            case Define.EMainMenuType.Skill:
                SkillContent.transform.SetParent(contentUIParent);
                SkillButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                break;
        }
    }

    public void CloseMainMenu()
    {
        GI.Inst.UIManager.CloseMainMenu();
    }
    
    public void RefreshEquipPassiveSkillUI(List<SO_Skill> skills)
    {
        SkillContent.RefreshEquipPassiveSkillUI(skills);
    }
    
    public void RefreshPassiveSkillUI(List<SO_PassiveSkill> skills)
    {
        SkillContent.RefreshPassiveSkillUI(skills);
    }
}
