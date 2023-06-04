using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbility_DaggerUlt : MonoBehaviour
{
    //스폰 되면 주변 몬스터 움직임 멈추고
    //어느정도 있다가
    //폭발 애니메이션
    //그 움직임 멈춘 몬스터들 대미지 주고. 화면에 보이는 모든 적이였으면 좋겠는데
    //플레이어 daggerUltNum 2로 해줘야함.
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
          
            aiController.FreezeMonster();
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
            monsterStatManager.OnDamage(DamageInfo, OwnerPlayerController.gameObject);
        }

        SkillEnd();
    }

    public void SkillEnd()
    {
        foreach (AIController aiController in tempAIControllers)
        {
            aiController.UnfreezeMonster();
        }
     
        OwnerPlayerController.DaggerUltEnd();
        GI.Inst.ResourceManager.Destroy(gameObject);
       
    }
    
}
