

public class TutorialStep_HideDialog : TutorialStep
{
    public TutorialStep_HideDialog(UI_TutorialDialog inTutorialDialogUI)
    {
        tutorialDialogUI = inTutorialDialogUI;
    }
    
    UI_TutorialDialog tutorialDialogUI;
    public override void BeginStep()
    {
        IsCompleted = false;
        tutorialDialogUI.FadeOut(this);
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
