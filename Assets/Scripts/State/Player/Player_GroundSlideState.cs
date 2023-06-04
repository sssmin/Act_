using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GroundSlideState : PlayerState
{
    public Player_GroundSlideState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    private float groundSlideTimer;
    private float groundSlideDuration = .4f;
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.groundSlide, true);
        Player.SetVelocity(PlayerController.MoveDir.x * Player.GroundSlideSpeed);
        groundSlideTimer = groundSlideDuration;
    }
    
    public override void Update()
    {
        base.Update();

        groundSlideTimer -= Time.deltaTime;
        if (groundSlideTimer < 0f)
        {
            if (PlayerController.MoveDir.x == 0f) 
            {
                TransitionState(Define.EPlayerState.Idle);
            }
            else
            {
                TransitionState(Define.EPlayerState.Move);
            }
        }
    }
    
    public override void EndState()
    {
        Animator.SetBool(AnimHash.groundSlide, false);
        PlayerController.Flip(false);
    }
    
    
}
