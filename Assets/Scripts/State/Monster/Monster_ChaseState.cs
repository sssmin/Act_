using UnityEngine;

public class Monster_ChaseState : MonsterState
{
    public Monster_ChaseState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }

    protected float randDistModifier;
    protected float idleTimer;

    
    public override void BeginState()
    {
        randDistModifier = Random.Range(-0.5f, 0.5f);
        Animator.SetBool(AnimHash.isMove, false);
        Animator.SetBool(AnimHash.isIdle, false);
    }

    public override void Update()
    {
        base.Update();
        
        idleTimer -= Time.deltaTime;

        if (Monster.StatManager.IsDead) return;
       
        if (AIController.Target.transform.position.x > Monster.transform.position.x) 
        {
            if (AIController.CurrentDir.x < 0) 
                AIController.TurnDir();
        }
        else
        {
            if (AIController.CurrentDir.x > 0) 
                AIController.TurnDir();
        }

        float dist = Vector3.Distance(AIController.Target.transform.position, Monster.transform.position);
        if (dist <= AIController.NormalAttackRange + randDistModifier)
        {
            Monster.SetZeroVelocity();
            Animator.SetBool(AnimHash.isMove, false);
            Animator.SetBool(AnimHash.isIdle, true);
            idleTimer = Random.Range(0.3f, 0.6f);
            if (AIController.CanNormalAttack)
                TransitionState(Define.EMonsterState.NormalAttack1);
        }
        else
        {
            Animator.SetBool(AnimHash.isIdle, true);
            if (idleTimer < 0f)
            {
                Animator.SetBool(AnimHash.isIdle, false);
                Animator.SetBool(AnimHash.isMove, true);
                Monster.SetVelocity(Monster.MoveSpeed * AIController.CurrentDir.x);
            }
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isMove, false);
        Animator.SetBool(AnimHash.isIdle, false);
    }
}