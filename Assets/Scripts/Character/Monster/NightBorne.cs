



using UnityEngine;

public class NightBorne : Monster
{
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
        
    }

    protected override void InitState()
    {
        base.InitState();
        
        // States.Add(Define.EMonsterState.Idle, new Monster_IdleState(Animator, Rb, this, AIController)); 
        // States.Add(Define.EMonsterState.Patrol, new Monster_PatrolState(Animator, Rb, this, AIController)); 
        // States.Add(Define.EMonsterState.Chase, new Monster_ChaseState(Animator, Rb, this, AIController)); 
        //
        //
        // foreach (var pair in States)
        // {
        //     ((MonsterState)pair.Value).monsterStateType = pair.Key;
        // }
    }
}
