using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialStep
{
    public abstract void BeginStep();
    public abstract void Execute(Tutorial tutorial);
    public abstract void EndStep();
    public bool IsCompleted { get; set; }
}

public class Tutorial 
{
    protected List<TutorialStep> tutorialSteps;
    private TutorialStep currentTutorialStep;
    private int currentIndex;
    protected UI_TutorialDialog tutorialDialogUI;
    public bool bIsCompleteTriggerStep;
    
    public void StartTutorial()
    {
        currentIndex = -1;
        StepInit();
        SetNextTutorialStep();
        GI.Inst.UIManager.CreateTutorialSkipButton(4f);
    }

    protected virtual void StepInit()
    {
    }
   
    public void SetNextTutorialStep()
    {
        if (currentTutorialStep != null)
        {
            currentTutorialStep.EndStep();
        }
    
        if (++currentIndex >= tutorialSteps.Count)
        {
            AllTutorialStepsCompleted();
            return;
        }
    
        currentTutorialStep = tutorialSteps[currentIndex];
        currentTutorialStep.BeginStep();
    }

    public void Execute()
    {
        if (currentTutorialStep != null)
        {
            currentTutorialStep.Execute(this);
        }
    }
    
    public void AllTutorialStepsCompleted() 
    {
        currentTutorialStep = null;
    }

    public virtual void SkipTutorial()
    {
        currentTutorialStep = null;
        tutorialSteps.Clear();
        currentIndex = -1;
        tutorialSteps.Add(new TutorialStep_DestroyDialog(tutorialDialogUI));
        
        //override 한 튜토리얼에서 다이얼로그 지우고 난 후에 동작을 스텝에 넣고, SetNextStep 호출
    }
}