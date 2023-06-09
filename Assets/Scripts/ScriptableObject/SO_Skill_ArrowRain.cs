using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/ArrowRain")]
public class SO_Skill_ArrowRain : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        Vector3 spawnPos = GetSpawnPos(playerController);
        if (spawnPos == new Vector3(10000, 10000)) return;
        GameObject arrowRain = GI.Inst.ResourceManager.Instantiate(EPrefabId.ArrowRain, GetSpawnPos(playerController), quaternion.identity);
        SkillAbility_ArrowRain skillAbilityArrowRain = arrowRain.GetComponent<SkillAbility_ArrowRain>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityArrowRain.Init(playerController.AttackDir, inOwner: playerController, damageInfo);
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