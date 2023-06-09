using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//enum 2개 추가 
//버프를 가진  아이템, 스킬 id

public class SkillAbility_ArrowBuff : MonoBehaviour
{
    [HideInInspector] public Effect effect;
    
    public void Init(Effect inEffect, StatManager casterStatManager, int perDmg)
    {
        effect = inEffect;
        effect.endTime = effect.duration + Time.time;
        effect.activationCondition = Define.EActivationCondition.ApplyImmediately;
        effect.effectInfo.effectDetail = Define.EEffectDetail.BuffMySelf;
        
        effect.effectInfo.onExecuteIncreaseStat = 
             () => casterStatManager.stats.normalAttackDamageIncPer.AddModifier(perDmg);
        
        effect.effectInfo.onExecuteDecreaseStat = 
             () => casterStatManager.stats.normalAttackDamageIncPer.SubModifier(perDmg);
        
        
        casterStatManager.AddSkillEffect(effect, Define.ESkillId.ArrowBuff);
        StartCoroutine(CoDestroyMySelf());
    }

    IEnumerator CoDestroyMySelf()
    {
        yield return new WaitForSeconds(2f);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }


    
}