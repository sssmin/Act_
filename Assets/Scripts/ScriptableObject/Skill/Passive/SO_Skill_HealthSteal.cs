using UnityEngine;

[CreateAssetMenu(fileName = "Skill_HealthSteal", menuName ="Data/PassiveSkill/HealthSteal")]
public class SO_Skill_HealthSteal : SO_PassiveSkill
{
    private float rate;
    
    public override void Init()
    {
        rate = 5f + ((skillLevel - 1) * (5f / 9));
        rate = Mathf.Round(rate * 10f) / 10f;
        SkillDesc = $"공격 시에 확률적으로 잃은 체력의 {rate}%를 회복합니다.";
    }
    
    
    public override void ExecSkill(StatManager castStatManager, PlayerController playerController)
    {
        EffectInfo effectInfo = new EffectInfo();
        effectInfo.applyPerBySkillLevel = rate;
        
        effect = new Effect_HealthSteal();
        
        float activateChancePer = 5f + ((skillLevel - 1) * (5f / 9));
        activateChancePer = Mathf.Round(activateChancePer * 10f) / 10f;
        
        effect.Init(Define.EActivationCondition.CauseDamage, activateChancePer, Define.EDamageType.Both, effectInfo);
    }
}