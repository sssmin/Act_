using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatManager : StatManager
{
    public override void Awake()
    {
        base.Awake();
        
        GI.Inst.ListenerManager.getStats -= GetStats;
        GI.Inst.ListenerManager.getStats += GetStats;
        GI.Inst.ListenerManager.onStatAddModifier -= StatAddModifier;
        GI.Inst.ListenerManager.onStatAddModifier += StatAddModifier;
        GI.Inst.ListenerManager.onStatSubModifier -= StatSubModifier;
        GI.Inst.ListenerManager.onStatSubModifier += StatSubModifier;
        
    }

    public override void Update()
    {
        base.Update();
        
        if (DurationEffectEndTimePq.GetCount() > 0)
        {
            foreach (KeyValuePair<EDurationEffectId, float> pair in IconFillAmount.ToList())
            {
                if (DurationEffectDurationDict.ContainsKey(pair.Key))
                {
                    float duration = DurationEffectDurationDict[pair.Key];
                    IconFillAmount[pair.Key] -= (1 / duration * Time.deltaTime);
                }
            }
            DurationEffect durationEffect = DurationEffectEndTimePq.Peek();
            if (durationEffect.durationEndTime <= Time.time)//지속시간 끝
            {
                durationEffect = DurationEffectEndTimePq.Pop();
                
                if (IconFillAmount.ContainsKey(durationEffect.durationEffectId))
                    IconFillAmount.Remove(durationEffect.durationEffectId);
                
                if (DurationEffectDurationDict.ContainsKey(durationEffect.durationEffectId))
                    DurationEffectDurationDict.Remove(durationEffect.durationEffectId);
                
                
                durationEffect.effectInfo.onExecuteDecreaseStat?.Invoke();
                RefreshInventoryUI();
                
            }
        }

        //test
        if (Input.GetKeyDown(KeyCode.T))
        {
            //AddCurrentHp(-20);
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = 30f;
            damageInfo.bIsCritical = false;
            TakeDamage(damageInfo, this, Define.EDamageType.Normal);

        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddCurrentHp(40);
        }
    }

    public override void TakeDamage(DamageInfo damageInfo, StatManager instigatorStatManager, Define.EDamageType damageType)
    {
        base.TakeDamage(damageInfo, instigatorStatManager, damageType);
        
        if (IsDead) return;
        GI.Inst.ListenerManager.ExecTakeDamageEffect(damageType, instigatorStatManager.GetComponent<StatManager>());
    }

    public override void AddCurrentHp(float value)
    {
        base.AddCurrentHp(value);
        
        float ratio = Mathf.Round((stats.currentHp.Value / stats.maxHp.Value * 100f) * 10) * 0.1f; 
        GI.Inst.UIManager.SetHpBar(ratio);
    }
    
}
