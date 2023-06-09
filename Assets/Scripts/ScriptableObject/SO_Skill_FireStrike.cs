using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/FireStrike")]
public class SO_Skill_FireStrike : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject daggerObj = GI.Inst.ResourceManager.Instantiate(EPrefabId.FireStrike, playerController.ControlledPlayer.arrowSpawnPoint.position, playerController.transform.rotation);
        SkillAbility_FireStrike skillAbilityFireStrike = daggerObj.GetComponent<SkillAbility_FireStrike>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityFireStrike.Init(playerController.AttackDir, inOwner: playerController, damageInfo);
    }
}
