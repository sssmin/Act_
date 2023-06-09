using System.Collections;
using UnityEngine;

public class SkillAbility_FireStrike : MonoBehaviour
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
        Animator = GetComponentInChildren<Animator>();
        AnimatorController = Animator.runtimeAnimatorController;
    }

    public void Init(Vector2 dir, BaseController inOwner, DamageInfo damageInfo)
    {
        OwnerController = inOwner;
        Rb.velocity = new Vector2(dir.x * speed, 0f);
        DamageInfo = damageInfo;
        CoDestroy = StartCoroutine(CoDestroyMyself());
        Animator.SetBool(AnimHash.activeFireStrike, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<StatManager>()?.OnDamage(DamageInfo, OwnerController.gameObject);
            DamageInfo temp = DamageInfo;
            temp.damage = DamageInfo.damage - Mathf.RoundToInt(DamageInfo.damage * 0.1f);
            temp.bIsCritical = DamageInfo.bIsCritical;
            DamageInfo = temp;
        }
        else //벽 같은데에 부딪히면 파괴
        {
            StopCoroutine(CoDestroy);
            GI.Inst.ResourceManager.Destroy(gameObject);
        }
    }

    IEnumerator CoDestroyMyself()
    {
        yield return new WaitForSeconds(2.5f);
        
        Rb.velocity = new Vector2(0f, 0f);

        GI.Inst.ResourceManager.Destroy(gameObject);
    }
    
}
