using System.Collections.Generic;
using UnityEngine;


public class UI_PassiveSkillSlotParent : MonoBehaviour
{
    private List<UI_Skill_PassiveSkillSlot> passiveSkillSlots = new List<UI_Skill_PassiveSkillSlot>();
    
    private bool InitCompleted { get; set; }
    
    
    public void RefreshPassiveSkillUI(List<SO_PassiveSkill> skills)
    {
        if (!InitCompleted)
        {
            InitCompleted = true;
            foreach (SO_PassiveSkill skill in skills)
            {
                GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Skill_PassiveSkillSlot", transform);
                UI_Skill_PassiveSkillSlot passiveSkillSlot = go.GetComponent<UI_Skill_PassiveSkillSlot>();
                PassiveSkill_ShortVer skillShortVer = new PassiveSkill_ShortVer();
                skillShortVer.DataCopy(skill);
                passiveSkillSlot.InitOnce();
                passiveSkillSlot.Refresh(skillShortVer);
                passiveSkillSlots.Add(passiveSkillSlot);
            }
        }
        else
        {
            for (int i = 0; i < passiveSkillSlots.Count; i++)
            {
                PassiveSkill_ShortVer skillShortVer = new PassiveSkill_ShortVer();
                skillShortVer.DataCopy(skills[i]);
                passiveSkillSlots[i].Refresh(skillShortVer);
            }
        }
    }
}
