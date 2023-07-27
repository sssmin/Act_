using System.Collections.Generic;



public class TutorialStep_TitleTutorial : TutorialStep
{
    public TutorialStep_TitleTutorial(){}
    
    private string titleText;
    private List<string> titleTexts = new List<string>();
    private int index;
    
    public override void BeginStep()
    {
        UI_TutorialTitle tutorialTitleUI = GI.Inst.UIManager.CreateTutorialTitle();
        tutorialTitleUI.SetTitle(titleTexts[index]);
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

    public TutorialStep AddTitle(string title)
    {
        titleTexts.Add(title);
        return this;
    }
    
}
