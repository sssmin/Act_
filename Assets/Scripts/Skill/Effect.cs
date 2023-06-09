using System;
using UnityEngine;




[Serializable]
public class Effect
{
    public Define.EActivationCondition activationCondition; //None이면 상시적용
    public float ActivateChancePer; //발동 확률. 0이면 상시 적용
    public Define.EDamageType damageType;
    public float duration; //0보다 크면 endTime이 있는거
    [HideInInspector] public float endTime;
    public EffectInfo effectInfo;
    
}

[Serializable]
public class EffectInfo
{
    public Define.EEffectType effectType;
    public Define.EEffectDetail effectDetail; //이 디테일에 따라 아래 델리게이트 Invoke
    
    public Func<int> onExecuteGetValue;
   
    public Func<int, int> onExecuteCalcValue; 
    
    public Action onExecuteIncreaseStat;
    public Action onExecuteDecreaseStat;
    public Action<int> onExecuteSpawn; 
    public Action<int> onExecuteDebuffToEnemy; 
  
}


