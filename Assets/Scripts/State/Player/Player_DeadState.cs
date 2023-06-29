using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }
    
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isDead, true);
        Player.SetZeroVelocity();
    }
    
    public override void Update()
    {
        base.Update();

        
    }
    
    public override void EndState()
    {
        
    }
    
    
}