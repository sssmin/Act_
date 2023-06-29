using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/Dash")]
public class SO_Skill_Dash : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"보는 방향으로 대쉬합니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {

    }
}
