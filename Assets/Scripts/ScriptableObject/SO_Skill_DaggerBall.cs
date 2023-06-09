using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/DaggerBall")]
public class SO_Skill_DaggerBall : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject daggerBallObj = GI.Inst.ResourceManager.Instantiate(EPrefabId.DaggerBall, playerController.ControlledPlayer.arrowSpawnPoint.position, playerController.transform.rotation);
        SkillAbility_DaggerBall skillAbilityDaggerBall = daggerBallObj.GetComponent<SkillAbility_DaggerBall>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityDaggerBall.Init(playerController.AttackDir, inOwner: playerController, damageInfo);
    }
}
