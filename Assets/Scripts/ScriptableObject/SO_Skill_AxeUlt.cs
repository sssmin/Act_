using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/AxeUlt")]
public class SO_Skill_AxeUlt : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        Vector3 spawnPos = Util.GetCenterWorldPos() + Vector3.up * 3f;
        GameObject daggerUltObj = GI.Inst.ResourceManager.Instantiate(EPrefabId.AxeUlt, spawnPos, quaternion.identity);
        SkillAbility_AxeUlt skillAbilityAxeUlt = daggerUltObj.GetComponent<SkillAbility_AxeUlt>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        List<Transform> monsters = Util.GetMonstersInScreen();
        skillAbilityAxeUlt.Init(playerController, damageInfo, monsters);
    }
}
