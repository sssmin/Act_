using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationEffect_DeadlyImpact : DurationEffect
{
    public override void CheckConditionAndExecute(Define.EDamageType inDamageType, Define.EActivationCondition inActivationCondition,
        StatManager enemyStatManager, StatManager casterStatManager, Sprite icon)
    {
        if (((damageType == Define.EDamageType.Both) || (damageType == inDamageType)) && activationCondition == inActivationCondition)
        {
            durationEndTime = duration + Time.time;
            casterStatManager.ExecDurationEffect(this, icon);
            //스킬 쿨타임 적용
            GI.Inst.CooltimeManager.SetPassiveCooltime(Define.ESkillId.DeadlyImpact, skillCooltime);
            GI.Inst.UIManager.SetPassiveCooltimeSlot(Define.ESkillId.DeadlyImpact, icon);
        }
    }
}
