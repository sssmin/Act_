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

    public override void AddCurrentHp(float value)
    {
        base.AddCurrentHp(value);
        
        float ratio = Mathf.Round((stats.currentHp.Value / stats.maxHp.Value * 100f) * 10) * 0.1f; 
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
        if (stats.evasionChancePer.Value > rand)
        {
            SpawnDamageText(Define.EDamageTextType.Evasion);
            return;
        }
       
        
        if (damageInfo.bIsCritical)
        {
            rand = Random.Range(0f, 100f);
            //크리 저항
            if (stats.criticalResistPer.Value > rand)
            {
                damage = Mathf.Round((damage / 2.5f) * 10) * 0.1f;
            }
        }

        float defence = stats.defence.Value;
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


        if (stats.currentHp.Value <= 0f)
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
