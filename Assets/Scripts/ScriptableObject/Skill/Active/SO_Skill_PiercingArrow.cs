using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/PiercingArrow")]
public class SO_Skill_PiercingArrow : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"적들을 관통하는 화살을 발사하여 피해를 입힙니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject go =
            GI.Inst.ResourceManager.Instantiate("PiercingArrow", playerController.ControlledPlayer.arrowSpawnPoint.position, quaternion.identity);
        SkillAbility_PiercingArrow skillAbilityPiercingArrow = go.GetComponent<SkillAbility_PiercingArrow>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);

        int addPer = Mathf.RoundToInt((Mathf.RoundToInt(playerController.CurrentChargeAmount / 5f + 1) * 5f));
        damageInfo.damage += Mathf.RoundToInt(damageInfo.damage * addPer / 100f);
        
        skillAbilityPiercingArrow.Init(playerController.AttackDir, inOwner: castStatManager, damageInfo);

    }
    
    
    
}