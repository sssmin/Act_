using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : BaseController
{
    //몬스터마다 컨트롤러 들고 있음. 여기에서 상태 제어
    public Player Target { get; private set; }
    private Monster ControlledMonster { get; set; }
    private Define.EMonsterState stateBeforeFreeze;

    public bool IsFrontWall { get; private set; }
    public bool IsFrontGround { get; private set; }
    
    public override Vector2 CurrentDir
    {
        get => base.CurrentDir;
        protected set
        {
            base.CurrentDir = value;
            if (CurrentDir.x < 0f)
            {
                Flip(true);
            }
            else if (CurrentDir.x > 0f)
            {
                Flip(false);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        ControlledMonster = GetComponent<Monster>();
        CurrentDir = new Vector2(1f, 0f);
    }

    void Start()
    {
        
    }

    void Update()
    {
        CheckFrontWall();
        CheckFrontGround();
    }

    void CheckFrontWall()
    {
        if (IsWallDetect())
            IsFrontWall = true;
        else
            IsFrontWall = false;
    }
    
    void CheckFrontGround()
    {
        if (IsGroundDetect())
            IsFrontGround = true;
        else
            IsFrontGround = false;
    }

    public void TurnDir()
    {
        CurrentDir = new Vector2(CurrentDir.x * -1, CurrentDir.y);
    }
    
    public void FreezeMonster()
    {
        ControlledMonster.Animator.speed = 0f;
        MonsterState currentState = (MonsterState)ControlledMonster.StateMachine.CurrentState;
        stateBeforeFreeze = currentState.monsterStateType;
        ControlledMonster.TransitionState(Define.EMonsterState.Freeze);
    }

    public void UnfreezeMonster()
    {
        ControlledMonster.Animator.speed = 1f;
        ControlledMonster.TransitionState(stateBeforeFreeze);
        stateBeforeFreeze = Define.EMonsterState.None;
    }
}
