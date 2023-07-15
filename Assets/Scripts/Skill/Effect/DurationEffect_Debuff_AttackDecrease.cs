using UnityEngine;

public class DurationEffect_Debuff_AttackDecrease : DurationEffect
{
    public override void CheckConditionAndExecute(Define.EDamageType inDamageType, Define.EActivationCondition inActivationCondition, 
        StatManager enemyStatManager, StatManager casterStatManager, Sprite icon)
    {
        if (((damageType == Define.EDamageType.Both) || (damageType == inDamageType)) && activationCondition == inActivationCondition)
        {
            durationEndTime = duration + Time.time;
            casterStatManager.ExecDurationEffect(this, icon);
            //스킬 쿨타임 적용
            
        }
    }
}
