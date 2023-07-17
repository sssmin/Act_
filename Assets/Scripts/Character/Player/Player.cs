using System.Collections.Generic;
using UnityEngine;


public class Player : BaseCharacter
{
    public Dictionary<Define.EPlayerState, State> States = new Dictionary<Define.EPlayerState, State>();
    public PlayerController PlayerController { get; set; }
    public CombatManager CombatManager { get; private set; }
    public InventoryManager InventoryManager { get; private set; }
    
    public CapsuleCollider2D CapsuleCollider { get; private set; }
    [SerializeField] public Transform arrowSpawnPoint;
    
    public float JumpForce { get; private set; }
    public float GroundSlideSpeed { get; private set; }
    public float DashSpeed { get; set; }

    protected override void Awake()
    {
        base.Awake();
        
        PlayerController = GetComponent<PlayerController>();
        CombatManager = GetComponent<CombatManager>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        InventoryManager = GetComponent<InventoryManager>();
        StatManager = GetComponent<PlayerStatManager>();

        JumpForce = 5f;
        GroundSlideSpeed = 13f;
        DashSpeed = 13f;
        StatManager.InstId = InstId;
        
        GI.Inst.ListenerManager.getPlayerInstId -= GetPlayerInstId;
        GI.Inst.ListenerManager.getPlayerInstId += GetPlayerInstId;
    }

    private void OnDestroy()
    {
        GI.Inst.ListenerManager.getPlayerInstId -= GetPlayerInstId;
    }
    
    protected override void Start()
    {
        base.Start();
        
        StateMachine.Init(States[Define.EPlayerState.Idle]);
    }

    public void SetBaseStat()
    {
        PlayerBaseStats playerBaseStats =  GI.Inst.ResourceManager.PlayerBaseStats;
        StatManager.InitStat(playerBaseStats.stats);
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
        States.Add(Define.EPlayerState.Dead, new Player_DeadState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.WallSliding, new Player_WallSlideState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.WallJump, new Player_WallJumpState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.DaggerNormalAttack, new DaggerNormalAttackState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.AxeNormalAttack, new AxeNormalAttackState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.BowNormalAttack, new BowNormalAttackState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.ThrowDaggerSkill, new ThrowDaggerSkillState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.PlayerCloneSkill, new PlayerCloneSkillState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.DaggerUlt, new DaggerUltState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.DaggerBall, new DaggerBallSkillState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.FireStrike, new FireStrikeState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.Earthquake, new EarthquakeState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.ThrowAxe, new ThrowAxeSkillState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.AxeUlt, new AxeUltState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.ArrowRain, new ArrowRainState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.PiercingArrow, new PiercingArrowState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.ArrowBuff, new ArrowBuffState(Animator, Rb, this, PlayerController));
        States.Add(Define.EPlayerState.DistortionArrow, new DistortionArrowState(Animator, Rb, this, PlayerController));

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

    public int GetPlayerInstId()
    {
        return InstId;
    }
    

    

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.green;
        // Gizmos.DrawLine(groundDetectObject.transform.position, new Vector3(groundDetectObject.transform.position.x, groundDetectDist * -1f));
    }
}
