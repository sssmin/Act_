using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public enum EDir
{
    Right,
    Left
}

public class PlayerController : BaseController
{
    private PlayerControl PlayerControl { get; set; }
    private InputAction MoveInputValue { get; set; }
    private bool bNormalAttackReserve;
    public bool IsReserveNormalAttack
    {
        get => bNormalAttackReserve;
        private set => bNormalAttackReserve = value;
    }
    private float normalAttackReserveDuration;
    private float normalAttackReserveTimer;
    public Player ControlledPlayer { get; private set; }
    
    public Vector2 AttackDir { get; private set; }

    private Vector2 moveDir;
    public Vector2 MoveDir
    {
        get => moveDir;
        private set
        {
            moveDir = value;
         
            if (moveDir.x < 0f)
            {
                CurrentDir = moveDir;
            }
            else if (moveDir.x > 0f)
            {
                CurrentDir = moveDir;
            }
        }
    }

    public override Vector2 CurrentDir
    {
        protected set
        {
            base.CurrentDir = value;
            if (CurrentDir.x < 0f)
            {
                if (!ControlledPlayer.DoNotFlipState())
                {
                    Flip(true);
                }
                if (CheckStateCanMove() && !IsWallDetect())
                {
                    TransitionMoveState();
                }
            }
            else if (CurrentDir.x > 0f)
            {
                if (!ControlledPlayer.DoNotFlipState())
                {
                    Flip(false);
                }
                
                if (CheckStateCanMove() && !IsWallDetect())
                {
                    TransitionMoveState();
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        ControlledPlayer = GetComponent<Player>();
        PlayerControl = new PlayerControl();
        normalAttackReserveDuration = 0.3f;
        WallDetectDist = 0.3f;
    }
    
    private void OnEnable()
    {
        PlayerControl.Enable();
        MoveInputValue = PlayerControl.Player.Move;
        PlayerControl.Player.Jump.performed += Jump;
        PlayerControl.Player.Dash.performed += Dash;
        PlayerControl.Player.NormalAttack.performed += NormalAttack;
        PlayerControl.Player.QSkill.performed += QSkill;
        PlayerControl.Player.WSkill.performed += WSkill;
        PlayerControl.Player.ESkill.performed += ESkill;
        PlayerControl.Player.RSkill.performed += RSkill;
    }

    private void OnDisable()
    {
        MoveInputValue.Disable();
        PlayerControl.Player.Jump.performed -= Jump;
        PlayerControl.Player.Dash.performed -= Dash;
        PlayerControl.Player.NormalAttack.performed -= NormalAttack;
        PlayerControl.Player.QSkill.performed -= QSkill;
        PlayerControl.Player.WSkill.performed -= WSkill;
        PlayerControl.Player.ESkill.performed -= ESkill;
        PlayerControl.Player.RSkill.performed -= RSkill;
    }

    void Start()
    {
        InitDelegate();
    }
    

    void Update()
    {
        MoveDir = MoveInputValue.ReadValue<Vector2>();
        normalAttackReserveTimer -= Time.deltaTime;
        if (normalAttackReserveTimer < 0f)
        {
            IsReserveNormalAttack = false;
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (CheckStateCanJump())
        {
            TransitionState(Define.EPlayerState.InAir);
        }
    }

    void Dash(InputAction.CallbackContext context)
    {
        //todo 쿨타임 부터 체크
        
        if (IsJumpingState())
        {
            TransitionState(Define.EPlayerState.Dash);
        }
        else if(CheckStateCanGroundSlide())
        {
            TransitionState(Define.EPlayerState.GroundSliding);
        }
    }

    bool IsJumpingState()
    {
        bool condition  = CheckCurrentState(Define.EPlayerState.InAir) ||
                          CheckCurrentState(Define.EPlayerState.Falling) ||
                          CheckCurrentState(Define.EPlayerState.JumpEnd);
        return condition;
    }
    
    void NormalAttack(InputAction.CallbackContext context)
    {
        if (CheckStateCanNormalAttack())
        {
            AttackDir = CurrentDir;
            //todo 장착 무기가 Dagger인지 Axe인지에 따라
            //TransitionState(Define.EPlayerState.DaggerNormalAttack);
            TransitionState(Define.EPlayerState.AxeNormalAttack);
            ;
            //TransitionState(Define.EPlayerState.BowNormalAttack);
        }
        else if (ControlledPlayer.StateMachine.CurrentState.IsAttacking)
        {
            //선입력 0.3초 후에 풀리게
            IsReserveNormalAttack = true;
            normalAttackReserveTimer = normalAttackReserveDuration;
        }
    }

    void InitDelegate()
    {
        
    }
    
    bool CheckStateCanJump()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.Idle) || CheckCurrentState(Define.EPlayerState.Move);
        return condition;
    }
    
    public bool CheckStateCanWallJump()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.WallSliding);
        return condition;
    }

