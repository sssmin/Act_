using UnityEngine;

public class WindHashashin_DefendState : MonsterState
{
    public WindHashashin_DefendState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }
    
    
    public override void BeginState()
    {
        if (Monster.StatManager.IsDead) return;
        Animator.SetBool(AnimHash.isDefend, true);
        Monster.SetZeroVelocity();
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isDefend, false);
    }
}
