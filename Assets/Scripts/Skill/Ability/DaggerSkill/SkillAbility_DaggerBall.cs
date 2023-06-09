using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_DaggerBall : MonoBehaviour
{
    private BaseController OwnerController { get; set; }
    private Rigidbody2D Rb { get; set; }
    private Animator Animator { get; set; }
    private RuntimeAnimatorController AnimatorController { get; set; }
    private DamageInfo DamageInfo { get; set; }
    public float speed;
    private Coroutine CoDestroy;
  

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        AnimatorController = Animator.runtimeAnimatorController;
    }

    public void Init(Vector2 dir, BaseController inOwner, DamageInfo damageInfo)
    {
        OwnerController = inOwner;
        Rb.velocity = new Vector2(dir.x * speed, 0f);
        DamageInfo = damageInfo;
        CoDestroy = StartCoroutine(CoDestroyMyself());
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        Animator.SetTrigger(AnimHash.daggerBallExplosionTrg);
        Rb.velocity = new Vector2(0f, 0f);
        //여기서 범위 Overlap 몬스터
        Collider2D[] collider2Ds = new Collider2D[10];
        Physics2D.OverlapCircleNonAlloc(gameObject.transform.position, 2f, collider2Ds, LayerMask.GetMask("Monster"));
        foreach (Collider2D col in collider2Ds)
        {
            if (col == null) break;
            StatManager monsterStat = col.GetComponent<StatManager>();
            monsterStat.OnDamage(DamageInfo, OwnerController.gameObject);
        }

        StartCoroutine(CoRestore());
    }

    //시간 지나는건 소멸
    IEnumerator CoDestroyMyself()
    {
        yield return new WaitForSeconds(.7f);
        
        Rb.velocity = new Vector2(0f, 0f);
        Animator.SetTrigger(AnimHash.daggerBallDestroyTrg);
        
        float animationEndTime = 0f;
        foreach (var clip in AnimatorController.animationClips)
        {
            if (clip.name == "DaggerBall_Destroy")
            {
                animationEndTime = clip.length;
                break;
            }
        }
        yield return new WaitForSeconds(animationEndTime);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }

    IEnumerator CoRestore()
    {
        yield return new WaitForSeconds(.7f);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
