using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class AIController : BaseController
{
    public Player Target { get; private set; }
    public Monster ControlledMonster { get; private set; }
    private Define.EMonsterState stateBeforeCrowdControl;
    private BoxCollider2D PlayerDetectCollider { get; set; }
    private Coroutine coTransitionState;

    public bool IsFrontWall { get; private set; }
    public bool IsFrontGround { get; private set; }
    public float NormalAttackRange { get; set; }
    public float NormalAttackCooltime { get; private set; }

    private bool bCanNormalAttack;
    public bool CanNormalAttack
    {
        get
        {
            if (ControlledMonster.StatManager.IsDead) return false;
            return bCanNormalAttack;
        }
        
        private set
        {
            bCanNormalAttack = value;
        }
    }

    private float RandDistModifier { get; set; }
    private Transform NormalAttackCollider { get; set; }
    
    public override Vector2 CurrentDir
    {
        get => base.CurrentDir;
        protected set
        {
            base.CurrentDir = value;
            if (CurrentDir.x < 0f)
            {
                Flip(EDir.Left);
                ((MonsterStatManager)ControlledMonster.StatManager).FlipMonsterInfoUI(EDir.Left);
            }
            else if (CurrentDir.x > 0f)
            {
                Flip(EDir.Right);
                ((MonsterStatManager)ControlledMonster.StatManager).FlipMonsterInfoUI(EDir.Right);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        ControlledMonster = GetComponent<Monster>();
        NormalAttackCollider = GetComponentInChildren<NormalAttackCollider>().transform;
      
        NormalAttackCooltime = 2.3f;
        CanNormalAttack = true;
    }

    public virtual void Start()
    {
        PlayerDetectCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
    }

    public virtual void Update()
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
    
    public void ExecNormalAttack()
    {
        Collider2D[] colliders = new Collider2D[10];
        Physics2D.OverlapCircleNonAlloc(NormalAttackCollider.transform.position, 2f, colliders, LayerMask.GetMask("Player"));
        
        foreach (var collider in colliders)
        {
            if (collider == null) break;
            
            GameObject enemyGo = collider.gameObject;
            StatManager enemyStatManager = enemyGo.GetComponent<StatManager>();
            if (enemyStatManager == StatManager) return;
            StatManager.CauseNormalAttack(enemyStatManager);
        }
    }
    
    public void NormalAttackCompleted()
    {
        CanNormalAttack = false;
        RandDistModifier = Random.Range(-0.2f, 0.8f);
        StartCoroutine(CoStartNormalAttackCooltime());
    }

    IEnumerator CoStartNormalAttackCooltime()
    {
        yield return new WaitForSeconds(NormalAttackCooltime + RandDistModifier);
        CanNormalAttack = true;
    }
    
    //player detect collider trigger
    private void OnTriggerEnter2D(Collider2D col)
    {
        Player player = col.GetComponent<Player>();
        if (player)
        {
            Target = player;
            ControlledMonster.TransitionState(Define.EMonsterState.Chase);
            if (coTransitionState != null)
            {
                StopCoroutine(coTransitionState);
            }
        }
    }

    public virtual void OnTriggerExit2D(Collider2D col)
    {
        Player player = col.GetComponent<Player>();
        if (player)
        {
            //일정 시간 후에 패트롤로
            coTransitionState = StartCoroutine(CoLoseTarget(Define.EMonsterState.Patrol, 2f));
        }
    }

    IEnumerator CoLoseTarget(Define.EMonsterState monsterState, float second)
    {
        yield return new WaitForSeconds(second);
        Target = null;
        ControlledMonster.TransitionState(monsterState);
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
    
    public bool CheckCurrentState(Define.EMonsterState monsterState)
    {
        return ControlledMonster.GetState(monsterState) == ControlledMonster.StateMachine.CurrentState;
    }
}
