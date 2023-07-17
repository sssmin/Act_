using System.Collections;
using UnityEngine;

public class SkillAbility_ThrowDagger : MonoBehaviour
{
    private StatManager OwnerStatManager { get; set; }
    private Rigidbody2D Rb { get; set; }
    private Animator Animator { get; set; }
    private RuntimeAnimatorController AnimatorController { get; set; }
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

    public void Init(Vector2 dir, StatManager inOwner, DamageInfo damageInfo)
    {
        OwnerStatManager = inOwner;
        Rb.velocity = new Vector2(dir.x * speed, 0f);
        DamageInfo = damageInfo;
        CoDestroy = StartCoroutine(CoDestroyMyself());
        prenetrationCount = 0;
        maxPenetrationNum = 5;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<StatManager>()?.TakeDamage(DamageInfo, OwnerStatManager, Define.EDamageType.Skill);
            prenetrationCount++;
            if (prenetrationCount > maxPenetrationNum) //최대 5명 관통가능이라면 6명째에서 파괴.
            {
                StopCoroutine(CoDestroy);
                GI.Inst.ResourceManager.Destroy(gameObject);
            }
        }
        else //벽 같은데에 부딪히면 파괴
        {
            StopCoroutine(CoDestroy);
            GI.Inst.ResourceManager.Destroy(gameObject);
        }
    }

    IEnumerator CoDestroyMyself()
    {
        yield return new WaitForSeconds(.7f);
        
        Rb.velocity = new Vector2(0f, 0f);
        Animator.SetTrigger(AnimHash.daggerExplosionTrg);
        
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
