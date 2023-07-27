using UnityEngine;

public class DurationEffect_LastStand : DurationEffect
{
    public override void CheckConditionAndExecute(Define.EDamageType inDamageType, Define.EActivationCondition inActivationCondition,
        StatManager enemyStatManager, StatManager casterStatManager, Sprite icon)
    {
        if (((damageType == Define.EDamageType.Both) || (damageType == inDamageType)) && activationCondition == inActivationCondition)
        {
            //체력이 20% 이하면
            if (casterStatManager.IsCurrentHpBelowPercent(20f))
            {
                durationEndTime = duration + Time.time;
                casterStatManager.ExecDurationEffect(this, icon);
                //스킬 쿨타임 적용
                GI.Inst.CooltimeManager.SetPassiveCooltime(Define.ESkillId.LastStand, skillCooltime);
                GI.Inst.UIManager.SetPassiveCooltimeSlot(Define.ESkillId.LastStand, icon);
            }
        }
    }
}
