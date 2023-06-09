﻿using UnityEngine;

public class Monster_IdleState : MonsterState
{
    public Monster_IdleState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    //Idle 상태에 들어오면 타겟이 있는지 확인 하고 타겟이 없으면 순찰
    //타겟이 있으면 타겟 쪽으로 이동
    //싸우다가 중간에 Idle로 들어올 수 있기 때문임.
    private float idleTimer;
    
    public override void BeginState()
    {
        Monster.SetZeroVelocity();
        
        if (AIController.Target != null)
        {
            Monster.TransitionState(Define.EMonsterState.Chase);
        }
        else
        {
            Animator.SetBool(AnimHash.isIdle, true);
            //todo 몇초 후에 Patol로 변환 
            idleTimer = Random.Range(2f, 3.5f);
        }
        // Animator.SetFloat(Define.xVelocity, PlayerController.MoveDir.x);
        // 
    }

    public override void Update()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer < 0f)
        {
            Monster.TransitionState(Define.EMonsterState.Patrol);
        }
        
        //todo idle일때도 내 앞 뒤에 적이 있는지 
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isIdle, true);
        idleTimer = 10f;
    }
}