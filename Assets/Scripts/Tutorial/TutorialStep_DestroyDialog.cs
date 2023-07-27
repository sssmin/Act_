using UnityEngine;

public class TutorialStep_DestroyDialog : TutorialStep
{
    public TutorialStep_DestroyDialog(UI_TutorialDialog tutorialDialogUI)
    {
        tutorialDialog = tutorialDialogUI;
    }

    private UI_TutorialDialog tutorialDialog;
    public override void BeginStep()
    {
        if (tutorialDialog)
            GameObject.Destroy(tutorialDialog.gameObject);
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
