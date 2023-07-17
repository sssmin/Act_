using System;

[Serializable]
public class DurationEffect : Effect, IComparable<DurationEffect>
{
    public float duration;
    public EDurationEffectId durationEffectId;
    public float durationEndTime; //지속시간이 끝나는 시간.
    //만약 쿨타임이 지속시간보다 짧아서 다시 Effect가 발동됐을 때 지속시간을 리셋할 것인지, 기존에 더할 것인지
    //boost potion 같은 게 기존에 더하는 케이스 (false)
    public bool bIsResetDuration; 
    
    
    public void Init(Define.EActivationCondition inActivationCondition, float inActivateChancePer,
        Define.EDamageType inDamageType, EffectInfo inEffectInfo, float inDuration, 
        EDurationEffectId effectId, bool isResetDuration)
    {
        activationCondition = inActivationCondition;
        activateChancePer = inActivateChancePer;
        damageType = inDamageType;
        effectInfo = inEffectInfo;
        duration = inDuration;
        durationEffectId = effectId;
        bIsResetDuration = isResetDuration;
    }
    
    public int CompareTo(DurationEffect other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
       
        return -durationEndTime.CompareTo(other.durationEndTime);
    }
}
