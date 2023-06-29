using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/ThrowAxe")]
public class SO_Skill_ThrowAxe : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"도끼를 던져 피해를 입히고, 스핀을 하며 피해를 입힙니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject go =
            GI.Inst.ResourceManager.Instantiate("Axe", playerController.ControlledPlayer.arrowSpawnPoint.position, playerController.transform.rotation);
        SkillAbility_ThrowAxe skillAbilityThrowDagger = go.GetComponent<SkillAbility_ThrowAxe>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityThrowDagger.Init(playerController.AttackDir, inOwner: playerController, castStatManager, damageInfo);

    }
}
