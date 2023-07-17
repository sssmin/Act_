using UnityEngine;

[CreateAssetMenu(fileName = "SteelWeapon_", menuName ="Data/Item/SteelWeapon")]
public class SteelWeapon : BaseWeapon
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
            EffectInfo effectInfo = new EffectInfo();
            desc = $"기본 공격 대미지 {EnhanceLevel}% 증가";
            effectDescs.Add(desc);
            
            effectInfo.onExecuteIncreaseStat = 
                () => ownerStatManager.characterStats.normalAttackDamageIncPer.AddModifier(EnhanceLevel);
            effectInfo.onExecuteDecreaseStat = 
                () => ownerStatManager.characterStats.normalAttackDamageIncPer.SubModifier(EnhanceLevel);
            
            Effect durationEffect = new Effect();
            durationEffect.Init(Define.EActivationCondition.None, -1f, 
                Define.EDamageType.Normal, effectInfo);
        
            effects.Add(durationEffect);
        }
        
        
        
    }
}
