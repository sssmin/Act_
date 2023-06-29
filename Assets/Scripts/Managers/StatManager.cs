using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ETakeDamageResult
{
    TakeDamageOnly,
    Dead
}

[Serializable]
public class Stats
{
    public Stat attack = new Stat(Define.EStatType.Attack);
    public Stat defence = new Stat(Define.EStatType.Defence);
    public Stat elemAttack = new Stat(Define.EStatType.ElemAttack);
    public Stat maxHp = new Stat(Define.EStatType.MaxHp);
    public Stat currentHp = new Stat(Define.EStatType.None);
    public Stat criticalChancePer = new Stat(Define.EStatType.CriticalChancePer); // - %임 Value가 50이면 50%.
    public Stat criticalResistPer = new Stat(Define.EStatType.CriticalResistPer); // - %임 Value가 50이면 50%.
    public Stat criticalDamageIncPer = new Stat(Define.EStatType.CriticalDamageIncPer); //크리티컬 피해량 증가율
    public Stat normalAttackDamageIncPer = new Stat(Define.EStatType.NormalAttackDamageIncPer); //기본 공격 피해량 증가율
    public Stat skillAttackDamageIncPer = new Stat(Define.EStatType.SkillAttackDamageIncPer); //스킬 피해량 증가율
    public Stat evasionChancePer = new Stat(Define.EStatType.EvasionChancePer);
    public Stat skillCooltimeDecRate = new Stat(Define.EStatType.SkillCooltimeDecRate);
    public Stat moveSpeed = new Stat(Define.EStatType.MoveSpeed);
    
    public void StatCopy(Stats stats)
    {
        attack.StatCopy(stats.attack);
        defence.StatCopy(stats.defence);
        elemAttack.StatCopy(stats.elemAttack);
        maxHp.StatCopy(stats.maxHp);
        currentHp.StatCopy(stats.currentHp);
        criticalChancePer.StatCopy(stats.criticalChancePer);
        criticalResistPer.StatCopy(stats.criticalResistPer);
        criticalDamageIncPer.StatCopy(stats.criticalDamageIncPer);
        normalAttackDamageIncPer.StatCopy(stats.normalAttackDamageIncPer);
        skillAttackDamageIncPer.StatCopy(stats.skillAttackDamageIncPer);
        evasionChancePer.StatCopy(stats.evasionChancePer);
        skillCooltimeDecRate.StatCopy(stats.skillCooltimeDecRate);
        moveSpeed.StatCopy(stats.moveSpeed);
    }

    public void InitStat(Stats stats)
    {
        StatCopy(stats);
        currentHp.StatCopy(stats.maxHp);
        //ui 갱신 필요
    }
}

[Serializable]
public class Stat
{
    public Stat(){}

    public Stat(Define.EStatType type) { statType = type;} 
    public Stat(Stat other)
    {
        statValue = other.statValue;
        Modifiers = new List<float>(other.Modifiers);
        statType = other.statType;
    }

    public Define.EStatType statType;
    [SerializeField]
    private float statValue;
    public float Value
    {
        get
        {
            float finalValue = statValue;
            if (Modifiers.Count > 0)
            {
                foreach (var modi in Modifiers)
                {
                    finalValue += modi;
                }
            }
            return finalValue;
        }
        set => statValue = value;
    }

    public float DefaultValue
    {
        get => statValue;
    }

    public float ModifierValue
    {
        get
        {
            float modifierValue = 0;
            foreach (var modi in Modifiers)
            {
                modifierValue += modi;
            }
            return modifierValue;
        }
    }
    
    private List<float> Modifiers = new List<float>();
    
    public void AddModifier(float value)
    {
        if (value != 0)
            Modifiers.Add(value);
    }

    public void SubModifier(float value)
    {
        if (value != 0)
            Modifiers.Remove(value);
    }
    
    public void StatCopy(Stat stat)
    {
        statValue = stat.statValue;
        Modifiers = new List<float>(stat.Modifiers);
    }
}

public struct DamageInfo
{
    public float damage;
    public bool bIsCritical;
}


public class StatManager : MonoBehaviour
{
    public int InstId { get; set; }
    public Stats stats = new Stats();
    [SerializeField] private Transform damageSpawnTransform;
    
    public PriorityQueue<DurationEffect> DurationEffectEndTimePq { get; set; } = new PriorityQueue<DurationEffect>();
    //value = 지속시간
    public Dictionary<EDurationEffectId, float> DurationEffectDurationDict { get; set; } = new Dictionary<EDurationEffectId, float>();
    public Dictionary<EDurationEffectId, float> IconFillAmount { get; set; } = new Dictionary<EDurationEffectId, float>();

    public BaseCharacter Character { get; set; }
    
    protected bool IsDead { get; private set; }

    public virtual void Awake()
    {
        
        
    }

    public virtual void Start()
    {
        //DamageSpawnTransform = GameObject.FindGameObjectWithTag("DamageSpawnTransform").GetComponent<Transform>();
    }

    public virtual void Update()
    {
    }

