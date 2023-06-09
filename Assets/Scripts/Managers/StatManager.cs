using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class Stats
{
    public Stat attack = new Stat();
    public Stat defence = new Stat();
    public Stat elemAttack = new Stat();
    public Stat maxHp = new Stat();
    public Stat currentHp = new Stat();
    public Stat criticalChancePer = new Stat(); // - %임 Value가 50이면 50%.
    public Stat criticalResistPer = new Stat(); // - %임 Value가 50이면 50%.
    public Stat criticalDamageIncPer = new Stat(); //크리티컬 피해량 증가율
    public Stat normalAttackDamageIncPer = new Stat(); //기본 공격 피해량 증가율
    public Stat skillAttackDamageIncPer = new Stat(); //스킬 피해량 증가율
    public Stat evasionChancePer = new Stat();
    public Stat skillCooltimeGrowthRate = new Stat();
    public Stat moveSpeed = new Stat();

}

[Serializable]
public class Stat
{
    public Stat(){}
    public Stat(Stat other)
    {
        statValue = other.statValue;
        Modifiers = new List<int>(other.Modifiers);
    }
    [SerializeField]
    private int statValue;
    public int Value
    {
        get
        {
            int finalValue = statValue;
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
    
    public List<int> Modifiers = new List<int>();
    
    public void AddModifier(int value)
    {
        if (value != 0)
            Modifiers.Add(value);
    }

    public void SubModifier(int value)
    {
        if (value != 0)
            Modifiers.Remove(value);
    }
}

public struct DamageInfo
{
    public int damage;
    public bool bIsCritical;
}




public class StatManager : MonoBehaviour
{
    public Stats stats = new Stats();
    public Dictionary<Define.ESkillId, Effect> skillEffects = new Dictionary<Define.ESkillId, Effect>();
    public Dictionary<Define.EItemId, Effect> itemEffects = new Dictionary<Define.EItemId, Effect>(); 

    void Start()
    {
        
    }

    void Update()
    {
        var statPair = skillEffects.ToArray();
        for (int i = 0; i < statPair.Length; i++)
        {
            if (statPair[i].Value.endTime <= Time.time) //끝난거
            {
                //todo 버프 삭제
                SubSkillEffect(statPair[i].Key);
            }
        }
        //
        // var enhancementPair = enhancementBuffs.ToArray();
        // for (int i = 0; i < enhancementPair.Length; i++)
        // {
        //     if (enhancementPair[i].Value.EndTime <= Time.time) //끝난거
        //     {
        //         //todo 버프 삭제
        //     }
        // }
    }

    public void InitStat()
    {
        
    }

    public void OnDamage(DamageInfo damageInfo, GameObject instigator)
    {
        //회피
        int rand = Random.Range(0, 100);
        if (stats.evasionChancePer.Value > rand)
            return;
        if (damageInfo.bIsCritical)
        {
            rand = Random.Range(0, 100);
            //크리 저항
            if (stats.criticalResistPer.Value > rand)
            {
                //크리 적용 전 대미지
                //Result = baseDamage + baseDamage * 1.5  => Result = baseDamage*(1 + 1.5) => baseDamage = Result/(1 + 1.5)
                damageInfo.damage = Mathf.RoundToInt(damageInfo.damage / 2.5f);
                //Debug.Log($"크리 적용 전 대미지 산출 : {damage}");
            }
        }

        int defence = stats.defence.Value;
        damageInfo.damage = Mathf.RoundToInt(Mathf.Clamp((damageInfo.damage * Random.Range(0.9f, 1.1f)) * (1 - ((float)defence / (100 + defence))), 0, Int32.MaxValue));
        Debug.Log($"{gameObject.name} 최종 받는 대미지 {damageInfo.damage}");
        
        AddCurrentHp(-damageInfo.damage);
    }

    public void CauseNormalAttack(StatManager enemyStatManager, float normalAttackCoef)
    {
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = CalcDamage();
        Debug.Log(damageInfo.damage);
        damageInfo.damage = Mathf.RoundToInt(damageInfo.damage * normalAttackCoef); //계수 적용
        Debug.Log(damageInfo.damage);
        //********* 기본공격 피해량 증가분
        damageInfo.damage += Mathf.RoundToInt(damageInfo.damage * (stats.normalAttackDamageIncPer.Value / 100f));
        Debug.Log(damageInfo.damage);
        
        CalcCriticalDamage(ref damageInfo);
        
        enemyStatManager.OnDamage(damageInfo, gameObject);
    }
    

    //스킬 대미지 계산 전 기본 대미지
    public DamageInfo GetDefaultDamage()
    {
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = CalcDamage();
        
        //********* 스킬공격 피해량 증가분
        damageInfo.damage += Mathf.RoundToInt(damageInfo.damage * (stats.skillAttackDamageIncPer.Value / 100f));
        
        CalcCriticalDamage(ref damageInfo);
        
        return damageInfo;
    }
    
    private int CalcDamage()
    {
        //todo 속성은 일단 보류
        int damage = stats.attack.Value + stats.elemAttack.Value;
        
        return damage;
    }

    private void CalcCriticalDamage(ref DamageInfo damageInfo)
    {
        int rand = Random.Range(0, 100);
        if (stats.criticalChancePer.Value > rand)
        {
            damageInfo.bIsCritical = true;
            //********* 크리티컬 피해량 증가분 계산
            damageInfo.damage += Mathf.RoundToInt(damageInfo.damage * (1.5f + stats.criticalDamageIncPer.Value / 100f));
        }
    }
    
    public void AddCurrentHp(int value)
    {
        stats.currentHp.Value = Mathf.Clamp(stats.currentHp.Value + value, 0, stats.maxHp.Value);
    }

    public void AddSkillEffect(Effect inEffect, Define.ESkillId skillId)
    {
        if (skillEffects.ContainsKey(skillId))
        {
            SubSkillEffect(skillId);
        }
        skillEffects.Add(skillId, inEffect);
        if (inEffect.activationCondition == Define.EActivationCondition.ApplyImmediately) //스탯 바로 적용
        {
            Debug.Log("적용해야댕");
            inEffect.effectInfo.onExecuteIncreaseStat.Invoke();
        }
    }
    
    public void SubSkillEffect(Define.ESkillId skillId)
    {
        if (skillEffects.ContainsKey(skillId))
        {
            //todo 적용된 거 없애는 과정 필요
            if (skillEffects[skillId].activationCondition == Define.EActivationCondition.ApplyImmediately) //스탯 바로 적용
            {
                skillEffects[skillId].effectInfo.onExecuteDecreaseStat.Invoke();
            }
            skillEffects.Remove(skillId);
            
        }
    }

    public void AddItemEffect(Effect inEffect, Define.EItemId itemId)
    {
        if (itemEffects.ContainsKey(itemId))
        {
            //todo 이미 같은 스킬에 의한 이펙트가 들어있다면 ?
        }
        itemEffects.Add(itemId, inEffect);
        if (inEffect.activationCondition == Define.EActivationCondition.ApplyImmediately) //스탯 바로 적용
        {
            Debug.Log("적용해야댕");
            inEffect.effectInfo.onExecuteIncreaseStat.Invoke();
        }
    }

   
    
    public void SubItemEffect(Define.EItemId itemId)
    {
        if (itemEffects.ContainsKey(itemId))
        {
            //todo 적용된 거 없애는 과정 필요
            itemEffects.Remove(itemId);
        }
    }
}
