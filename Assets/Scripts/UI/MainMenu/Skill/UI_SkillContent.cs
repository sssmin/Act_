using System.Collections.Generic;
using UnityEngine;

public class UI_SkillContent : MonoBehaviour
{
    [SerializeField] private UI_ActiveSkillSlotParent activeSkillSlotParent;
    [SerializeField] private UI_EquipPassiveSkillSlotParent equipPassiveSkillSlotParent;
    [SerializeField] private UI_PassiveSkillSlotParent passiveSkillSlotParent;

    public void InitOnce()
    {
        activeSkillSlotParent.InitOnce();
        equipPassiveSkillSlotParent.InitOnce();
    }

    //패시브를 장착했을 때, 해제했을 때, 패시브 스킬 레벨업 버튼 눌렀을 때
    public void RefreshEquipPassiveSkillUI(List<SO_Skill> skills)
    {
        equipPassiveSkillSlotParent.RefreshEquipPassiveSkillUI(skills);
    }
    
    //맨 처음 초기화시, 패시브 스킬 레벨업 버튼 눌렀을 때
    public void RefreshPassiveSkillUI(List<SO_PassiveSkill> skills)
    {
        passiveSkillSlotParent.RefreshPassiveSkillUI(skills);
    }
}