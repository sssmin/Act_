using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/PiercingArrow")]
public class SO_Skill_PiercingArrow : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject arrowRain = GI.Inst.ResourceManager.Instantiate(EPrefabId.PiercingArrow, playerController.ControlledPlayer.arrowSpawnPoint.position, quaternion.identity);
        SkillAbility_PiercingArrow skillAbilityPiercingArrow = arrowRain.GetComponent<SkillAbility_PiercingArrow>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);

        int addPer = Mathf.RoundToInt((Mathf.RoundToInt(playerController.CurrentChargeAmount / 5f + 1) * 5f));
        damageInfo.damage += Mathf.RoundToInt(damageInfo.damage * addPer / 100f);
        
        skillAbilityPiercingArrow.Init(playerController.AttackDir, inOwner: playerController, damageInfo);
    }
    
    
    
}