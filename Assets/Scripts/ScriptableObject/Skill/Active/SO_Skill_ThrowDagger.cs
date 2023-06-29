using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/ThrowDagger")]
public class SO_Skill_ThrowDagger : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);

        SkillDesc = $"대거를 던져 피해를 입힙니다.";
    }

    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject go =
            GI.Inst.ResourceManager.Instantiate("Dagger", playerController.ControlledPlayer.arrowSpawnPoint.position, playerController.transform.rotation);
        SkillAbility_ThrowDagger skillAbilityThrowDagger = go.GetComponent<SkillAbility_ThrowDagger>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityThrowDagger.Init(playerController.AttackDir, inOwner: castStatManager, damageInfo);

    }
}
