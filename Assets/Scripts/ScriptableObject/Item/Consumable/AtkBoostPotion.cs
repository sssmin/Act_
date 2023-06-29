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
        
        effectInfo.onExecuteIncreaseStat = 
            () => casterStatManager.stats.attack.AddModifier(15);
        
        effectInfo.onExecuteDecreaseStat = 
            () => casterStatManager.stats.attack.SubModifier(15);
        
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
