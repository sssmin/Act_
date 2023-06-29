using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/DaggerUlt")]
public class SO_Skill_DaggerUlt : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"보이는 모든 적들에게 피해를 입힙니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        Vector3 spawnPos = Util.GetCenterWorldPos() + Vector3.up * 3f;

        GameObject go =
            GI.Inst.ResourceManager.Instantiate("DaggerUlt", spawnPos, quaternion.identity);
        SkillAbility_DaggerUlt skillAbilityDaggerUlt = go.GetComponent<SkillAbility_DaggerUlt>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        List<Transform> monsters = Util.GetMonstersInScreen();
        skillAbilityDaggerUlt.Init(playerController, castStatManager, damageInfo, monsters);


    }
}
