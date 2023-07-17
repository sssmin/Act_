using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_Earthquake : MonoBehaviour
{
    private StatManager OwnerStatManager { get; set; }
    private Rigidbody2D Rb { get; set; }
    private Animator Animator { get; set; }
    private DamageInfo DamageInfo { get; set; }
    private BoxCollider2D BoxCollider { get; set; }
    public SpriteRenderer Sr { get; set; }

    private Dictionary<int, StatManager> enemyStats { get; set; } = new Dictionary<int, StatManager>();
 

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
        Sr = GetComponent<SpriteRenderer>();
    }

    public void Init(StatManager inOwner, DamageInfo damageInfo)
    {
        OwnerStatManager = inOwner;
        DamageInfo = damageInfo;
        Animator.SetBool(AnimHash.activeEarthquake, true);
        BoxCollider.enabled = false;
        enemyStats.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int instId = other.GetInstanceID();
        if (other.CompareTag("Monster") && !enemyStats.ContainsKey(instId))
        {
            StatManager enemyStat = other.GetComponent<StatManager>();
            enemyStats.Add(instId, enemyStat);
            enemyStat.TakeDamage(DamageInfo, OwnerStatManager, Define.EDamageType.Skill);
        }
    }
    
    public void Activate()
    {
        BoxCollider.enabled = true;
    }

    public void Deactivate()
    {
        BoxCollider.enabled = false;
    }

    public void DestroyNotify()
    {
        GI.Inst.ResourceManager.Destroy(gameObject);
        enemyStats.Clear();
    }

}
