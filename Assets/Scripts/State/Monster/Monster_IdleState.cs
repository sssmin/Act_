using UnityEngine;

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
        
        if (Monster.StatManager.IsDead) return;
        Animator.SetBool(AnimHash.isIdle, true);
        if (AIController.Target != null)
        {
            Monster.TransitionState(Define.EMonsterState.Chase);
        }
        else
        {
            idleTimer = Random.Range(2f, 3.5f);
        }
    }

    public override void Update()
    {
        idleTimer -= Time.deltaTime;

        if (AIController.Target != null)
        {
            if (idleTimer < 1f)
            {
                TransitionState(Define.EMonsterState.Chase);
                return;    
            }
        }
        
        if (idleTimer < 0f)
        {
            Monster.TransitionState(Define.EMonsterState.Patrol);
        }
        
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isIdle, false);
        idleTimer = 10f;
    }
}