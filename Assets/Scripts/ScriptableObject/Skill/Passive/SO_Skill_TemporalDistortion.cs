using UnityEngine;

[CreateAssetMenu(fileName = "Skill_TemporalDistortion", menuName ="Data/PassiveSkill/TemporalDistortion")]
public class SO_Skill_TemporalDistortion : SO_PassiveSkill
{
    private float activateChancePer;
    
    public override void Init()
    {
        activateChancePer = 10f + ((skillLevel - 1) * (10f / 9));
        activateChancePer = Mathf.Round(activateChancePer * 10f) / 10f;
        SkillDesc = $"공격 시 {activateChancePer}% 확률로 액티브 스킬 1개의 쿨타임이 랜덤으로 초기화됩니다.";
    }
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        effect = new Effect_TemporalDistortion();
        
        effect.Init(Define.EActivationCondition.CauseDamage, activateChancePer, Define.EDamageType.Both, null);
        effect.skillCooltime = skillCooltime;
    }
}
