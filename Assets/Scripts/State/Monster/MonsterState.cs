using System;
using UnityEngine;

[Serializable]
public class MonsterState : State
{
    public MonsterState() { }
    public Define.EMonsterState monsterStateType;
    
    public MonsterState(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController) : base(animator, rigidbody2D, character, baseController)
    {
        AIController = (AIController)baseController;
        Monster = (Monster)character;
    }
    
    protected AIController AIController { get; set; }
    protected Monster Monster { get; set; }
    

    public override void Update()
    {
        
    }
    
    public void TransitionState(Define.EMonsterState monsterState)
    {
        Monster.TransitionState(monsterState);
    }
    
    public override void OnTriggerAnim(int instanceId)
    {
        if (Monster.InstId == instanceId)
            IsAnimTrigger = true;
    }
}