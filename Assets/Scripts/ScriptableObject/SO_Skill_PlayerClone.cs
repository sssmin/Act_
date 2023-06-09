using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/PlayerCloneSkill")]
public class SO_Skill_PlayerClone : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        //SkillManager에서 코루틴 사용하여 Clone 스폰
        GI.Inst.ListenerManager.OnExecPlayerClone(playerController.ControlledPlayer.InstId, castStatManager, damageInfo);
    }
}
