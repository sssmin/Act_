
using UnityEngine;

public class WandererMagician_ChaseState : Monster_ChaseState
{
    public WandererMagician_ChaseState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }

    private float projectileRange = 4f;

    
    public override void BeginState()
    {
        randDistModifier = Random.Range(-0.5f, 0.5f);
    }

    public override void Update()
    {
        idleTimer -= Time.deltaTime;
       
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
        
            //3               2.5
        if (dist <= AIController.NormalAttackRange + randDistModifier)
        {
            Monster.SetZeroVelocity();
            Animator.SetBool(AnimHash.isRun, false);
            Animator.SetBool(AnimHash.isIdle, true);
            idleTimer = Random.Range(0.3f, 0.6f);
            if (AIController.CanNormalAttack)
            {
                TransitionState(Define.EMonsterState.NormalAttack1);
            }
        } //3               4
        else if (dist <= projectileRange + randDistModifier)
        {
            Monster.SetZeroVelocity();
            Animator.SetBool(AnimHash.isRun, false);
            Animator.SetBool(AnimHash.isIdle, true);
            idleTimer = Random.Range(0.3f, 0.6f);
            if (AIController.CanNormalAttack)
            {
                TransitionState(Define.EMonsterState.NormalAttack2);
            }
        }
        else
        {
            if (idleTimer < 0f)
            {
                Animator.SetBool(AnimHash.isIdle, false);
                Animator.SetBool(AnimHash.isRun, true);
                Monster.SetVelocity(Monster.MoveSpeed * AIController.CurrentDir.x);
            }
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isRun, false);
        Animator.SetBool(AnimHash.isIdle, false);
    }
}
