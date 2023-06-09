using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : BaseController
{
    //몬스터마다 컨트롤러 들고 있음. 여기에서 상태 제어
    public Player Target { get; private set; }
    private Monster ControlledMonster { get; set; }
    private Define.EMonsterState stateBeforeCrowdControl;

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
        ;
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
    
    
    public void SetDisableState(Define.EMonsterState monsterState) //아무것도 못하는 상태
    {
        ControlledMonster.Animator.speed = 0f;
        SetBeforeState();
        ControlledMonster.TransitionState(monsterState);
    }

    public void RevertState()
    {
        ControlledMonster.Animator.speed = 1f;
        ControlledMonster.TransitionState(stateBeforeCrowdControl);
        stateBeforeCrowdControl = Define.EMonsterState.None;
    }

    void SetBeforeState()
    {
        MonsterState currentState = (MonsterState)ControlledMonster.StateMachine.CurrentState;
        //DisableState 걸때 이전 상태로 돌아가도록 이전 상태 저장하는데,
        //만약 DisableState 걸린 상태에서 DisableState를 걸면 DisableState를 이전 상태로 저장해버리기 때문에 그걸 방지하기 위함
        if (!IsDisableState(currentState)) 
            stateBeforeCrowdControl = currentState.monsterStateType;
    }

    bool IsDisableState(MonsterState currentState)
    {
        bool condition = currentState.monsterStateType == Define.EMonsterState.Freeze ||
                         currentState.monsterStateType == Define.EMonsterState.Suppression;
        return condition;
    }
}
