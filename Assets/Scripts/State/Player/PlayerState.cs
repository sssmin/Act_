using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    public PlayerState() { }
    public Define.EPlayerState playerStateType;
    protected bool bIsAnimTrigger;
    
    public PlayerState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController) : base(animator, rigidbody2D, character, baseController)
    {
        PlayerController = (PlayerController)baseController;
        Player = (Player)character;
        GI.Inst.ListenerManager.onTriggerAnim -= OnTriggerAnim;
        GI.Inst.ListenerManager.onTriggerAnim += OnTriggerAnim;
    }
    
    protected PlayerController PlayerController { get; set; }
    protected Player Player { get; set; }
    
    
    void Start()
    {
        
    }

    public override void Update()
    {
        Animator.SetFloat(AnimHash.yVelocity, Rb.velocity.y);
    }
    
    public void TransitionState(Define.EPlayerState playerState)
    {
        Player.TransitionState(playerState);
    }

    public void OnTriggerAnim(int instanceId)
    {
        if (Player.InstId == instanceId)
            bIsAnimTrigger = true;
    }
}
