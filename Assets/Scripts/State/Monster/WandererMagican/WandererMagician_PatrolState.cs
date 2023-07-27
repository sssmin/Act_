using UnityEngine;

public class WandererMagician_PatrolState : Monster_PatrolState
{
    public WandererMagician_PatrolState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }


    public override void BeginState()
    {
        if (Monster.StatManager.IsDead) return;
        Animator.SetBool(AnimHash.isWalk, true);
        ThinkTimer = Random.Range(1f, 2f);
    }

    public override void Update()
    {
        ThinkTimer -= Time.deltaTime;
        if (Monster.StatManager.IsDead) return;
        if (AIController.Target != null)
        {
            TransitionState(Define.EMonsterState.Chase);
            return;
        }
        
        Monster.SetVelocity((Monster.MoveSpeed / 2) * AIController.CurrentDir.x);
        //Idle로 전환, 뒤돌기
        if (AIController.IsFrontWall || !AIController.IsFrontGround)
        {
            AIController.TurnDir();
            TransitionState(Define.EMonsterState.Idle);
            return;
        }
        //1초마다 뒤로 돌지 생각 
        if (ThinkTimer < 0f)
        {
            if (Random.Range(0, 100) < 30)
            {
                AIController.TurnDir();
                TransitionState(Define.EMonsterState.Idle);
                return;
            }
            ThinkTimer = Random.Range(1f, 2f);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isWalk, false);
    }
}
