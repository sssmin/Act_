using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heros", menuName ="Data/Item/HerosWeapon")]
public class HerosWeapon : BaseWeapon
{
    public override void Init(StatManager ownerStatManager)
    {
        //todo Steel 이펙트 임시 복붙해놓은 것.
        EffectInfo effectInfo = new EffectInfo();
        
        effectInfo.onExecuteIncreaseStat = 
            () => ownerStatManager.characterStats.normalAttackDamageIncPer.AddModifier(EnhancementLevel);
        effectInfo.onExecuteDecreaseStat = 
            () => ownerStatManager.characterStats.normalAttackDamageIncPer.SubModifier(EnhancementLevel);
        
        
        Effect durationEffect = new Effect();
        durationEffect.Init(Define.EActivationCondition.None, -1f, 
            Define.EDamageType.Normal, effectInfo);
        
        effects.Add(durationEffect);
        effectDescs.Clear();
        string desc = $"기본 공격 대미지 {EnhancementLevel}% 증가";
        effectDescs.Add(desc);
    }
}
