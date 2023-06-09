using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/ThrowAxe")]
public class SO_Skill_ThrowAxe : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject axeObj = GI.Inst.ResourceManager.Instantiate(EPrefabId.Axe, playerController.ControlledPlayer.arrowSpawnPoint.position, playerController.transform.rotation);
        SkillAbility_ThrowAxe skillAbilityThrowDagger = axeObj.GetComponent<SkillAbility_ThrowAxe>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityThrowDagger.Init(playerController.AttackDir, inOwner: playerController, damageInfo);
    }
}
