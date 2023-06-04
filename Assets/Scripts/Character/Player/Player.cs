using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Player : BaseCharacter
{
    public Dictionary<Define.EPlayerState, State> States = new Dictionary<Define.EPlayerState, State>();
    public PlayerController PlayerController { get; set; }
    public CombatManager CombatManager { get; private set; }
    public StatManager StatManager { get; private set; }
    [SerializeField] public Transform arrowSpawnPoint;
    
    public float JumpForce { get; private set; }
    public float GroundSlideSpeed { get; private set; }
    public float DashSpeed { get; set; }

    protected override void Awake()
    {
        base.Awake();
        
        PlayerController = GetComponent<PlayerController>();
        CombatManager = GetComponent<CombatManager>();
        StatManager = GetComponent<StatManager>();

        MoveSpeed = 5f;
        JumpForce = 5f;
        DashSpeed = 13f;
        GroundSlideSpeed = 13f;
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Init(States[Define.EPlayerState.Idle]);
        //GI.Inst.ListenerManager.onTransitionStateReq += TransitionStateNotify;
    }

    
    
    protected override void InitState()
    {
        States.Add(Define.EPlayerState.Idle, new Player_IdleState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.Move, new Player_MoveState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.InAir, new Player_InAirState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.Falling, new Player_FallingState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.JumpEnd, new Player_JumpEndState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.GroundSliding, new Player_GroundSlideState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.Dash, new Player_DashState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.WallSliding, new Player_WallSlideState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.WallJump, new Player_WallJumpState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.DaggerNormalAttack, new DaggerNormalAttackState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.AxeNormalAttack, new AxeNormalAttackState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.BowNormalAttack, new BowNormalAttackState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.ThrowDaggerSkill, new ThrowDaggerSkillState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.PlayerCloneSkill, new PlayerCloneSkillState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.DaggerUlt, new DaggerUltState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.DaggerBall, new DaggerBallSkillState(Animator, Rb, this, PlayerController));

        foreach (var pair in States)
        {
            ((PlayerState)pair.Value).playerStateType = pair.Key;
        }
    }

    public State GetState(Define.EPlayerState playerState)
    {
        if (States.ContainsKey(playerState))
        {
            return States[playerState];
        }
        return null;
    }

    private void TransitionStateNotify(int instanceId, Define.EPlayerState playerState)
    {
        if (InstId != instanceId) return;
        
        if (States.ContainsKey(playerState))
        {
            StateMachine.TransitionState(States[playerState]);
        }
    }
    
    public void TransitionState(Define.EPlayerState playerState)
    {
        if (States.ContainsKey(playerState))
        {
            StateMachine.TransitionState(States[playerState]);
        }
    }

    
    public override bool DoNotFlipState()
    {
        return StateMachine.CurrentState != States[Define.EPlayerState.Idle] && 
                StateMachine.CurrentState != States[Define.EPlayerState.InAir] && 
                StateMachine.CurrentState != States[Define.EPlayerState.JumpEnd] && 
                StateMachine.CurrentState != States[Define.EPlayerState.Falling] && 
               StateMachine.CurrentState != States[Define.EPlayerState.Move];

    }

    public void AnimPauseNotify()
    {
        StateMachine.CurrentState.AnimPauseNotify();
    }

   
    

    

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.green;
        // Gizmos.DrawLine(groundDetectObject.transform.position, new Vector3(groundDetectObject.transform.position.x, groundDetectDist * -1f));
    }
}
