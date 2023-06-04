using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public List<int> Modifiers { get; set; }= new List<int>();
    

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
    public Stats Stats = new Stats();
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitStat()
    {
        
    }

    //instigator는 
    public void OnDamage(DamageInfo damageInfo, GameObject instigator)
    {
        //회피
        int rand = Random.Range(0, 100);
        if (Stats.evasionChancePer.Value > rand)
            return;
        if (damageInfo.bIsCritical)
        {
            rand = Random.Range(0, 100);
            //크리 저항
            if (Stats.criticalResistPer.Value > rand)
            {
                //크리 적용 전 대미지
                //Result = baseDamage + baseDamage * 1.5  => Result = baseDamage*(1 + 1.5) => baseDamage = Result/(1 + 1.5)
                damageInfo.damage = Mathf.RoundToInt(damageInfo.damage / 2.5f);
                //Debug.Log($"크리 적용 전 대미지 산출 : {damage}");
            }
        }

        int defence = Stats.defence.Value;
        damageInfo.damage = Mathf.RoundToInt(Mathf.Clamp((damageInfo.damage * Random.Range(0.9f, 1.1f)) * (1 - ((float)defence / (100 + defence))), 0, Int32.MaxValue));
        //Debug.Log($"{gameObject.name} 최종 받는 대미지 {damage}");
        AddCurrentHp(-damageInfo.damage);
    }
    
    public void CalcDmgNormalAttack(StatManager enemyStatManager, float coefficient)
    {
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = CalcDamage();
        
        damageInfo.damage = Mathf.RoundToInt(damageInfo.damage * coefficient); //계수가 먼저 적용되어야 할듯.
        
        //Debug.Log($"크리 적용 전 대미지 : {damageInfo.damage}");
        damageInfo.bIsCritical = false;
        CalcCriticalDamage(ref damageInfo);
        //Debug.Log($"크리 적용 후 대미지 : {damageInfo.damage}");
        enemyStatManager.OnDamage(damageInfo, gameObject);
    }


    public void CalcDmgSkillAttack(StatManager enemyStatManager)
    {
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = CalcDamage();
        
        CalcCriticalDamage(ref damageInfo);
        enemyStatManager.OnDamage(damageInfo, gameObject);
    }

    //스킬을 쐈을 때 아예 모든걸 계산해서 스킬 오브젝트에 담아 보내기
    public DamageInfo GetDefaultDamage()
    {
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = CalcDamage();
        CalcCriticalDamage(ref damageInfo);
        
        return damageInfo;
    }
    
    private int CalcDamage()
    {
        //todo 속성은 일단 보류
        int damage = Stats.attack.Value + Stats.elemAttack.Value;
        
        return damage;
    }

    private void CalcCriticalDamage(ref DamageInfo damageInfo)
    {
        int rand = Random.Range(0, 100);
        if (Stats.criticalChancePer.Value > rand)
        {
            damageInfo.bIsCritical = true;
            damageInfo.damage += Mathf.RoundToInt(damageInfo.damage * 1.5f);
        }
    }


    public void AddCurrentHp(int value)
    {
        Stats.currentHp.Value = Mathf.Clamp(Stats.currentHp.Value + value, 0, Stats.maxHp.Value);
    }
}
