
public class TutorialDiaglog : Tutorial
{
    private DialogSystem DialogSystem { get; set; }
    
    public override void BeginTutorial()
    {
        DialogSystem = gameObject.GetComponent<DialogSystem>();
        DialogSystem.Setup();
    }

    public override void Execute(TutorialManager tutorialManager)
    {
        bool isCompleted = DialogSystem.UpdateDialog();
        if (isCompleted)
        {
            tutorialManager.SetNextTutorial();
        }
    }
    
}
