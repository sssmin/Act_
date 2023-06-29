using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_DaggerUlt : MonoBehaviour
{
    private PlayerController OwnerPlayerController { get; set; }
    private StatManager OwnerStatManager { get; set; }
    private DamageInfo DamageInfo { get; set; }
    private Animator Animator { get; set; }
    private SpriteRenderer Sr { get; set; }

    List<StatManager> tempMonsterStats = new List<StatManager>();
    List<AIController> tempAIControllers = new List<AIController>();
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Sr = GetComponent<SpriteRenderer>();
    }
    

    public void Init(PlayerController playerController, StatManager statManager, DamageInfo damageInfo, List<Transform> monsters)
    {
        OwnerPlayerController = playerController;
        OwnerStatManager = statManager;
        DamageInfo = damageInfo;
        
        foreach (Transform monster in monsters)
        {
            AIController aiController = monster.GetComponent<AIController>();
            StatManager monsterStatManager = monster.GetComponent<StatManager>();
            tempAIControllers.Add(aiController);
            tempMonsterStats.Add(monsterStatManager);
          
            aiController.SetDisableState(Define.EMonsterState.Freeze);
        }

        StartCoroutine(CoExplosion());
    }

    IEnumerator CoExplosion()
    {
        yield return new WaitForSeconds(2f);
        Animator.SetBool(AnimHash.isDaggerUltExplosion, true);
    }

    public void TakeDamageNotify()
    {
        Sr.color = Color.clear;
        StartCoroutine(CoTakeDamage());
    }

    IEnumerator CoTakeDamage()
    {
        yield return new WaitForSeconds(1.5f);
        foreach (StatManager monsterStatManager in tempMonsterStats)
        {
            monsterStatManager.TakeDamage(DamageInfo, OwnerStatManager, Define.EDamageType.Skill);
        }

        SkillEnd();
    }

    public void SkillEnd()
    {
        foreach (AIController aiController in tempAIControllers)
        {
            aiController.RevertState();
        }
     
        OwnerPlayerController.DaggerUltEnd();
        GI.Inst.ResourceManager.Destroy(gameObject);
       
    }
    
}
