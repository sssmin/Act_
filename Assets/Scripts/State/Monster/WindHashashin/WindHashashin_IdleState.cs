using UnityEngine;

public class WindHashashin_IdleState : MonsterState
{
    public WindHashashin_IdleState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }
    
    private float idleTimer;
    
    public override void BeginState()
    {
        Monster.SetZeroVelocity();
        if (Monster.StatManager.IsDead) return;
        
        Animator.SetBool(AnimHash.isIdle, true);
        idleTimer = Random.Range(0.5f, 1f);
    }

    public override void Update()
    {
        idleTimer -= Time.deltaTime;
        if (AIController.Target != null)
        {
            if (idleTimer < 0f)
            {
                TransitionState(Define.EMonsterState.Chase);
            }
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isIdle, false);
    }
}
