using UnityEngine;

public class Monster_DeadState : MonsterState
{
    public Monster_DeadState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isDead, true);
        Monster.SetZeroVelocity();
    }
    
    public override void Update()
    {
        base.Update();

        
    }
    
    public override void EndState()
    {
        
    }
    
    
}