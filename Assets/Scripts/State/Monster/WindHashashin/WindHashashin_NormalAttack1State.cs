using UnityEngine;

public class WindHashashin_NormalAttack1State : MonsterState
{
    public WindHashashin_NormalAttack1State(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isNormalAttack1, true);
        Monster.SetZeroVelocity();
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isNormalAttack1, false);
    }
}
