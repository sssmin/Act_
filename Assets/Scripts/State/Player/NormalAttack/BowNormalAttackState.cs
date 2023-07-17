using UnityEngine;

public class BowNormalAttackState : BaseNormalAttackState
{
    public BowNormalAttackState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        base.BeginState();
        Animator.SetBool(AnimHash.isBowNormalAttack, true);
    }

    public override void EndState()
    {
        base.EndState();
        Animator.SetBool(AnimHash.isBowNormalAttack, false);
    }
}
