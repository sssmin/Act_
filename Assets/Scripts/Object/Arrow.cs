using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BaseProjectile
{
    //여기서 충돌 하면 대미지 처리,
    //todo 일정 시간 후에 파괴
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public override void Init(Vector2 dir, BaseController inOwner, float inAttackCoefficient)
    {
        base.Init(dir, inOwner, inAttackCoefficient);

        StartCoroutine(CoDestroyMyself(3f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //todo other이 monster 면 피해 입히기
        StatManager enemyStatManager = other.GetComponent<StatManager>();
        PlayerController PC = OwnerController as PlayerController;
        PC.ControlledPlayer.StatManager.CauseNormalAttack(enemyStatManager, AttackCoefficient);
        
        GI.Inst.ResourceManager.Destroy(gameObject);
    }

    IEnumerator CoDestroyMyself(float second)
    {
        yield return new WaitForSeconds(second);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
