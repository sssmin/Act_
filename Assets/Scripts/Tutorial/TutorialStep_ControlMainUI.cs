

using System;

public class TutorialStep_ControlMainUI : TutorialStep
{
    public TutorialStep_ControlMainUI(Action inCallback)
    {
        callback = inCallback;
    }
    
    public TutorialStep_ControlMainUI(EMainUIComponent mainUIComponent, EMainUIComponent multipleMainUIComponent)
    {
        
    }

    private Action callback;
    
    public override void BeginStep()
    {
        callback?.Invoke();
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
       
    }
    
}
