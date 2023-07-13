

public class Necromancer : Monster
{
    protected override void Start()
    {
        base.Start();

        AIController.NormalAttackRange = 5f;
    }
    
    protected override void InitState()
    {
        States.Add(Define.EMonsterState.Idle, new Monster_IdleState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Patrol, new Monster_PatrolState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Chase, new Monster_ChaseState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Freeze, new Monster_FreezeState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Suppression, new Monster_SuppressionState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Dead, new Monster_DeadState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.NormalAttack1, new Necromancer_NormalAttackState(Animator, Rb, this, AIController));

        foreach (var pair in States)
        {
            ((MonsterState)pair.Value).monsterStateType = pair.Key;
        }
    }
}
