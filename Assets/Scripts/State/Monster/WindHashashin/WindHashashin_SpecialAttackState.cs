using UnityEngine;

public class WindHashashin_SpecialAttackState : MonsterState
{
    public WindHashashin_SpecialAttackState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
        aiControllerWindHashashin = (AIController_WindHashashin)AIController;
    }

    private AIController_WindHashashin aiControllerWindHashashin;
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isSpecialAttack1, true);
        Monster.SetZeroVelocity();
        aiControllerWindHashashin.SetAttackTimer(Define.EBossAttackType.SpecialAttack1);
    }
    

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isSpecialAttack1, false);
    }
    
    
}
