using UnityEngine;

public class TutorialStep_FadeEffect : TutorialStep
{
    [SerializeField] private FadeEffect fadeEffect;
    [SerializeField] private bool isFadeIn;
    private bool isCompleted;
    
   public override void BeginStep()
   {
       if (isFadeIn)
           fadeEffect.FadeIn(OnAfterFadeEffect);
       else
           fadeEffect.FadeOut(OnAfterFadeEffect);
   }

    private void OnAfterFadeEffect()
    {
        isCompleted = true;
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
    }
    
}
