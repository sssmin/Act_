using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/DistortionArrow")]
public class SO_Skill_DistortionArrow : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"중력장을 소환하여 제압하고 피해를 입힙니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject go =
            GI.Inst.ResourceManager.Instantiate("DistortionArrow", playerController.ControlledPlayer.arrowSpawnPoint.position, quaternion.identity);
        
        SkillAbility_DistortionArrow skillAbilityDistortionArrow = go.GetComponent<SkillAbility_DistortionArrow>();

        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);

        int addPer = Mathf.RoundToInt((Mathf.RoundToInt(playerController.CurrentChargeAmount / 5f + 1) * 5f));
        damageInfo.damage += Mathf.RoundToInt(damageInfo.damage * addPer / 100f);

        skillAbilityDistortionArrow.Init(playerController.AttackDir, inOwner: castStatManager, damageInfo);

    }
}
