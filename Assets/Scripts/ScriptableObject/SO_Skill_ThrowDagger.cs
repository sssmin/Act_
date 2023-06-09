using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/Skill/ThrowDagger")]
public class SO_Skill_ThrowDagger : ActiveSkill
{
    public override void ExecuteSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject daggerObj = GI.Inst.ResourceManager.Instantiate(EPrefabId.Dagger, playerController.ControlledPlayer.arrowSpawnPoint.position, playerController.transform.rotation);
        SkillAbility_ThrowDagger skillAbilityThrowDagger = daggerObj.GetComponent<SkillAbility_ThrowDagger>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityThrowDagger.Init(playerController.AttackDir, inOwner: playerController, damageInfo);
    }

    
}
