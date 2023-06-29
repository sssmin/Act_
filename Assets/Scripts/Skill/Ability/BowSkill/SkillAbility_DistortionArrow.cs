using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_DistortionArrow : MonoBehaviour
{
    private StatManager OwnerStatManager { get; set; }
    private Rigidbody2D Rb { get; set; }
    private Animator Animator { get; set; }
    private DamageInfo DamageInfo { get; set; }
    public float speed;
    private Coroutine CoStop;
    private CircleCollider2D AttractionCollider { get; set; }
    private CircleCollider2D StopCollider { get; set; }
    private SpriteRenderer Sr { get; set; }
    private ParticleSystem ParticleSystem { get; set; }
    public float attractionForce = 30f;
    public float attractionRange = 5f;
    private bool bIsActive;
    private List<Transform> Monsters { get; set; } = new List<Transform>();
    private float damageTimer;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Sr = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        AttractionCollider = GetComponent<CircleCollider2D>();
        StopCollider = GetComponentInChildren<CircleCollider2D>();
        ParticleSystem = GetComponent<ParticleSystem>();
    }
    

    public void Init(Vector2 dir, StatManager inOwner, DamageInfo damageInfo)
    {
        OwnerStatManager = inOwner;
        Rb.velocity = new Vector2(dir.x * speed, 0f);
        DamageInfo newDamageInfo = new DamageInfo();
        newDamageInfo.damage = Mathf.RoundToInt(damageInfo.damage / 5f);
        newDamageInfo.bIsCritical = damageInfo.bIsCritical;
        DamageInfo = newDamageInfo;

        Sr.enabled = true;
        StopCollider.enabled = true;
        AttractionCollider.enabled = false;
        Animator.SetBool(AnimHash.activeDistortionArrow, false);
        CoStop = StartCoroutine(CoStopMove());
        bIsActive = false;
        damageTimer = 0f;
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!bIsActive)
        {
            Rb.velocity = new Vector2(0f, 0f);
            Active();
        }
        
        if (col.CompareTag("Monster"))
        {
            if (CoStop != null) StopCoroutine(CoStop);
            if (!Monsters.Contains(col.transform))
            {
                AIController aiController = col.GetComponent<AIController>();
                aiController.SetDisableState(Define.EMonsterState.Suppression);
                Monsters.Add(col.transform);
            }
        }
    }
    
    IEnumerator CoStopMove()
    {
        yield return new WaitForSeconds(1f);
        Rb.velocity = new Vector2(0f, 0f);
        Active();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (bIsActive)
        {
            if (col.CompareTag("Monster"))
            {
                Vector3 dir = transform.position - col.transform.position;
                float dist = dir.magnitude;
           
                if (dist <= attractionRange)
                {
                    float forceMagnitude = attractionForce * (1f - dist / attractionRange);
                    Vector3 force = dir.normalized * forceMagnitude;
                    col.GetComponent<Rigidbody2D>()?.AddForce(force);
                }
                //여기서 지속 댐
                damageTimer += Time.deltaTime;
                if (damageTimer > 0.5f)
                {
                    damageTimer = 0f;
                    StatManager enemyStatManager = col.GetComponent<StatManager>();
                    enemyStatManager.TakeDamage(DamageInfo, OwnerStatManager, Define.EDamageType.Skill);
                }
            }
        }
    }

    void Active()
    {
        bIsActive = true;
        ParticleSystem.Play();
        Animator.SetBool(AnimHash.activeDistortionArrow, true);
        StopCollider.enabled = false;
        AttractionCollider.enabled = true;
        StartCoroutine(CoDestroyMyself());
    }

   
    
    IEnumerator CoDestroyMyself()
    {
        yield return new WaitForSeconds(2.5f);
        //todo 몬스터들 풀기
        foreach (Transform monster in Monsters)
        {
            AIController aiController = monster.GetComponent<AIController>();
            aiController.RevertState();
        }
        Monsters.Clear();
        bIsActive = false;
        AttractionCollider.enabled = false;
        Sr.enabled = false;
        ParticleSystem.Stop();
        Animator.SetBool(AnimHash.activeDistortionArrow, false);
        yield return new WaitForSeconds(5f);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }

    
    
    

    
    
}
