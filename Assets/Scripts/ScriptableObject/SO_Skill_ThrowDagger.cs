using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/ThrowDaggerSkill")]
public class SO_Skill_ThrowDagger : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, Transform spawnPoint, PlayerController playerController)
    {
        GameObject daggerObj = GI.Inst.ResourceManager.Instantiate(EPrefabId.Dagger, spawnPoint.position, quaternion.identity);
        SkillAbility_ThrowDagger skillAbilityThrowDagger = daggerObj.GetComponent<SkillAbility_ThrowDagger>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityThrowDagger.Init(playerController.AttackDir, inOwner: playerController, damageInfo);
    }

    
}
