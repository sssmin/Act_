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
    
    public override void Init(Vector2 dir, BaseController inOwner, int damage)
    {
        base.Init(dir, inOwner, damage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //todo other이 monster 면 피해 입히기
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
