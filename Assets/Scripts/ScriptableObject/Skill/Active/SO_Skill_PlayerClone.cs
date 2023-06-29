using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/PlayerCloneSkill")]
public class SO_Skill_PlayerClone : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"가까운 적들의 위치에 분신을 소환하여 피해를 입힙니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        //SkillManager에서 코루틴 사용하여 Clone 스폰
        GI.Inst.ListenerManager.OnExecPlayerClone(playerController.ControlledPlayer.InstId, castStatManager, damageInfo);
    }
}
