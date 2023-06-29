using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/Earthquake")]
public class SO_Skill_Earthquake : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"대지에 균열을 일으켜 피해를 입힙니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        //SkillManager에서 코루틴 사용하여 earthquake 스폰
        GI.Inst.ListenerManager.OnExecEarthquake(playerController.ControlledPlayer.InstId, castStatManager, damageInfo);
    }
}