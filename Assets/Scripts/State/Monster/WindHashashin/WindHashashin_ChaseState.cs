using UnityEngine;

public class WindHashashin_ChaseState : Monster_ChaseState
{
    public WindHashashin_ChaseState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
        aiControllerWindHashashin = (AIController_WindHashashin)AIController;
    }

    private AIController_WindHashashin aiControllerWindHashashin;
    private float projectileRange = 4f;
    
    
    public override void BeginState()
    {
        randDistModifier = Random.Range(-0.5f, 0.5f);
        Animator.SetBool(AnimHash.isMove, false);
        Animator.SetBool(AnimHash.isIdle, false);
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

        if (aiControllerWindHashashin.IsAttackReady(Define.EBossAttackType.SpecialAttack1))
        {
            TransitionState(Define.EMonsterState.SpecialAttack1);
            return;
        }
        

        float dist = Vector3.Distance(AIController.Target.transform.position, Monster.transform.position);

        if (dist > projectileRange + randDistModifier)
        {
            Animator.SetBool(AnimHash.isIdle, false);
            Animator.SetBool(AnimHash.isMove, true);
            Monster.SetVelocity(Monster.MoveSpeed * AIController.CurrentDir.x);
            return;
        }

       
        if (dist <= AIController.NormalAttackRange + randDistModifier)
        {
            Monster.SetZeroVelocity();
            Animator.SetBool(AnimHash.isMove, false);
            Animator.SetBool(AnimHash.isIdle, true);
            idleTimer = Random.Range(0.3f, 0.6f);
            if (aiControllerWindHashashin.IsAttackReady(Define.EBossAttackType.NormalAttack3))
            {
                TransitionState(Define.EMonsterState.NormalAttack3);
            }
            else
            {
                TransitionState(Define.EMonsterState.NormalAttack1);
            }
        } 
        else if (dist <= projectileRange + randDistModifier)
        {
            Monster.SetZeroVelocity();
            Animator.SetBool(AnimHash.isMove, false);
            Animator.SetBool(AnimHash.isIdle, true);
            idleTimer = Random.Range(0.3f, 0.6f);
            if (aiControllerWindHashashin.IsAttackReady(Define.EBossAttackType.NormalAttack2))
            {
                TransitionState(Define.EMonsterState.NormalAttack2);
            }
            else
            {
                TransitionState(Define.EMonsterState.NormalAttack1);
            }
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isMove, false);
        Animator.SetBool(AnimHash.isIdle, false);
    }
}
