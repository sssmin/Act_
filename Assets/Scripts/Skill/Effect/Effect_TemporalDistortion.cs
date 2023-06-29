using UnityEngine;

public class Effect_TemporalDistortion : Effect
{
    public override void CheckConditionAndExecute(Define.EDamageType inDamageType, Define.EActivationCondition inActivationCondition,
        StatManager enemyStatManager, StatManager casterStatManager, Sprite icon)
    {
        if (((damageType == Define.EDamageType.Both) || (damageType == inDamageType)) && activationCondition == inActivationCondition)
        {
            if (activateChancePer >= Random.Range(0f, 100f))
            {
                //액티브 스킬 랜덤 초기화
                GI.Inst.CooltimeManager.ResetCooltimeRandomActive();
                //스킬 쿨타임 적용
                GI.Inst.CooltimeManager.SetPassiveCooltime(Define.ESkillId.TemporalDistortion, skillCooltime);
                GI.Inst.UIManager.SetPassiveCooltimeSlot(Define.ESkillId.TemporalDistortion, icon);
            }
        }
    }
}
