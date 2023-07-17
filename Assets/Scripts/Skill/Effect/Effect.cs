using System;
using UnityEngine;

public enum EDurationEffectId
{
    None,
    ArrowBuff,
    AtkBoostPotion,
    LastStand,
    NimbleReflexes,
    DeadlyImpact,
    Burn,
    Frozen,
    Poison,
    
    Max
}

[Serializable]
public class EffectInfo
{
    public Action onExecuteIncreaseStat;
    public Action onExecuteDecreaseStat;

    //레벨에 따라 증가하는 적용 퍼센트 ex) 회피율 7.5%~15% , 피흡량 5~10%
    public float applyPerBySkillLevel;
}

[Serializable]
public class Effect
{
    public Define.EActivationCondition activationCondition; //None이면 상시적용
    public float activateChancePer; //발동 확률. -1이면 상시 적용
    public Define.EDamageType damageType;
    public EffectInfo effectInfo;
    public float skillCooltime; //이 이펙트를 가진 스킬의 쿨타임. -1이면 스킬 쿨타임 없음

    public void Init(Define.EActivationCondition inActivationCondition, float inActivateChancePer,
        Define.EDamageType inDamageType, EffectInfo inEffectInfo)
    {
        activationCondition = inActivationCondition;
        activateChancePer = inActivateChancePer;
        damageType = inDamageType;
        effectInfo = inEffectInfo;
    }

    public virtual void CheckConditionAndExecute(Define.EDamageType inDamageType, Define.EActivationCondition inActivationCondition, 
        StatManager enemyStatManager, StatManager casterStatManager, Sprite icon)
    {
        
    }
}




