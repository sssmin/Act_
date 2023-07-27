using System;
using System.Collections.Generic;

public class TutorialStep_Callback : TutorialStep
{
    public TutorialStep_Callback(){}
    

    private List<Action> callbacks = new List<Action>();
    private int index;
    
    public override void BeginStep()
    {
        callbacks[index]?.Invoke();
        index++;
        IsCompleted = true;
    }

    public override void Execute(Tutorial tutorial)
    {
        if (IsCompleted)
        {
            tutorial.SetNextTutorialStep();
        }
    }

    public override void EndStep()
    {
        IsCompleted = false;
    }

    public TutorialStep AddCallback(Action action)
    {
        callbacks.Add(action);
        return this;
    }
}
