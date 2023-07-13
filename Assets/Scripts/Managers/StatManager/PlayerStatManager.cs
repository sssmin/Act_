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
        if (IsDead) return;
        float damage = damageInfo.damage;

        //회피
        float rand = Random.Range(0f, 100f);
        if (stats.evasionChancePer.Value > rand)
        {
            SpawnDamageText(Define.EDamageTextType.Evasion);
            return;
        }
        if (damageInfo.bIsCritical)
        {
            rand = Random.Range(0f, 100f);
            //크리 저항
            if (stats.criticalResistPer.Value > rand)
            {
                //크리 적용 전 대미지
                //Result = baseDamage + baseDamage * 1.5  => Result = baseDamage*(1 + 1.5) => baseDamage = Result/(1 + 1.5)
                damage = Mathf.Round((damage / 2.5f) * 10) * 0.1f;
            }
        }

        float defence = stats.defence.Value;
        damage = Mathf.Clamp((damage * Random.Range(0.9f, 1.1f)) * (1 - (defence / (100 + defence))), 0f, float.MaxValue);
        damage = Mathf.Round((damage) * 10) * 0.1f;
        //Debug.Log($"{gameObject.name} 최종 받는 대미지 {damageInfo.damage}");
        //Hit Effect 적용
        Character.HitEffect();
        AddCurrentHp(-damage);
        
        
        if (damageInfo.bIsCritical)
        {
            SpawnDamageText(Define.EDamageTextType.PlayerDamagedCritical, damage);
        }
        else
        {
            SpawnDamageText(Define.EDamageTextType.PlayerDamaged, damage);
        }
        
        if (stats.currentHp.Value <= 0f)
        {
            Dead();
            return;
        }
        
        GI.Inst.ListenerManager.ExecTakeDamageEffect(damageType, instigatorStatManager.GetComponent<StatManager>());
        
    }
    
    public override void CauseNormalAttack(StatManager enemyStatManager, float normalAttackCoef = 1f)
    {
        GI.Inst.ListenerManager.PlayersNormalAttack();
        
        base.CauseNormalAttack(enemyStatManager, normalAttackCoef);
    }

    public override void AddCurrentHp(float value)
    {
        base.AddCurrentHp(value);
        
        float ratio = Mathf.Round((stats.currentHp.Value / stats.maxHp.Value * 100f) * 10) * 0.1f; 
        GI.Inst.UIManager.SetHpBar(ratio);
    }

    public void InitCurrentHp(float currentHp)
    {
        stats.currentHp.Value = currentHp;
    }
    
    
}
