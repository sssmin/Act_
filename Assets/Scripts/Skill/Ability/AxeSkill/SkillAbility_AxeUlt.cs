using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_AxeUlt : MonoBehaviour
{
    private PlayerController OwnerPlayerController { get; set; }
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
    

    public void Init(PlayerController playerController, DamageInfo damageInfo, List<Transform> monsters)
    {
        OwnerPlayerController = playerController;
        DamageInfo = damageInfo;
        
        foreach (Transform monster in monsters)
        {
            AIController aiController = monster.GetComponent<AIController>();
            StatManager statManager = monster.GetComponent<StatManager>();
            tempAIControllers.Add(aiController);
            tempMonsterStats.Add(statManager);
          
            aiController.SetDisableState(Define.EMonsterState.Freeze);
        }

        //StartCoroutine(CoExplosion());
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
            monsterStatManager.OnDamage(DamageInfo, OwnerPlayerController.gameObject);
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
