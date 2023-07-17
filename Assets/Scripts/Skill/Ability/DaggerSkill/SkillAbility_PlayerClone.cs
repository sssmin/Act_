using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillAbility_PlayerClone : MonoBehaviour
{
    private Animator Animator { get; set; }
    private SpriteRenderer Sr { get; set; }
    private StatManager OwnerStatManager { get; set; }
    private StatManager EnemyStatManager { get; set; }
    [SerializeField] private GameObject normalAttackOverlap;
    private DamageInfo DamageInfo { get; set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Sr = GetComponent<SpriteRenderer>();
    }
    
    public void Init(StatManager ownerStatManager, DamageInfo damageInfo, StatManager enemyStatManager)
    {
        Sr.color = Color.white;
        OwnerStatManager = ownerStatManager;
        EnemyStatManager = enemyStatManager;
        DamageInfo = damageInfo;
       
        if (Random.value > 0.5f)
            Sr.sortingOrder += 1;
        else
            Sr.sortingOrder -= 1;
        
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            Animator.SetBool(AnimHash.isDaggerNormalAttack, true);
            Animator.SetInteger(AnimHash.attackComboNum, 0);
        }
        else if (rand == 1)
        {
            Animator.SetBool(AnimHash.isDaggerNormalAttack, true);
            Animator.SetInteger(AnimHash.attackComboNum, 1);
        }
        else if (rand == 2)
        {
            Animator.SetBool(AnimHash.isDaggerNormalAttack, true);
            Animator.SetInteger(AnimHash.attackComboNum, 2);
        }
    }
    
    public void ExecuteCloneAttackNotify()
    {
        EnemyStatManager.TakeDamage(DamageInfo, OwnerStatManager, Define.EDamageType.Skill);
    }

    public void DestroyCloneNotify()
    {
        OwnerStatManager = null;
        EnemyStatManager = null;
        
        StartCoroutine(FadeOutClone());
    }

    IEnumerator FadeOutClone()
    {
        while (true)
        {
            Sr.color = new Color(1, 1, 1, Sr.color.a - (Time.deltaTime * 1f));
            yield return null;
            if (Sr.color.a <= 0)
                break;
        }
        
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
