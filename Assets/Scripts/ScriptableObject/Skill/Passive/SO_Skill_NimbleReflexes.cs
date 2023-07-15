using UnityEngine;

[CreateAssetMenu(fileName = "Skill_NimbleReflexes", menuName ="Data/PassiveSkill/NimbleReflexes")]
public class SO_Skill_NimbleReflexes : SO_PassiveSkill
{
    private DurationEffect durationEffect;
    private float evasionChance;
    
    public override void Init()
    {
        evasionChance = 1.5f + ((skillLevel - 1) * (3.5f / 9));
        evasionChance = Mathf.Round( evasionChance * 10f) / 10f;
        SkillDesc = $"공격 시 확률적으로 7초 동안 회피율이 {evasionChance}% 증가합니다.";
    }
    
    public override void ExecSkill(StatManager casterStatManager, PlayerController playerController)
    {
        EffectInfo effectInfo = new EffectInfo();
        effectInfo.applyPerBySkillLevel = evasionChance;
        
         effectInfo.onExecuteIncreaseStat = () =>
         {
             casterStatManager.characterStats.evasionChancePer.AddModifier(effectInfo.applyPerBySkillLevel);
         };
         
         effectInfo.onExecuteDecreaseStat = () =>
         {
             casterStatManager.characterStats.evasionChancePer.SubModifier(effectInfo.applyPerBySkillLevel);
         };
        durationEffect = new DurationEffect_NimbleReflexes();
        
        float activateChancePer = 7.5f + ((skillLevel - 1) * (7.5f / 9));
        activateChancePer = Mathf.Round( activateChancePer * 10f) / 10f;
        float duraion = 7f;
        durationEffect.Init(
            Define.EActivationCondition.CauseDamage, 
            activateChancePer, 
            Define.EDamageType.Both, 
            effectInfo, 
            duraion,
            EDurationEffectId.NimbleReflexes,
            true);

        effect = durationEffect;
    }
}