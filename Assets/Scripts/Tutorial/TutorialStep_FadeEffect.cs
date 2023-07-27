using UnityEngine;

public class TutorialStepFadeEffect : TutorialStep
{
    [SerializeField] private FadeEffect fadeEffect;
    [SerializeField] private bool isFadeIn;
    private bool isCompleted;
    
   public override void BeginTutorial()
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
            tutorial.SetNextTutorial();
        }
    }
    
}
