using UnityEngine;

public class Monster_PatrolState : MonsterState
{
    public Monster_PatrolState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }

    protected float ThinkTimer;

    public override void BeginState()
    {
        if (Monster.StatManager.IsDead) return;
        Animator.SetBool(AnimHash.isMove, true);
        ThinkTimer = Random.Range(1f, 2f);
    }

    public override void Update()
    {
        base.Update();
        
        ThinkTimer -= Time.deltaTime;
        if (Monster.StatManager.IsDead) return;
        if (AIController.Target != null)
        {
            TransitionState(Define.EMonsterState.Chase);
            return;
        }
        
        Monster.SetVelocity(Monster.MoveSpeed * AIController.CurrentDir.x);
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
        Animator.SetBool(AnimHash.isMove, false);
    }
}