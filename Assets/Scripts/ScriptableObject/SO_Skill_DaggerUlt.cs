using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/DaggerUlt")]
public class SO_Skill_DaggerUlt : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        Vector3 spawnPos = Util.GetCenterWorldPos() + Vector3.up * 3f;
        GameObject daggerUltObj = GI.Inst.ResourceManager.Instantiate(EPrefabId.DaggerUlt, spawnPos, quaternion.identity);
        SkillAbility_DaggerUlt skillAbilityDaggerUlt = daggerUltObj.GetComponent<SkillAbility_DaggerUlt>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        List<Transform> monsters = Util.GetMonstersInScreen();
        skillAbilityDaggerUlt.Init(playerController, damageInfo, monsters);
        
    }
}
