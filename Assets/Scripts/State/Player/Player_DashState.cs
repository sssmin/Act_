using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DashState : PlayerState
{
    public Player_DashState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
        : base(animator, rigidbody2D, character, baseController) { }

    private float dashTimer;
    private float DashDir { get; set; }
    
    public override void BeginState()
    {
        Animator.SetBool(AnimHash.isDash, true);
        DashDir = PlayerController.CurrentDir.x;
        dashTimer = 0.3f;
    }
    
    public override void Update()
    {
        base.Update();

        dashTimer -= Time.deltaTime;
        if (PlayerController.IsWallDetect())
        {
            TransitionState(Define.EPlayerState.WallSliding);
            return;
        }
        
        Player.SetVelocity(Player.DashSpeed * DashDir, 0);

        if (dashTimer < 0f)
        {
            TransitionState(Define.EPlayerState.Idle);
        }
    }

    public override void EndState()
    {
        Animator.SetBool(AnimHash.isDash, false);
    }
}
