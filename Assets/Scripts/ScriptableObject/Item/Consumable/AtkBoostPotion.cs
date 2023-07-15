using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable_", menuName ="Data/Item/Consumable/AtkBoostPotion")]
public class AtkBoostPotion : Consumable
{
    [HideInInspector] public DurationEffect effect;
    
    public override void UseItem(StatManager casterStatManager)
    {
        effect = new DurationEffect();
        EffectInfo effectInfo = new EffectInfo();
        
        float atkIncValue = casterStatManager.characterStats.attack.Value * 0.1f;
        Debug.Log(atkIncValue);
        
        effectInfo.onExecuteIncreaseStat = 
            () => casterStatManager.characterStats.attackIncValue.AddModifier(atkIncValue);
        
        effectInfo.onExecuteDecreaseStat = 
            () => casterStatManager.characterStats.attackIncValue.SubModifier(atkIncValue);
        
        float duration = 10f;
        
        effect.Init(
            Define.EActivationCondition.None, 
            -1f, 
            Define.EDamageType.None, 
            effectInfo, 
            duration,
            EDurationEffectId.AtkBoostPotion, 
            false);
        
        effect.durationEndTime = duration + Time.time;
        casterStatManager.ExecDurationEffect(effect, itemIcon);
    }
}
