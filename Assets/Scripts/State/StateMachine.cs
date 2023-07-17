using System;
using UnityEngine;

[Serializable]
public class StateMachine : MonoBehaviour
{
    public State CurrentState;
    

    public void Init(State state)
    {
        CurrentState = state;
        CurrentState.BeginState();
    }
    
    public void TransitionState(State state)
    {
        CurrentState.EndState();
        CurrentState = state;
        CurrentState.BeginState();
    }
    
}
