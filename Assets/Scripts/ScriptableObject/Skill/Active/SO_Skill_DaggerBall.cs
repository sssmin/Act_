using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName ="Data/ActiveSkill/DaggerBall")]
public class SO_Skill_DaggerBall : SO_ActiveSkill
{
    public override void EquipInit(int inSkillLevel, StatManager casterStatManager)
    {
        base.EquipInit(inSkillLevel, casterStatManager);
        
        SkillDesc = $"대거를 휘둘러 구체를 발사하여 피해를 입힙니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        GameObject go =
            GI.Inst.ResourceManager.Instantiate("DaggerBall",
                playerController.ControlledPlayer.arrowSpawnPoint.position, playerController.transform.rotation);
        SkillAbility_DaggerBall skillAbilityDaggerBall = go.GetComponent<SkillAbility_DaggerBall>();
       
        DamageInfo damageInfo = castStatManager.GetDefaultDamage();
        damageInfo.damage = GetSkillTotalDamage(damageInfo.damage);
        skillAbilityDaggerBall.Init(playerController.AttackDir, inOwner: castStatManager, damageInfo);

    }
}