    bool CheckStateCanGroundSlide()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.Move);
        return condition;
    }
    
    public bool CheckStateCanWallSlide()
    {
        if (IsDead()) return false;
        bool condition = ControlledPlayer.StateMachine.CurrentState != ControlledPlayer.GetState(Define.EPlayerState.WallJump);
     
        return condition;
    }

    bool CheckStateCanNormalAttack()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.Idle) ||
                         CheckCurrentState(Define.EPlayerState.Move);
        return condition;
    }
    
    protected override bool CheckStateCanMove()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.Idle);
        return condition;
    }

    protected override void TransitionMoveState()
    {
        TransitionState(Define.EPlayerState.Move);
    }
    
    public override bool IsWallDetect()
    {
        if (Physics2D.Raycast(wallDetectObject.transform.position, new Vector2(MoveDir.x, 0f), WallDetectDist, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }

    public bool CheckCurrentState(Define.EPlayerState playerState)
    {
        return ControlledPlayer.GetState(playerState) == ControlledPlayer.StateMachine.CurrentState;
    }

    bool IsDead()
    {
        return CheckCurrentState(Define.EPlayerState.Dead);
    }

    public void QSkill(InputAction.CallbackContext context)
    {
        //todo 조건확인 쿨타임, 내가 사용할 수 있는 상태인지 Idle, Move 
        AttackDir = CurrentDir;
        
        ActiveSkill skill = GI.Inst.ListenerManager.GetSkill(KeyCode.Q);
        ControlledPlayer.TransitionState(skill.skillState);
    }
    
    public void WSkill(InputAction.CallbackContext context)
    {
        //todo 조건확인 쿨타임, 내가 사용할 수 있는 상태인지 Idle, Move 
        AttackDir = CurrentDir;
        
        ActiveSkill skill = GI.Inst.ListenerManager.GetSkill(KeyCode.W);
        ControlledPlayer.TransitionState(skill.skillState);
    }
    
    public void ESkill(InputAction.CallbackContext context)
    {
        //todo 조건확인 쿨타임, 내가 사용할 수 있는 상태인지 Idle, Move 
        AttackDir = CurrentDir;
        
        ActiveSkill skill = GI.Inst.ListenerManager.GetSkill(KeyCode.E);
        ControlledPlayer.TransitionState(skill.skillState);
    }
    
    public void RSkill(InputAction.CallbackContext context)
    {
        //todo 조건확인 쿨타임, 내가 사용할 수 있는 상태인지 Idle, Move 
        AttackDir = CurrentDir;
        
        ActiveSkill skill = GI.Inst.ListenerManager.GetSkill(KeyCode.R);
        ControlledPlayer.TransitionState(skill.skillState);
    }
    
    protected void TransitionState(Define.EPlayerState playerState)
    {
        ControlledPlayer.TransitionState(playerState);
    }

    public void DaggerUltEnd()
    {
        ControlledPlayer.TransitionState(Define.EPlayerState.Idle);
    }
    
    
    
    

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(wallDetectObject.transform.position, new Vector3(wallDetectObject.transform.position.x + wallDetectDist * MoveDir.x, wallDetectObject.transform.position.y, wallDetectObject.transform.position.z));
    // }
}
