using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class State
{
    public State() { }
    
    public State(Animator animator, Rigidbody2D rigidbody2D, BaseCharacter character, BaseController baseController)
    {
        Animator = animator;
        Rb = rigidbody2D;
        GI.Inst.ListenerManager.onTriggerAnim -= OnTriggerAnim;
        GI.Inst.ListenerManager.onTriggerAnim += OnTriggerAnim;
    }
    
    public Animator Animator { get; set; }
    public Rigidbody2D Rb { get; set; }
    public bool IsAttacking { get; set; }
    public bool IsPauseAnim { get; protected set; } //애님 pause 완료하면 AnimTrigger에서 현재 상태에 알리기 위한 변수
    protected bool IsAnimTrigger { get; set; }
    
    public virtual void Update()
    {
        
    }

    public virtual void BeginState()
    {
        
    }

    public virtual void EndState()
    {
        
    }

    public void AnimPauseNotify() //State에서 아래 변수 사용
    {
        IsPauseAnim = true;
    }
    
    public virtual void OnTriggerAnim(int instanceId)
    {
        
    }
    
}





