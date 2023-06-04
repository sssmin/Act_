using System;
using System.Collections;
using UnityEngine;

public class SkillAbility_ThrowDagger : MonoBehaviour
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
    private int maxPenetrationNum; //최대 관통 수
    private int prenetrationCount;

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
        prenetrationCount = 0;
        maxPenetrationNum = 5; //todo 이것도 아이템 효과로 늘어날수도
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (OwnerController.CompareTag(other.tag)) return; //자기자신이면 return
        
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<StatManager>()?.OnDamage(DamageInfo, OwnerController.gameObject);
            prenetrationCount++;
            if (prenetrationCount > maxPenetrationNum) //최대 5명 관통가능이라면 6명째에서 파괴.
            {
                GI.Inst.ResourceManager.Destroy(gameObject);
            }
        }
        else //벽 같은데에 부딪히면 파괴
        {
            GI.Inst.ResourceManager.Destroy(gameObject);
        }
    }

    IEnumerator CoDestroyMyself()
    {
        yield return new WaitForSeconds(.7f);
        //todo 여기서 만약 파괴할 때 폭발하는 아이템 효과가 있으면
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
