using System.Collections;
using UnityEngine;

public class SkillAbility_PiercingArrow : MonoBehaviour
{
    private StatManager OwnerStatManager { get; set; }
    private Rigidbody2D Rb { get; set; }
    private Animator Animator { get; set; }
    private DamageInfo DamageInfo { get; set; }
    public float speed;
    private Coroutine CoDestroy;
    private BoxCollider2D BoxCollider { get; set; }
    private SpriteRenderer Sr { get; set; }
    private ParticleSystem ParticleSystem { get; set; }
    

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
        Sr = GetComponent<SpriteRenderer>();
        ParticleSystem = GetComponent<ParticleSystem>();
    }
    

    public void Init(Vector2 dir, StatManager inOwner, DamageInfo damageInfo)
    {
        OwnerStatManager = inOwner;
        Rb.velocity = new Vector2(dir.x * speed, 0f);
        DamageInfo = damageInfo;
        BoxCollider.enabled = true;
        Sr.enabled = true;
        ParticleSystem.Play();
        CoDestroy = StartCoroutine(CoDestroyMyself());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<StatManager>()?.TakeDamage(DamageInfo, OwnerStatManager, Define.EDamageType.Skill);
        }
        else
        {
            StopCoroutine(CoDestroy);
            StartCoroutine(CoDestroyStuck());
        }
    }
    
    IEnumerator CoDestroyStuck()
    {
        BoxCollider.enabled = false;
        Sr.enabled = false;
        ParticleSystem.Stop();
        yield return new WaitForSeconds(1f);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
    
    IEnumerator CoDestroyMyself()
    {
        yield return new WaitForSeconds(1f);
        BoxCollider.enabled = false;
        Sr.enabled = false;
        ParticleSystem.Stop();
        yield return new WaitForSeconds(1f);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }

    
    
    
}
