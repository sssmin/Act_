using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BaseCharacter
{
    public Dictionary<Define.EMonsterState, State> States = new Dictionary<Define.EMonsterState, State>();
    //todo 몬스터도 State를 가질건데 Player랑은 기능 자체는 다를듯

    protected AIController AIController { get; set; }

    protected override void Awake()
    {
        base.Awake();

        AIController = GetComponent<AIController>();
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Init(States[Define.EMonsterState.Idle]);
        //GI.Inst.ListenerManager.onTransitionStateReq += TransitionState;
    }

    protected override void InitState()
    {
        States.Add(Define.EMonsterState.Idle, new Monster_IdleState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Patrol, new Monster_PatrolState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Chase, new Monster_ChaseState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Freeze, new Monster_FreezeState(Animator, Rb, this, AIController));
        States.Add(Define.EMonsterState.Suppression, new Monster_SuppressionState(Animator, Rb, this, AIController));
        // States.Add(Define.EMonsterState.CrowdControl, new Monster_IdleState(Animator, Rb, this, AIController));
        // States.Add(Define.EMonsterState.NormalAttack, new Monster_IdleState(Animator, Rb, this, AIController));
        // States.Add(Define.EMonsterState.Dead, new Monster_IdleState(Animator, Rb, this, AIController));
        
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
    
}
