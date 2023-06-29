using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/AxeUlt")]
public class SO_Skill_AxeUlt : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"도끼를 휘둘러 피해를 입힙니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        Vector3 spawnPos = Util.GetCenterWorldPos() + Vector3.up * 3f;

        GameObject go =
            GI.Inst.ResourceManager.Instantiate("AxeUlt", spawnPos, quaternion.identity);
        SkillAbility_AxeUlt skillAbilityAxeUlt = go.GetComponent<SkillAbility_AxeUlt>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        List<Transform> monsters = Util.GetMonstersInScreen();
        skillAbilityAxeUlt.Init(playerController, castStatManager, damageInfo, monsters);

    }
}
