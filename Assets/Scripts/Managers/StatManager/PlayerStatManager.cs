using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerStatManager : StatManager
{
    private Dictionary<EDurationEffectId, float> IconFillAmount { get; } = new Dictionary<EDurationEffectId, float>();
    
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

    private void OnDestroy()
    {
        GI.Inst.ListenerManager.getStats -= GetStats;
        GI.Inst.ListenerManager.onStatAddModifier -= StatAddModifier;
        GI.Inst.ListenerManager.onStatSubModifier -= StatSubModifier;
    }

    public override void Update()
    {
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
        if (characterStats.evasionChancePer.Value > rand)
        {
            SpawnDamageText(Define.EDamageTextType.Evasion);
            return;
        }
        if (damageInfo.bIsCritical)
        {
            rand = Random.Range(0f, 100f);
            //크리 저항
            if (characterStats.criticalResistPer.Value > rand)
            {
                //크리 적용 전 대미지
                //Result = baseDamage + baseDamage * 1.5  => Result = baseDamage*(1 + 1.5) => baseDamage = Result/(1 + 1.5)
                damage = Mathf.Round((damage / 2.5f) * 10) * 0.1f;
            }
        }

        float defence = characterStats.defence.Value  + characterStats.defenceIncValue.Value;
        damage = Mathf.Clamp((damage * Random.Range(0.9f, 1.1f)) * (1 - (defence / (100 + defence))), 0f, float.MaxValue);
        damage = Mathf.Round((damage) * 10) * 0.1f;
        //Debug.Log($"{gameObject.name} 최종 받는 대미지 {damageInfo.damage}");
        //Hit Effect 적용
        Character.HitEffect();
        AddCurrentHp(-damage);
        
        
        if (damageInfo.bIsCritical)
            SpawnDamageText(Define.EDamageTextType.PlayerDamagedCritical, damage);
        else
            SpawnDamageText(Define.EDamageTextType.PlayerDamaged, damage);
        
        if (characterStats.currentHp.Value <= 0f)
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

    protected override void AddCurrentHp(float value)
    {
        base.AddCurrentHp(value);

        RefreshCurrentHpUI();
    }

    public void HealMaxHpPer(float per)
    {
        float addToValue = characterStats.maxHp.Value * per * 0.01f;
        AddCurrentHp(addToValue);
    }

    public void InitCurrentHp(float currentHp)
    {
        characterStats.currentHp.Value = currentHp;
        RefreshCurrentHpUI();
    }

    private void RefreshCurrentHpUI()
    {
        float ratio = Mathf.Round((characterStats.currentHp.Value / characterStats.maxHp.Value * 100f) * 10) * 0.1f; 
        GI.Inst.UIManager.SetHpBar(ratio);
    }
    
    public override void ExecDurationEffect(Effect effect, Sprite icon)
    {
        DurationEffect newDurationEffect = effect as DurationEffect;
        
        if (newDurationEffect != null)
        {
            EDurationEffectId effectId = newDurationEffect.durationEffectId;
            //이미 있는지 확인,
            if (DurationEffectDurationDict.ContainsKey(effectId))
            {
                //이미 있으면 Reset인지, 누적인지 확인
                if (newDurationEffect.bIsResetDuration) //Reset이면 duration을 리셋 => endTime을 새로운거로 바꿔준다.
                {
                    DurationEffectDurationDict[effectId] = newDurationEffect.duration;
                    
                    DurationEffectEndTimePq.Peek().effectInfo.onExecuteDecreaseStat?.Invoke();
                    DurationEffectEndTimePq.Delete(item => item.durationEffectId == newDurationEffect.durationEffectId);
                    
                    DurationEffectEndTimePq.Push(newDurationEffect);
                    newDurationEffect.effectInfo.onExecuteIncreaseStat?.Invoke();
                    
                    IconFillAmount[effectId] = 1;
                }
                else //누적이면 기존 duration에 새로운 duration 더하기 => 기존 endTime에 duration 더하기
                {
                    DurationEffect oldEffect = DurationEffectEndTimePq.FindPop(item =>
                        item.durationEffectId == effectId);
                    oldEffect.effectInfo.onExecuteDecreaseStat?.Invoke();
                    
                    float leftDuration = oldEffect.durationEndTime - Time.time; //남은 지속 시간
                    DurationEffectDurationDict[effectId] = leftDuration + newDurationEffect.duration;
                    
                    oldEffect.durationEndTime += newDurationEffect.duration;
                    newDurationEffect.durationEndTime = oldEffect.durationEndTime; //갱신된 콜백 호출위해 end time만 덮어쓰기.
                    
                    DurationEffectEndTimePq.Push(newDurationEffect);
                    newDurationEffect.effectInfo.onExecuteIncreaseStat?.Invoke();
                    
                    IconFillAmount[effectId] = 1;
                    
                }
            }
            else  //없으면 새롭게 추가
            {
                DurationEffectDurationDict.Add(effectId, newDurationEffect.duration);
                
                DurationEffectEndTimePq.Push(newDurationEffect);
                newDurationEffect.effectInfo.onExecuteIncreaseStat?.Invoke();
                
                GI.Inst.UIManager.SetEffectSlot(effectId, icon);
                IconFillAmount.Add(effectId, 1);
            }
            RefreshInventoryUI();
        }
    }
    
    public float GetFillAmount(EDurationEffectId effectId)
    {
        if (IconFillAmount.ContainsKey(effectId))
            return IconFillAmount[effectId];
        return 0f;
    }

    public void OutOfBoundDead()
    {
        AddCurrentHp(-characterStats.currentHp.Value);
        Dead();
    }
    
    protected override void Dead()
    {
        if (IsDead) return;
        
        IsDead = true;
        
        Player player = GetComponent<Player>();
        if (player)
        {
            player.TransitionState(Define.EPlayerState.Dead);
            StartCoroutine(InitPlayerIfDead(2f, player));
            GI.Inst.SceneLoadManager.RequestLoadSceneAsync("Town", 2f);
        }
    }

    IEnumerator InitPlayerIfDead(float second, Player player)
    {
        yield return new WaitForSeconds(second);
        AddCurrentHp(characterStats.maxHp.Value * 0.5f);
        IsDead = false;
        player.TransitionState(Define.EPlayerState.Idle);
    }

    public override void TakeTrapDamage()
    {
        if (IsDead) return;
        
        
        float subToHp = characterStats.maxHp.Value * 0.2f;
        
        Character.HitEffect();
        AddCurrentHp(-subToHp);
        SpawnDamageText(Define.EDamageTextType.DamagedByTrap, subToHp);
        
        if (characterStats.currentHp.Value <= 0f)
        {
            Dead();
            return;
        }
        
        GI.Inst.ListenerManager.ExecTakeDamageEffect(Define.EDamageType.Both, null);
    }
}
