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
    public Stat attackIncValue = new Stat(Define.EStatType.AttackIncValue);
    public Stat defence = new Stat(Define.EStatType.Defence);
    public Stat defenceIncValue = new Stat(Define.EStatType.DefenceIncValue);
    public Stat maxHp = new Stat(Define.EStatType.MaxHp);
    public Stat currentHp = new Stat(Define.EStatType.CurrentHp);
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
        attackIncValue.StatCopy(stats.attackIncValue);
        defence.StatCopy(stats.defence);
        defenceIncValue.StatCopy(stats.defenceIncValue);
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
    }
    
    public void InitMonsterStat(Stats stats, int level)
    {
        StatCopy(stats);
        ModifierMonsterLevel(stats, level);
        currentHp.StatCopy(stats.maxHp);
        currentHp.AddModifier(stats.currentHp.Value * (level - 1));
    }
    
    public void ModifierMonsterLevel(Stats stats, int level)
    {
        attack.AddModifier(stats.attack.Value * (level - 1));
        attackIncValue.AddModifier(stats.attackIncValue.Value * (level - 1));
        defence.AddModifier(stats.defence.Value * (level - 1));
        defenceIncValue.AddModifier(stats.defenceIncValue.Value * (level - 1));
        maxHp.AddModifier(stats.maxHp.Value * (level - 1));
        criticalChancePer.AddModifier(stats.criticalChancePer.Value * (level - 1));
        criticalResistPer.AddModifier(stats.criticalResistPer.Value * (level - 1));
        criticalDamageIncPer.AddModifier(stats.criticalDamageIncPer.Value * (level - 1));
        normalAttackDamageIncPer.AddModifier(stats.normalAttackDamageIncPer.Value * (level - 1));
        skillAttackDamageIncPer.AddModifier(stats.skillAttackDamageIncPer.Value * (level - 1));
        evasionChancePer.AddModifier(stats.evasionChancePer.Value * (level - 1));
        skillCooltimeDecRate.AddModifier(stats.skillCooltimeDecRate.Value * (level - 1));
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

    public Define.EStatType statType = Define.EStatType.None;
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
        set
        {
            statValue = value;
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

    public void ClearModifier()
    {
        Modifiers.Clear();
    }
    
    public void StatCopy(Stat stat)
    {
        statValue = stat.statValue;
        if (statType != Define.EStatType.CurrentHp)
            statType = stat.statType;
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
    public Stats characterStats = new Stats();
    [SerializeField] private Transform damageSpawnTransform;
    
    protected PriorityQueue<DurationEffect> DurationEffectEndTimePq { get; set; } = new PriorityQueue<DurationEffect>();
    //value = 지속시간
    protected Dictionary<EDurationEffectId, float> DurationEffectDurationDict { get; set; } = new Dictionary<EDurationEffectId, float>();
    
    public BaseCharacter Character { get; set; }
    
    public bool IsDead { get; protected set; }

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }

    public void InitStat(Stats inStat)
    {
        characterStats.InitStat(inStat);
    }

    protected Stats GetStats(int instanceId)
    {
        if (instanceId == InstId)
            return characterStats;
        return null;
    }

    protected void SpawnDamageText(Define.EDamageTextType damageTextType, float damage = 0)
    {
        float randValueX = Random.Range(-1f, 1f);
        float randValueY = Random.Range(-0.5f, 0.5f);
        Vector3 spawnPos = new Vector3(damageSpawnTransform.position.x + randValueX,
            damageSpawnTransform.position.y + randValueY);
        GI.Inst.UIManager.SpawnDamageText(damageTextType, spawnPos, damage);
    }

    public virtual void TakeDamage(DamageInfo damageInfo, StatManager instigatorStatManager, Define.EDamageType damageType)
    {
        
    }

    public virtual void CauseNormalAttack(StatManager enemyStatManager, float normalAttackCoef = 1f)
    {
        if (enemyStatManager)
        {
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.damage = CalcDamage();
      
            damageInfo.damage = Mathf.Round((damageInfo.damage * normalAttackCoef) * 10) * 0.1f; //계수 적용
        
            //********* 기본공격 피해량 증가분
            damageInfo.damage += Mathf.Round(damageInfo.damage * (characterStats.normalAttackDamageIncPer.Value / 100f) * 10) * 0.1f;
        
            CalcCriticalDamage(ref damageInfo);
        
            enemyStatManager.TakeDamage(damageInfo, this, Define.EDamageType.Normal);
        }
    }
    
    //스킬 대미지 계산 전 기본 대미지
    public DamageInfo GetDefaultDamage()
    {
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = CalcDamage();
        
        //********* 스킬공격 피해량 증가분
        damageInfo.damage += Mathf.Round((damageInfo.damage * (characterStats.skillAttackDamageIncPer.Value / 100f)) * 10) * 0.1f;
        
        CalcCriticalDamage(ref damageInfo);
        
        return damageInfo;
    }
    
    private float CalcDamage()
    {
        float damage = characterStats.attack.Value + characterStats.attackIncValue.Value;
        
        return damage;
    }

    private void CalcCriticalDamage(ref DamageInfo damageInfo)
    {
        int rand = Random.Range(0, 100);
        if (characterStats.criticalChancePer.Value > rand)
        {
            damageInfo.bIsCritical = true;
            //********* 크리티컬 피해량 증가분 계산
            damageInfo.damage += Mathf.Round((damageInfo.damage * (1.5f + characterStats.criticalDamageIncPer.Value / 100f)) * 10) * 0.1f;
        }
    }
    
    protected virtual void AddCurrentHp(float value)
    {
        characterStats.currentHp.Value = Mathf.Clamp(characterStats.currentHp.Value + value, 0f, characterStats.maxHp.Value);
        
    }

    protected virtual void Dead()
    {
        
    }

    public virtual void ExecDurationEffect(Effect effect, Sprite icon)
    {
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
                        characterStats.attack.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.Defence:
                        characterStats.defence.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.MaxHp:
                        characterStats.maxHp.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalChancePer:
                        characterStats.criticalChancePer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalResistPer:
                        characterStats.criticalResistPer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalDamageIncPer:
                        characterStats.criticalDamageIncPer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.NormalAttackDamageIncPer:
                        characterStats.normalAttackDamageIncPer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.SkillAttackDamageIncPer:
                        characterStats.skillAttackDamageIncPer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.EvasionChancePer:
                        characterStats.evasionChancePer.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.SkillCooltimeDecRate:
                        characterStats.skillCooltimeDecRate.AddModifier(stat.Value);
                        break;
                    case Define.EStatType.MoveSpeed:
                        characterStats.moveSpeed.AddModifier(stat.Value);
                        break;
                }
            }
        }
    }
    
    protected void StatSubModifier(int InstId, List<Stat> statsList)
    {
        if (InstId == this.InstId)
        {
            foreach (Stat stat in statsList)
            {
                switch (stat.statType)
                {
                    case Define.EStatType.Attack:
                        characterStats.attack.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.Defence:
                        characterStats.defence.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.MaxHp:
                        characterStats.maxHp.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalChancePer:
                        characterStats.criticalChancePer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalResistPer:
                        characterStats.criticalResistPer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.CriticalDamageIncPer:
                        characterStats.criticalDamageIncPer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.NormalAttackDamageIncPer:
                        characterStats.normalAttackDamageIncPer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.SkillAttackDamageIncPer:
                        characterStats.skillAttackDamageIncPer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.EvasionChancePer:
                        characterStats.evasionChancePer.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.SkillCooltimeDecRate:
                        characterStats.skillCooltimeDecRate.SubModifier(stat.Value);
                        break;
                    case Define.EStatType.MoveSpeed:
                        characterStats.moveSpeed.SubModifier(stat.Value);
                        break;
                }
            }
        }
    }

    public void HealPerLoseHp(float percentage) //잃은 체력의 % 회복
    {
        float loseHp = characterStats.maxHp.Value - characterStats.currentHp.Value;
        float addToHp = loseHp * percentage * 0.01f;
        AddCurrentHp(addToHp);
        SpawnDamageText(Define.EDamageTextType.Heal, addToHp);
    }

    public bool IsCurrentHpBelowPercent(float per)
    {
        //최대 * 퍼센트 * 0.01 >= 현재
        return characterStats.maxHp.Value * per * 0.01f >= characterStats.currentHp.Value;
    }

    public virtual void TakeTrapDamage()
    {
        
    }
    
}
