using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_DaggerBall : MonoBehaviour
{
    private BaseController OwnerController { get; set; }
    private Rigidbody2D Rb { get; set; }
    private Animator Animator { get; set; }
    private RuntimeAnimatorController AnimatorController { get; set; }
    private int Damage { get; set; }
    private bool IsCritical { get; set; }
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
        if (OwnerController.CompareTag(other.tag)) return; //자기자신이면 return
        
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<StatManager>()?.OnDamage(DamageInfo, OwnerController.gameObject);
           
        }
        else //벽 같은데에 부딪히면 파괴
        {
            GI.Inst.ResourceManager.Destroy(gameObject);
        }
    }

    IEnumerator CoDestroyMyself()
    {
        yield return new WaitForSeconds(.7f);
        
        Rb.velocity = new Vector2(0f, 0f);
        Animator.SetTrigger(AnimHash.daggerExplosionTrigger);
        
        float animationEndTime = 0f;
        foreach (var clip in AnimatorController.animationClips)
        {
            if (clip.name == "Dagger_CircleExplosion")
            {
                animationEndTime = clip.length;
                break;
            }
        }
        yield return new WaitForSeconds(animationEndTime);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
    
}
