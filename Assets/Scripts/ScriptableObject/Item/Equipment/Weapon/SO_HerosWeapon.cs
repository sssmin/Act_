using UnityEngine;

[CreateAssetMenu(fileName = "Heros", menuName ="Data/Item/HerosWeapon")]
public class HerosWeapon : SO_BaseWeapon
{
    public override void Init(StatManager ownerStatManager)
    {
        effectDescs.Clear();
        effects.Clear();
        
        string desc;
        
        if (EnhanceLevel <= 0)
        {
            desc = $"부가 효과 개방 - +1 강화";
            effectDescs.Add(desc);
            return;
        }
        
        if (EnhanceLevel > 0)
        {
            desc = $"스킬 쿨타임 {EnhanceLevel}% 감소";
            effectDescs.Add(desc);
            
            EffectInfo effectInfo = new EffectInfo();
            effectInfo.onExecuteIncreaseStat = 
                () => ownerStatManager.characterStats.skillCooltimeDecRate.AddModifier(EnhanceLevel);
            effectInfo.onExecuteDecreaseStat = 
                () => ownerStatManager.characterStats.skillCooltimeDecRate.SubModifier(EnhanceLevel);
            
            Effect effect = new Effect();
            effect.Init(Define.EActivationCondition.None, -1f, 
                Define.EDamageType.Normal, effectInfo);
            effects.Add(effect);
            
            if (EnhanceLevel < 7)
            {
                desc = "부가 효과 추가 개방 - +7 강화";
                effectDescs.Add(desc);
            }
        }
        
        if (EnhanceLevel >= 7)
        {
            desc = "크리티컬 피해량 5% 증가";
            effectDescs.Add(desc);
            
            EffectInfo effectInfo = new EffectInfo();
            
            effectInfo.onExecuteIncreaseStat = 
                () => ownerStatManager.characterStats.criticalDamageIncPer.AddModifier(5f);
            effectInfo.onExecuteDecreaseStat = 
                () => ownerStatManager.characterStats.criticalDamageIncPer.SubModifier(5f);
            
            Effect effect = new Effect();
            effect.Init(Define.EActivationCondition.None, -1f, 
                Define.EDamageType.Normal, effectInfo);
            effects.Add(effect);
        }
    }
}
