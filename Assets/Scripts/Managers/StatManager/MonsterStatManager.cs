using UnityEngine;


public class MonsterStatManager : StatManager
{
    private UI_MonsterInfo MonsterInfoUI { get; set; }
    protected AIController AIController { get; set; }

    public override void Awake()
    {
        base.Awake();

        AIController = GetComponent<AIController>();
    }

    public override void Start()
    {
        float spawnXPos = GetComponentInChildren<CapsuleCollider2D>().size.x / 2f;
        float spawnYPos = GetComponentInChildren<CapsuleCollider2D>().size.y + 0.5f;
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Main_MonsterInfo", transform);
        MonsterInfoUI = go.GetComponent<UI_MonsterInfo>();
        MonsterInfoUI.InitPos(spawnXPos, spawnYPos);
    }

    public override void Update()
    {
        if (DurationEffectEndTimePq.GetCount() > 0)
        {
            DurationEffect durationEffect = DurationEffectEndTimePq.Peek();
            if (durationEffect.durationEndTime <= Time.time)//지속시간 끝
            {
                durationEffect = DurationEffectEndTimePq.Pop();
                
                if (DurationEffectDurationDict.ContainsKey(durationEffect.durationEffectId))
                    DurationEffectDurationDict.Remove(durationEffect.durationEffectId);
                
                durationEffect.effectInfo.onExecuteDecreaseStat?.Invoke();
            }
        }
    }

    protected override void AddCurrentHp(float value)
    {
        base.AddCurrentHp(value);
        
        float ratio = Mathf.Round((characterStats.currentHp.Value / characterStats.maxHp.Value * 100f) * 10) * 0.1f; 
        MonsterInfoUI.SetBar(ratio);
    }

    public void FlipMonsterInfoUI(BaseController.EDir dir)
    {
        MonsterInfoUI.Flip(dir);
    }

    public override void TakeDamage(DamageInfo damageInfo, StatManager instigatorStatManager, Define.EDamageType damageType)
    {
        if (IsDead) return;
        float damage = damageInfo.damage;
        
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

        float defence = characterStats.defence.Value + characterStats.defenceIncValue.Value;
        damage = Mathf.Clamp((damage * Random.Range(0.9f, 1.1f)) * (1 - (defence / (100 + defence))), 0f, float.MaxValue);
        damage = Mathf.Round((damage) * 10) * 0.1f;
      
        Character.HitEffect();
        AddCurrentHp(-damage);
        
        SkillManager instigatorSkillManager = instigatorStatManager.GetComponent<SkillManager>();
        
        if (instigatorSkillManager)
        {
            if (damageInfo.bIsCritical)
            {
                SpawnDamageText(Define.EDamageTextType.MonsterDamagedCritical, damage);
            }
            else
            {
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

    protected override void Dead()
    {
        if (IsDead) return;
        
        IsDead = true;
        
        Monster monster = GetComponent<Monster>();
        if (monster)
        {
            monster.TransitionState(Define.EMonsterState.Dead);
            if (monster.DropTable != null)
                monster.DropTable.DropItem();
            else
                Debug.Log("드랍 테이블 없음");
        }

        GI.Inst.DungeonManager.IncreaseKillCount();
    }

    public override void ExecDurationEffect(Effect effect, Sprite icon)
    {
        DurationEffect newDurationEffect = effect as DurationEffect;
        
        if (newDurationEffect != null)
        {
            EDurationEffectId effectId = newDurationEffect.durationEffectId;
            //이미 있는지 확인,
            if (DurationEffectDurationDict.ContainsKey(effectId))
            {
                //이미 있으면 Reset인지, 누적인지 확인
                if (newDurationEffect.bIsResetDuration) //Reset이면 duration을 리셋 => endTime을 새로운거로 바꿔준다.
                {
                    DurationEffectDurationDict[effectId] = newDurationEffect.duration;
                    
                    DurationEffectEndTimePq.Peek().effectInfo.onExecuteDecreaseStat?.Invoke();
                    DurationEffectEndTimePq.Delete(item => item.durationEffectId == newDurationEffect.durationEffectId);

                    DurationEffectEndTimePq.Push(newDurationEffect);
                    newDurationEffect.effectInfo.onExecuteIncreaseStat?.Invoke();
                }
                else //누적이면 기존 duration에 새로운 duration 더하기 => 기존 endTime에 duration 더하기
                {
                    DurationEffect oldEffect = DurationEffectEndTimePq.FindPop(item =>
                        item.durationEffectId == effectId);
                    oldEffect.effectInfo.onExecuteDecreaseStat?.Invoke();
                    
                    float leftDuration = oldEffect.durationEndTime - Time.time; //남은 지속 시간
                    DurationEffectDurationDict[effectId] = leftDuration + newDurationEffect.duration;
                    
                    oldEffect.durationEndTime += newDurationEffect.duration;
                    newDurationEffect.durationEndTime = oldEffect.durationEndTime; //갱신된 콜백 호출위해 end time만 덮어쓰기.
                    
                    DurationEffectEndTimePq.Push(newDurationEffect);
                    newDurationEffect.effectInfo.onExecuteIncreaseStat?.Invoke();
                    
                }
            }
            else  //없으면 새롭게 추가
            {
                DurationEffectDurationDict.Add(effectId, newDurationEffect.duration);
                
                DurationEffectEndTimePq.Push(newDurationEffect);
                newDurationEffect.effectInfo.onExecuteIncreaseStat?.Invoke();
            }
            MonsterInfoUI.SetStatusImage(icon, newDurationEffect.durationEndTime);
        }
    }
    
    public void InitStat(Stats inStat, int level)
    {
        characterStats.InitMonsterStat(inStat, level);
    }
    
    public override void TakeTrapDamage()
    {
    }
}
