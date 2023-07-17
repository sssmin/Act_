using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/FireStrike")]
public class SO_Skill_FireStrike : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"화염을 발사하여 피해를 입힙니다. 적을 관통할 때마다 대미지는 줄어듭니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject go =
            GI.Inst.ResourceManager.Instantiate("FireStrike",
                playerController.ControlledPlayer.arrowSpawnPoint.position, playerController.transform.rotation);
        
        SkillAbility_FireStrike skillAbilityFireStrike = go.GetComponent<SkillAbility_FireStrike>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityFireStrike.Init(playerController.AttackDir, castStatManager, damageInfo);

    }
}
