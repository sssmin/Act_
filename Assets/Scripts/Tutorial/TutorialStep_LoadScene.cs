
public class TutorialStep_LoadScene : TutorialStep
{
    public TutorialStep_LoadScene(string inSceneName, float inWaitForSeconds)
    {
        sceneName = inSceneName;
        waitForSeconds = inWaitForSeconds;
    }

    private string sceneName;
    private float waitForSeconds;
    public override void BeginStep()
    {
        GI.Inst.SceneLoadManager.RequestLoadSceneAsync(sceneName, waitForSeconds);
    }

    public override void Execute(Tutorial tutorial)
    {
       
    }

    public override void EndStep()
    {
        
    }
}
