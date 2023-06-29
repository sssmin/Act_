using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/ArrowRain")]
public class SO_Skill_ArrowRain : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"화살 비를 쏟아 피해를 입힙니다.";
    }


    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        Vector3 spawnPos = GetSpawnPos(playerController);
        if (spawnPos == new Vector3(10000, 10000)) return;

        GameObject go =
            GI.Inst.ResourceManager.Instantiate("ArrowRain", spawnPos, quaternion.identity);
        SkillAbility_ArrowRain skillAbilityArrowRain = go.GetComponent<SkillAbility_ArrowRain>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityArrowRain.Init(playerController.AttackDir, castStatManager, damageInfo);

    }

    public Vector3 GetSpawnPos(PlayerController playerController)
    {
        //앞으로 8 아래로 ray
        RaycastHit2D hitResult =
            Physics2D.Raycast(playerController.transform.position + new Vector3(playerController.CurrentDir.x * 8f, 0f, 0f), Vector2.down, 1f, LayerMask.GetMask("Ground"));
        if (hitResult)
        {
            return hitResult.point;
        }
        return new Vector3(10000, 10000, 10000);
    }
}