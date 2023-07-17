using System.Collections.Generic;


public class Monster : BaseCharacter
{
    public Dictionary<Define.EMonsterState, State> States = new Dictionary<Define.EMonsterState, State>();
   
    private string monsterPrefabId;
    public string MonsterPrefabId
    {
        get => monsterPrefabId;
        set => monsterPrefabId = value;
    }
    private DropTable dropTable;
    public DropTable DropTable
    {
        get => dropTable;
        set => dropTable = value;
    }
    public AIController AIController { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        AIController = GetComponent<AIController>();
        StatManager.InstId = InstId;
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Init(States[Define.EMonsterState.Idle]);
    }

    protected override void InitState()
    {
        States.Add(Define.EMonsterState.Idle, new Monster_IdleState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Patrol, new Monster_PatrolState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Chase, new Monster_ChaseState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Freeze, new Monster_FreezeState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Suppression, new Monster_SuppressionState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Dead, new Monster_DeadState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.NormalAttack1, new Monster_NormalAttackState(Animator, Rb, this, AIController));


        foreach (var pair in States)
        {
            ((MonsterState)pair.Value).monsterStateType = pair.Key;
        }
    }
    
    public void TransitionState(Define.EMonsterState monsterState)
    {
        if (States.ContainsKey(monsterState))
        {
            StateMachine.TransitionState(States[monsterState]);
        }
    }

    public void DestroyMySelf()
    {
        if (HitEffectCoroutine != null)
        {
            StopCoroutine(HitEffectCoroutine);
        }
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
    
    public State GetState(Define.EMonsterState monsterState)
    {
        if (States.ContainsKey(monsterState))
        {
            return States[monsterState];
        }
        return null;
    }
}
