using System;
using System.Collections;
using UnityEngine;

public class SkillAbility_ArrowRain : MonoBehaviour
{
    private StatManager OwnerStatManager { get; set; }
    private Rigidbody2D Rb { get; set; }
    private Animator Animator { get; set; }
    private DamageInfo DamageInfo { get; set; }
    private Coroutine CoDestroy;
    private float damageTimer;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }
    

    public void Init(Vector2 dir, StatManager inOwner, DamageInfo damageInfo)
    {
        OwnerStatManager = inOwner;
        DamageInfo newDamageInfo = new DamageInfo();
        newDamageInfo.damage = Mathf.RoundToInt(damageInfo.damage / 9f);
        DamageInfo = newDamageInfo;
        damageTimer = 0.3f;
        StartCoroutine(CoDestroyMyself());
    }

    //대미지 3초인데 9번 대미지
    //0.3초에 1번 대미지/
    

    private void OnTriggerStay2D(Collider2D other)
    {
        damageTimer -= Time.deltaTime;
        if (damageTimer < 0f)
        {
            other.GetComponent<StatManager>()?.TakeDamage(DamageInfo, OwnerStatManager, Define.EDamageType.Skill);
            damageTimer = 0.3f;
        }
    }

    IEnumerator CoDestroyMyself()
    {
        yield return new WaitForSeconds(3.5f);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
    
    

    
    
}
