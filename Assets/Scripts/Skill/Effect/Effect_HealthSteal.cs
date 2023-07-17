using UnityEngine;

public class Effect_HealthSteal : Effect
{
    public override void CheckConditionAndExecute(Define.EDamageType inDamageType, Define.EActivationCondition inActivationCondition, 
        StatManager enemyStatManager, StatManager casterStatManager, Sprite icon)
    {
        if (((damageType == Define.EDamageType.Both) || (damageType == inDamageType)) && activationCondition == inActivationCondition)
        {
            if (activateChancePer >= Random.Range(0f, 100f))
            {
                casterStatManager.HealPerLoseHp(effectInfo.applyPerBySkillLevel);
            }
        }
    }
}
