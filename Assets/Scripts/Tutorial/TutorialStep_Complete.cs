using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep_Complete : TutorialStep
{
    public TutorialStep_Complete(ETutorial type)
    {
        tutorialType = type;
    }

    private ETutorial tutorialType;
    
    public override void BeginStep()
    {
        IsCompleted = true;
        GI.Inst.UIManager.DestroyTutorialSkipButton();
        
        GI.Inst.ListenerManager.EnablePlayerControl();
    }

    public override void Execute(Tutorial tutorial)
    {
        if (IsCompleted)
        {
            GI.Inst.TutorialManager.SetTutorialCompleted(tutorialType);
            tutorial.SetNextTutorialStep();
        }
    }

    public override void EndStep()
    {
        
    }
}
