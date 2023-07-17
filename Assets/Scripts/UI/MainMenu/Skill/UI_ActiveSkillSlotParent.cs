using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_ActiveSkillSlotParent : MonoBehaviour
{
    private List<UI_Skill_ActiveSkillSlot> activeSkillSlots = new List<UI_Skill_ActiveSkillSlot>();

    public void InitOnce()
    {
        GI.Inst.UIManager.refreshActiveSkillSlots -= RefreshActiveSkillSlots;
        GI.Inst.UIManager.refreshActiveSkillSlots += RefreshActiveSkillSlots;
        GI.Inst.UIManager.clearActiveSkillSlots -= Clear;
        GI.Inst.UIManager.clearActiveSkillSlots += Clear;
        
        for (int i = 0; i < (int)EActiveSkillOrder.Max; i++)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Skill_ActiveSkillSlot", transform);
            
            UI_Skill_ActiveSkillSlot skillActiveSkillSlot = go.GetComponent<UI_Skill_ActiveSkillSlot>();
          
            skillActiveSkillSlot.InitOnce();
            activeSkillSlots.Add(skillActiveSkillSlot);
        }
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.refreshActiveSkillSlots -= RefreshActiveSkillSlots;
        GI.Inst.UIManager.clearActiveSkillSlots -= Clear;
    }

    private void RefreshActiveSkillSlots(List<SO_ActiveSkill> skills)
    {
        int index = 0;
        for (int i = 0; i < skills.Count; i++)
        {
            UI_Skill_ActiveSkillSlot skillSlot = activeSkillSlots[i];
            ActiveSkill_ShortVer skillShortVer = new ActiveSkill_ShortVer();
            skillShortVer.DataCopy(skills[i], GI.Inst.PlayerSkillManager.GetActiveSkillLevel(skills[i].activeSkillOrder));
            skillSlot.Refresh(skillShortVer, index++);
        }
    }

    public void Clear()
    {
        foreach (UI_Skill_ActiveSkillSlot skillSlot in activeSkillSlots)
        {
            skillSlot.Clear();
        }
    }
}
