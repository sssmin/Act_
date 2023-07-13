

public class WandererMagician : Monster
{
    protected override void Start()
    {
        base.Start();
        
        AIController.NormalAttackRange = 3.5f;
    }
    
    protected override void InitState()
    {
        States.Add(Define.EMonsterState.Idle, new Monster_IdleState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Patrol, new WandererMagician_PatrolState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Chase, new WandererMagician_ChaseState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Freeze, new Monster_FreezeState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Suppression, new Monster_SuppressionState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Dead, new Monster_DeadState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.NormalAttack1, new Monster_NormalAttackState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.NormalAttack2, new WandererMagician_SphereAttackState(Animator, Rb, this, AIController));
        
        foreach (var pair in States)
        {
            ((MonsterState)pair.Value).monsterStateType = pair.Key;
        }
    }
}
