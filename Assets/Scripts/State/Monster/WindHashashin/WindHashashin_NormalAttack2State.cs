using UnityEngine;

public class WindHashashin_NormalAttack2State : MonsterState
{
    public WindHashashin_NormalAttack2State(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
        aiControllerWindHashashin = (AIController_WindHashashin)AIController;
    }

    private AIController_WindHashashin aiControllerWindHashashin;
    
    public override void BeginState()
    {
        if (Monster.StatManager.IsDead) return;
        Animator.SetBool(AnimHash.isNormalAttack2, true);
        Monster.SetZeroVelocity();
        aiControllerWindHashashin.SetAttackTimer(Define.EBossAttackType.NormalAttack2);
    }
    
    public override void EndState()
    {
        Animator.SetBool(AnimHash.isNormalAttack2, false);
    }
}
