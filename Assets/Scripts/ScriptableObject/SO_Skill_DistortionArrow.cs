using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/DistortionArrow")]
public class SO_Skill_DistortionArrow : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject distortionArrow = GI.Inst.ResourceManager.Instantiate(EPrefabId.DistortionArrow,
            playerController.ControlledPlayer.arrowSpawnPoint.position, quaternion.identity);
        SkillAbility_DistortionArrow skillAbilityDistortionArrow = distortionArrow.GetComponent<SkillAbility_DistortionArrow>();

        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);

        int addPer = Mathf.RoundToInt((Mathf.RoundToInt(playerController.CurrentChargeAmount / 5f + 1) * 5f));
        damageInfo.damage += Mathf.RoundToInt(damageInfo.damage * addPer / 100f);

        skillAbilityDistortionArrow.Init(playerController.AttackDir, inOwner: playerController, damageInfo);
    }
}
