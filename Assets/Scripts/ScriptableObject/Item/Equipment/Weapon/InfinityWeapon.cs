using UnityEngine;


[CreateAssetMenu(fileName = "Infinity", menuName ="Data/Item/InfinityWeapon")]
public class InfinityWeapon : BaseWeapon
{
    public override void Init(StatManager ownerStatManager)
    {
        effectDescs.Clear();
        effects.Clear();
        
        string desc;

        if (EnhanceLevel <= 0)
        {
            desc = $"Y부가 효과 개방 - +1 강화";
            effectDescs.Add(desc);
            return;
        }
        
        if (EnhanceLevel > 0)
        {
            desc = $"크리티컬 확률 {EnhanceLevel}% 증가";
            effectDescs.Add(desc);
            
            EffectInfo effectInfo = new EffectInfo();
            effectInfo.onExecuteIncreaseStat = 
                () => ownerStatManager.characterStats.criticalChancePer.AddModifier(EnhanceLevel);
            effectInfo.onExecuteDecreaseStat = 
                () => ownerStatManager.characterStats.criticalChancePer.SubModifier(EnhanceLevel);
            
            Effect effect = new Effect();
            effect.Init(Define.EActivationCondition.None, -1f, 
                Define.EDamageType.Normal, effectInfo);
            effects.Add(effect);
            
            if (EnhanceLevel < 5)
            {
                desc = "Y부가 효과 추가 개방 - +5 강화";
                effectDescs.Add(desc);
            }
        }

        if (EnhanceLevel >= 5)
        {
            desc = $"7% 확률로 잃은 체력의 {5f + EnhanceLevel}% 회복";
            effectDescs.Add(desc);
            
            EffectInfo effectInfo = new EffectInfo();
            effectInfo.applyPerBySkillLevel = 5f + EnhanceLevel;

            Effect_HealthSteal effect = new Effect_HealthSteal();
            effect.Init(Define.EActivationCondition.CauseDamage, 7f, 
                Define.EDamageType.Both, effectInfo);
            effects.Add(effect);
            
            if (EnhanceLevel < 10)
            {
                desc = "Y부가 효과 추가 개방 - +10 강화";
                effectDescs.Add(desc);
            }
        }
        
        if (EnhanceLevel >= 10)
        {
            desc = "크리티컬 피해량 15% 증가";
            effectDescs.Add(desc);
            
            EffectInfo effectInfo = new EffectInfo();
            
            effectInfo.onExecuteIncreaseStat = 
                () => ownerStatManager.characterStats.criticalDamageIncPer.AddModifier(15f);
            effectInfo.onExecuteDecreaseStat = 
                () => ownerStatManager.characterStats.criticalDamageIncPer.SubModifier(15f);
            
            Effect effect = new Effect();
            effect.Init(Define.EActivationCondition.None, -1f, 
                Define.EDamageType.Normal, effectInfo);
            effects.Add(effect);
        }
    }
}
