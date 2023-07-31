
//다이얼로그 타입:
//타이핑하고, 진행중 변경되는거(액션 대기)
//타이핑하고, 시간 후에 다음 단계로 넘어가는거
//타이핑 없이 진행중을 완료로 바꾸는거

using System.Collections.Generic;

public enum EDialogType
{
    TypingAndWaitAction,
    TypingAndNextStep,
    NoTypingTransitionComplete
}

public struct DialogInfo
{
    public DialogInfo(string inText, float inX, float inY)
    {
        text = inText;
        x = inX;
        y = inY;
    }
    public string text;
    public float x;
    public float y;
}

public class TutorialStep_Dialog : TutorialStep
{
   
    public TutorialStep_Dialog(UI_TutorialDialog inTutorialDialogUI, EDialogType type)
    {
        tutorialDialogUI = inTutorialDialogUI;
        dialogType = type;
    }
    
    private string text;
    private float x;
    private float y;

    private List<DialogInfo> infos = new List<DialogInfo>();
    private int index;
    private UI_TutorialDialog tutorialDialogUI;
    private EDialogType dialogType;
    
    
    public override void BeginStep()
    {
        IsCompleted = false;
        
        switch (dialogType)
        {
            case EDialogType.TypingAndWaitAction:
                tutorialDialogUI.gameObject.SetActive(true);
                tutorialDialogUI.Init(infos[index].text, infos[index].x, infos[index].y, this, dialogType);
                GI.Inst.ListenerManager.DisablePlayerControl();
                index++;
                break;
            case EDialogType.TypingAndNextStep:
                tutorialDialogUI.gameObject.SetActive(true);
                tutorialDialogUI.Init(infos[index].text, infos[index].x, infos[index].y, this, dialogType);
                index++;
                break;
            case EDialogType.NoTypingTransitionComplete:
                GI.Inst.SoundManager.PlayEffectSound("StepComplete");
                tutorialDialogUI.SetStatusText(ETutorialStatus.Complete, this);
                break;
        }
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
        GI.Inst.ListenerManager.EnablePlayerControl();
    }
    
    public TutorialStep AddDialog(string inText, float inX, float inY)
    {
        DialogInfo info = new DialogInfo(inText, inX, inY);
        infos.Add(info);
        return this;
    }
    
}
