
public class TutorialStep_Diallog : TutorialStep
{
    public TutorialStep_Diallog(string inText, float inX, float inY, UI_TutorialDialog inTutorialDialogUI)
    {
        text = inText;
        x = inX;
        y = inY;
        tutorialDialogUI = inTutorialDialogUI;
    }
    private string text;
    private float x;
    private float y;
    UI_TutorialDialog tutorialDialogUI;
    
    public override void BeginStep()
    {
        tutorialDialogUI.gameObject.SetActive(true);
    }

    public override void Execute(Tutorial tutorial)
    {
        if (isCompleted)
        {
            tutorial.SetNextTutorialStep();
        }
    }

    public override void EndStep()
    {
        tutorialDialogUI.Clear();
        tutorialDialogUI.gameObject.SetActive(false);
    }
    
}
