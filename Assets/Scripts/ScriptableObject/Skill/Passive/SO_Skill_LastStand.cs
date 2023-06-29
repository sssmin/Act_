using UnityEngine;

[CreateAssetMenu(fileName = "Skill_LastStand", menuName ="Data/PassiveSkill/LastStand")]
public class SO_Skill_LastStand : SO_PassiveSkill
{
    private DurationEffect durationEffect;
    private float atkPer;
    private float defPer;
    
    public override void Init()
    {
        atkPer = 1f + ((skillLevel - 1) * (2f / 9));
        atkPer = Mathf.Round(atkPer * 10f) / 10f;
        
        defPer = 2.5f + ((skillLevel - 1) * (2.5f / 9));
        defPer = Mathf.Round(defPer * 10f) / 10f;
        SkillDesc = $"체력이 20% 이하가 되면 10초 동안 공격력 {atkPer}%, 방어력 {defPer}%가 증가합니다.";
    }
    
    public override void ExecSkill(StatManager casterStatManager, PlayerController playerController)
    {
        EffectInfo effectInfo = new EffectInfo();
        
        effectInfo.onExecuteIncreaseStat = () =>
        {
            casterStatManager.stats.attack.AddModifier(atkPer);
            casterStatManager.stats.defence.AddModifier(defPer);
        };
         
        effectInfo.onExecuteDecreaseStat = () =>
        {
            casterStatManager.stats.attack.SubModifier(atkPer);
            casterStatManager.stats.defence.SubModifier(defPer);
        };
        durationEffect = new DurationEffect_LastStand();
        
        float duration = 10f;
        
        durationEffect.Init(
            Define.EActivationCondition.TakeDamage, 
            -1f, 
            Define.EDamageType.Both, 
            effectInfo, 
            duration,
            EDurationEffectId.LastStand,
            true);
        durationEffect.skillCooltime = skillCooltime;
        effect = durationEffect;
    }
}