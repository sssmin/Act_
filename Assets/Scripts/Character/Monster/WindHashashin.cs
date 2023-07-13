
public class WindHashashin : Monster
{
    protected override void Awake()
    {
        base.Awake();
        
        AIController = GetComponent<AIController_WindHashashin>();
    }

    protected override void Start()
    {
        base.Start();
        
        AIController.NormalAttackRange = 3.5f;
    }
    
    protected override void InitState()
    {
        States.Add(Define.EMonsterState.Idle, new WindHashashin_IdleState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Chase, new WindHashashin_ChaseState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Freeze, new Monster_FreezeState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Suppression, new Monster_SuppressionState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Dead, new Monster_DeadState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.NormalAttack1, new WindHashashin_NormalAttack1State(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.NormalAttack2, new WindHashashin_NormalAttack2State(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.NormalAttack3, new WindHashashin_Tornado(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.SpecialAttack1, new WindHashashin_SpecialAttackState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Dodge, new WindHashashin_DodgeState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Defend, new WindHashashin_DefendState(Animator, Rb, this, AIController));
        
        foreach (var pair in States)
        {
            ((MonsterState)pair.Value).monsterStateType = pair.Key;
        }
    }
}