    public void InitStat(Stats inStat)
    {
        stats.InitStat(inStat);
    }

    public Stats GetStats(int instanceId)
    {
        if (instanceId == InstId)
            return stats;
        return null;
    }

    private void SpawnDamageText(Define.EDamageTextType damageTextType, float damage = 0)
    {
        float randValueX = Random.Range(-1f, 1f);
        float randValueY = Random.Range(-0.5f, 0.5f);
        Vector3 spawnPos = new Vector3(damageSpawnTransform.position.x + randValueX,
            damageSpawnTransform.position.y + randValueY);
        GI.Inst.UIManager.SpawnDamageText(damageTextType, spawnPos, damage);
    }

    public virtual void TakeDamage(DamageInfo damageInfo, StatManager instigatorStatManager, Define.EDamageType damageType)
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
                //Debug.Log($"크리 적용 전 대미지 산출 : {damage}");
            }
        }

        float defence = stats.defence.Value;
        damage = Mathf.Clamp((damage * Random.Range(0.9f, 1.1f)) * (1 - (defence / (100 + defence))), 0f, float.MaxValue);
        damage = Mathf.Round((damage) * 10) * 0.1f;
        //Debug.Log($"{gameObject.name} 최종 받는 대미지 {damageInfo.damage}");
        //Hit Effect 적용
        Character.HitEffect();
        AddCurrentHp(-damage);
   
        
        SkillManager instigatorSkillManager = instigatorStatManager.GetComponent<SkillManager>();

        if (instigatorSkillManager)
        {
            if (damageInfo.bIsCritical)
            {
                SpawnDamageText(Define.EDamageTextType.MonsterDamagedCritical, damage);
            }
            else
            {
                SpawnDamageText(Define.EDamageTextType.MonsterDamaged, damage);
            }
        }
        else
        {
            if (damageInfo.bIsCritical)
            {
                SpawnDamageText(Define.EDamageTextType.PlayerDamagedCritical, damage);
            }
            else
            {
                SpawnDamageText(Define.EDamageTextType.PlayerDamaged, damage);
            }
        }


        if (stats.currentHp.Value <= 0f)
        {
            Dead();
            if (instigatorSkillManager)
            {
                instigatorSkillManager.CauseDamageSuccessfully(damageType, ETakeDamageResult.Dead, this);
                return;
            }
        }

        if (instigatorSkillManager)
        {
            instigatorSkillManager.CauseDamageSuccessfully(damageType, ETakeDamageResult.TakeDamageOnly, this);
        }
        
    }

    public void CauseNormalAttack(StatManager enemyStatManager, float normalAttackCoef)
    {
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = CalcDamage();
        //Debug.Log(damageInfo.damage);
        damageInfo.damage = Mathf.Round((damageInfo.damage * normalAttackCoef) * 10) * 0.1f; //계수 적용
        //Debug.Log(damageInfo.damage);
        //********* 기본공격 피해량 증가분
        damageInfo.damage += Mathf.Round(damageInfo.damage * (stats.normalAttackDamageIncPer.Value / 100f) * 10) * 0.1f;
        //Debug.Log(damageInfo.damage);
        
        CalcCriticalDamage(ref damageInfo);
        
        enemyStatManager.TakeDamage(damageInfo, this, Define.EDamageType.Normal);
    }
    

    //스킬 대미지 계산 전 기본 대미지
    public DamageInfo GetDefaultDamage()
    {
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = CalcDamage();
        
        //********* 스킬공격 피해량 증가분
        damageInfo.damage += Mathf.Round((damageInfo.damage * (stats.skillAttackDamageIncPer.Value / 100f)) * 10) * 0.1f;
        
        CalcCriticalDamage(ref damageInfo);
        
        return damageInfo;
    }
    
    private float CalcDamage()
    {
        //todo 속성은 일단 보류
        float damage = stats.attack.Value + stats.elemAttack.Value;
        
        return damage;
    }

    private void CalcCriticalDamage(ref DamageInfo damageInfo)
    {
        int rand = Random.Range(0, 100);
        if (stats.criticalChancePer.Value > rand)
        {
            damageInfo.bIsCritical = true;
            //********* 크리티컬 피해량 증가분 계산
            damageInfo.damage += Mathf.Round((damageInfo.damage * (1.5f + stats.criticalDamageIncPer.Value / 100f)) * 10) * 0.1f;
        }
    }
    
    public virtual void AddCurrentHp(float value)
    {
        stats.currentHp.Value = Mathf.Clamp(stats.currentHp.Value + value, 0f, stats.maxHp.Value);
        float ratio = stats.currentHp.Value / stats.maxHp.Value * 100f;
        //hpbar ui에 보내기.
    }

    void Dead()
    {
        if (IsDead) return;
        
        IsDead = true;
        
        Monster monster = GetComponent<Monster>();
        if (monster)
        {
            monster.TransitionState(Define.EMonsterState.Dead);
            monster.DropTable.DropItem();
        }
        else
        {
            Player player = GetComponent<Player>();
            player.TransitionState(Define.EPlayerState.Dead);
        }
    }

    public void ExecDurationEffect(Effect effect, Sprite icon)
    {
        DurationEffect durationEffect = effect as DurationEffect;
        
        if (durationEffect != null)
        {
            EDurationEffectId effectId = durationEffect.durationEffectId;
            //이미 있는지 확인,
            if (DurationEffectDurationDict.ContainsKey(effectId))
            {
                //이미 있으면 Reset인지, 누적인지 확인
                if (durationEffect.bIsResetDuration) //Reset이면 duration을 리셋 => endTime을 새로운거로 바꿔준다.
                {
                    DurationEffectDurationDict[effectId] = durationEffect.duration;
                    
                    DurationEffectEndTimePq.Delete(item => item.durationEffectId == durationEffect.durationEffectId);

                    DurationEffect tempEffect = new DurationEffect();
                    tempEffect = durationEffect;
                    DurationEffectEndTimePq.Push(tempEffect);
                    
                    IconFillAmount[effectId] = 1;
                }
                else //누적이면 기존 duration에 새로운 duration 더하기 => 기존 endTime에 duration 더하기
                {
                    DurationEffect tempEffect = DurationEffectEndTimePq.FindPop(item =>
                        item.durationEffectId == effectId);
                    
                    float leftDuration = tempEffect.durationEndTime - Time.time;
                    DurationEffectDurationDict[effectId] = leftDuration;
                    DurationEffectDurationDict[effectId] += durationEffect.duration;
                    
                    tempEffect.durationEndTime += durationEffect.duration;
                    DurationEffectEndTimePq.Push(tempEffect);
                    
                    IconFillAmount[effectId] = 1;
                }
            }
            else  //없으면 새롭게 추가
            {
                DurationEffectDurationDict.Add(effectId, durationEffect.duration);
                
                DurationEffectEndTimePq.Push(durationEffect);
                
                durationEffect.effectInfo.onExecuteIncreaseStat?.Invoke();
                RefreshInventoryUI();
                
                GI.Inst.UIManager.SetEffectSlot(effectId, icon);
                IconFillAmount.Add(effectId, 1);
                
            }
        }
    }

    protected void RefreshInventoryUI()
    {
        GI.Inst.UIManager.RefreshInventoryUI();
    }
    
    protected void StatAddModifier(int InstId, List<Stat> statsList)
    {
        if (InstId == this.InstId)
        {
            foreach (Stat stat in statsList)
            {
                switch (stat.statType)
                {
                    case Define.EStatType.Attack:
                        stats.attack.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.Defence:
                        stats.defence.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.ElemAttack:
                        stats.elemAttack.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.MaxHp:
                        stats.maxHp.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalChancePer:
                        stats.criticalChancePer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalResistPer:
                        stats.criticalResistPer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalDamageIncPer:
                        stats.criticalDamageIncPer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.NormalAttackDamageIncPer:
                        stats.normalAttackDamageIncPer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.SkillAttackDamageIncPer:
                        stats.skillAttackDamageIncPer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.EvasionChancePer:
                        stats.evasionChancePer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.SkillCooltimeDecRate:
                        stats.skillCooltimeDecRate.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.MoveSpeed:
                        stats.moveSpeed.AddModifier(stat.Value);
                        break;
                }
            }
        }
    }
    
    public void StatSubModifier(int InstId, List<Stat> statsList)
    {
        if (InstId == this.InstId)
        {
            foreach (Stat stat in statsList)
            {
                switch (stat.statType)
                {
                    case Define.EStatType.Attack:
                        stats.attack.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.Defence:
                        stats.defence.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.ElemAttack:
                        stats.elemAttack.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.MaxHp:
                        stats.maxHp.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalChancePer:
                        stats.criticalChancePer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalResistPer:
                        stats.criticalResistPer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalDamageIncPer:
                        stats.criticalDamageIncPer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.NormalAttackDamageIncPer:
                        stats.normalAttackDamageIncPer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.SkillAttackDamageIncPer:
                        stats.skillAttackDamageIncPer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.EvasionChancePer:
                        stats.evasionChancePer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.SkillCooltimeDecRate:
                        stats.skillCooltimeDecRate.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.MoveSpeed:
                        stats.moveSpeed.SubModifier(stat.Value);
                        break;
                }
            }
        }
    }

    public float GetFillAmount(EDurationEffectId effectId)
    {
        if (IconFillAmount.ContainsKey(effectId))
            return IconFillAmount[effectId];
        return 0f;
    }

    public void ApplyHealthSteal(float percentage)
    {
        float loseHp = stats.maxHp.Value - stats.currentHp.Value;
        float addToHp = loseHp * percentage * 0.01f;
        AddCurrentHp(addToHp);
    }

    public bool IsCurrentHpBelowPercent(float per)
    {
        //최대 * 퍼센트 * 0.01 >= 현재
        return stats.maxHp.Value * per * 0.01f >= stats.currentHp.Value;
    }
    
}
