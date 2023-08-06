using System.Collections.Generic;
using UnityEngine;

public class UI_EquipPassiveSkillSlotParent : MonoBehaviour
{
    private List<UI_Skill_EquipPassiveSkillSlot> equipPassiveSkillSlots = new List<UI_Skill_EquipPassiveSkillSlot>();
    

    public void InitOnce()
    {
        GI.Inst.UIManager.checkEquippedPassive -= CheckEquippedPassive;   
        GI.Inst.UIManager.checkEquippedPassive += CheckEquippedPassive;   
        GI.Inst.UIManager.blinkEquipPassiveSkillSlot -= Blink;   
        GI.Inst.UIManager.blinkEquipPassiveSkillSlot += Blink;   
        GI.Inst.UIManager.setEquipPassive -= SetEquipPassive;   
        GI.Inst.UIManager.setEquipPassive += SetEquipPassive;   
        
        
        for (int i = 0; i < 3; i++)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Skill_EquipPassiveSkillSlot", transform);
            UI_Skill_EquipPassiveSkillSlot equipPassiveSkillSlot = go.GetComponent<UI_Skill_EquipPassiveSkillSlot>();
            equipPassiveSkillSlot.index = i;
            equipPassiveSkillSlot.InitOnce();
            equipPassiveSkillSlots.Add(equipPassiveSkillSlot);
        }
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.checkEquippedPassive -= CheckEquippedPassive;    
        GI.Inst.UIManager.blinkEquipPassiveSkillSlot -= Blink;   
        GI.Inst.UIManager.setEquipPassive -= SetEquipPassive;   
    }

    //불러오기 로드시 한번 호출
    private void SetEquipPassive(SO_PassiveSkill passiveSkill)
    {
        PassiveSkill_Lite skillShortVer = new PassiveSkill_Lite();
        skillShortVer.DataCopy(passiveSkill);
        foreach (UI_Skill_EquipPassiveSkillSlot equipPassiveSkillSlot in equipPassiveSkillSlots)
        {
            if (equipPassiveSkillSlot.index == passiveSkill.equipIndex)
            {
                equipPassiveSkillSlot.Init(skillShortVer);
            }
        }
    }

    private void CheckEquippedPassive(PassiveSkill_Lite passiveSkill)
    {
        foreach (UI_Skill_EquipPassiveSkillSlot skillSlot in equipPassiveSkillSlots)
        {
            skillSlot.ClearIfSame(passiveSkill);
        }
    }

    private void Blink(bool cond)
    {
        foreach (UI_Skill_EquipPassiveSkillSlot skillSlot in equipPassiveSkillSlots)
        {
            skillSlot.Blink(cond);
        }
    }
}
