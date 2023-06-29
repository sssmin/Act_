using UnityEngine;

[CreateAssetMenu(fileName = "Skill_DeadlyImpact", menuName ="Data/PassiveSkill/DeadlyImpact")]
public class SO_Skill_DeadlyImpact : SO_PassiveSkill
{
    private DurationEffect durationEffect;
    private float criticalChance;
    
    public override void Init()
    {
        criticalChance = 5f + ((skillLevel - 1) * (5f / 9));
        criticalChance = Mathf.Round(criticalChance * 10f) / 10f;
        SkillDesc = $"기본 공격 시 5초 동안 크리티컬 확률이 {criticalChance}% 증가합니다.";
    }
    
    public override void ExecSkill(StatManager casterStatManager, PlayerController playerController)
    {
        EffectInfo effectInfo = new EffectInfo();
        effectInfo.applyPerBySkillLevel = criticalChance;
        
        effectInfo.onExecuteIncreaseStat = () =>
        {
            casterStatManager.stats.criticalChancePer.AddModifier(effectInfo.applyPerBySkillLevel);
        };
         
        effectInfo.onExecuteDecreaseStat = () =>
        {
            casterStatManager.stats.criticalChancePer.SubModifier(effectInfo.applyPerBySkillLevel);
        };
        durationEffect = new DurationEffect_DeadlyImpact();
        
        float duration = 5f;
        
        durationEffect.Init(
            Define.EActivationCondition.CauseDamage, 
            -1f, 
            Define.EDamageType.Normal, 
            effectInfo, 
            duration,
            EDurationEffectId.DeadlyImpact,
            true);
        durationEffect.skillCooltime = skillCooltime;
        effect = durationEffect;
    }
}