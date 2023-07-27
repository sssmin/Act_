using UnityEngine;

public class Monster_NormalAttackState : MonsterState
{
    public Monster_NormalAttackState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character,
        BaseController baseController)
        : base(animator, rigidbody2D, character, baseController)
    {
    }
    
    public override void BeginState()
    {
        if (Monster.StatManager.IsDead) return;
        Animator.SetBool(AnimHash.isNormalAttack1, true);
        Monster.SetZeroVelocity();
    }
    

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isNormalAttack1, false);
    }
}
