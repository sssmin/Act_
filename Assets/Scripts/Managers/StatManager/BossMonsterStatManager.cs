using UnityEngine;

public class BossMonsterStatManager : MonsterStatManager
{
    public override void TakeDamage(DamageInfo damageInfo, StatManager instigatorStatManager, Define.EDamageType damageType)
    {
        if (IsDead) return;
        float damage = damageInfo.damage;
        bool isDefend = false;
        
        //닷지
        if (AIController.CheckCurrentState(Define.EMonsterState.Dodge))
        {
            SpawnDamageText(Define.EDamageTextType.Dodge);
            return;
        }

        //회피
        float rand = Random.Range(0f, 100f);
        if (characterStats.evasionChancePer.Value > rand)
        {
            SpawnDamageText(Define.EDamageTextType.Evasion);
            return;
        }
        
        if (damageInfo.bIsCritical)
        {
            rand = Random.Range(0f, 100f);
            //크리 저항
            if (characterStats.criticalResistPer.Value > rand)
            {
                damage = Mathf.Round((damage / 2.5f) * 10) * 0.1f;
            }
        }

        float defence = characterStats.defence.Value;
        damage = Mathf.Clamp((damage * Random.Range(0.9f, 1.1f)) * (1 - (defence / (100 + defence))), 0f, float.MaxValue);
        damage = Mathf.Round((damage) * 10) * 0.1f;
        
        if (GI.Inst.ListenerManager.PlayersAttack()) //is defend?
        {
            damage -= damage * 0.7f; //대미지 70% 감소
            isDefend = true;
        }
      
        Character.HitEffect();
        AddCurrentHp(-damage);

        SkillManager instigatorSkillManager = instigatorStatManager.GetComponent<SkillManager>();
        
        if (instigatorSkillManager)
        {
            if (damageInfo.bIsCritical)
            {
                if (isDefend)
                    SpawnDamageText(Define.EDamageTextType.MonsterDefendDamagedCritical, damage);
                else
                    SpawnDamageText(Define.EDamageTextType.MonsterDamagedCritical, damage);
            }
            else
            {
                if (isDefend) 
                    SpawnDamageText(Define.EDamageTextType.MonsterDefendDamaged, damage);
                else
                    SpawnDamageText(Define.EDamageTextType.MonsterDamaged, damage);
            }
        }

        if (characterStats.currentHp.Value <= 0f)
        {
            Dead();
            if (instigatorSkillManager)
            {
                instigatorSkillManager.CauseDamageSuccessfully(damageType, ETakeDamageResult.Dead, this);
                return;
            }
        }

        if (instigatorSkillManager)
        {
            instigatorSkillManager.CauseDamageSuccessfully(damageType, ETakeDamageResult.TakeDamageOnly, this);
        }
    }
}
