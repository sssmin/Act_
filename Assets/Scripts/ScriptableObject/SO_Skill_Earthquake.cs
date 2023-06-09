using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/Earthquake")]
public class SO_Skill_Earthquake : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        //SkillManager에서 코루틴 사용하여 earthquake 스폰
        GI.Inst.ListenerManager.OnExecEarthquake(playerController.ControlledPlayer.InstId, castStatManager, damageInfo);
    }
}