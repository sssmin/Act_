
using UnityEngine;
using UnityEngine.UI;

public enum EWaitForAction
{
    KeyPress,
    ButtonPress,
    EquipEquipment,
    EquipPassiveSkill,
    GroundSliding,
    TriggerStep
}

public class TutorialStep_WaitForAction : TutorialStep
{
    public TutorialStep_WaitForAction(EWaitForAction inWaitForAction)
    {
        waitForAction = inWaitForAction;
    }
    
    public TutorialStep_WaitForAction(EWaitForAction inWaitForAction, Tutorial tutorial)
    {
        waitForAction = inWaitForAction;
        Tutorial = tutorial;
    }
    
    public TutorialStep_WaitForAction(KeyCode first, EWaitForAction inWaitForAction)
    {
        WaitForFirstKeyCode = first;
        waitForAction = inWaitForAction;
    }
    
    public TutorialStep_WaitForAction(KeyCode first, KeyCode second, EWaitForAction inWaitForAction)
    {
        WaitForFirstKeyCode = first;
        WaitForSecondKeyCode = second;
        waitForAction = inWaitForAction;
    }
    public TutorialStep_WaitForAction(EBindKeyType first, EWaitForAction inWaitForAction)
    {
        WaitForFirstKeyType = first;
        waitForAction = inWaitForAction;
    }
    
    public TutorialStep_WaitForAction(EBindKeyType first, EBindKeyType second, EWaitForAction inWaitForAction)
    {
        WaitForFirstKeyType = first;
        WaitForSecondKeyType = second;
        waitForAction = inWaitForAction;
    }
    
    public TutorialStep_WaitForAction(Button button, EWaitForAction inWaitForAction)
    {
        WaitForPressButton = button;
        waitForAction = inWaitForAction;
    }

    private EWaitForAction waitForAction;
    private KeyCode WaitForFirstKeyCode { get; set; } = KeyCode.End;
    private KeyCode WaitForSecondKeyCode { get; set; }  = KeyCode.End;
    private EBindKeyType WaitForFirstKeyType { get; set; } = EBindKeyType.None;
    private EBindKeyType WaitForSecondKeyType { get; set; } = EBindKeyType.None;
    private Button WaitForPressButton { get; set; }
    private bool IsCompleteFirstKey { get; set; } = true;
    private bool IsCompleteSecondKey { get; set; } = true;
    
    private Tutorial Tutorial { get; set; } 

    public override void BeginStep()
    {
        if (WaitForPressButton)
        {
            WaitForPressButton.onClick.AddListener(OnClickButton);
        }
        
        if (WaitForFirstKeyType != EBindKeyType.None)
        {
            string key = GI.Inst.Player.PlayerController.GetBindingKeyString(WaitForFirstKeyType);
            WaitForFirstKeyCode = PlayerController.KeyToKeyCode(key);
        }
        if (WaitForSecondKeyType != EBindKeyType.None)
        {
            string key = GI.Inst.Player.PlayerController.GetBindingKeyString(WaitForSecondKeyType);
            WaitForSecondKeyCode = PlayerController.KeyToKeyCode(key);
        }

        if (WaitForFirstKeyCode != KeyCode.End)
            IsCompleteFirstKey = false;
        if (WaitForSecondKeyCode != KeyCode.End)
            IsCompleteSecondKey = false;

        if (waitForAction == EWaitForAction.TriggerStep)
            Tutorial.bIsCompleteTriggerStep = false;
    }

    public override void Execute(Tutorial tutorial)
    {
        if (IsCompleted)
        {
            tutorial.SetNextTutorialStep();
            return;
        }

        if (GI.Inst.ListenerManager.IsEnablePlayerControl())
        {
            switch (waitForAction)
            {
                case EWaitForAction.KeyPress:
                    if (WaitForFirstKeyCode != KeyCode.End)
                    {
                        if (Input.GetKey(WaitForFirstKeyCode))
                            IsCompleteFirstKey = true;
                    }
                    if (WaitForSecondKeyCode != KeyCode.End)
                    {
                        if (Input.GetKey(WaitForSecondKeyCode))
                            IsCompleteSecondKey = true;
                    }
                    if (IsCompleteFirstKey && IsCompleteSecondKey)
                        IsCompleted = true;
                    break;
                case EWaitForAction.EquipEquipment:
                    if (GI.Inst.ListenerManager.GetEquippedWeapon())
                        IsCompleted = true;
                    break;
                case EWaitForAction.EquipPassiveSkill:
                    if (GI.Inst.ListenerManager.IsAnyEquippedPassiveSkill())
                        IsCompleted = true;
                    break;
                case EWaitForAction.GroundSliding:
                    if (GI.Inst.Player.PlayerController.CheckCurrentState(Define.EPlayerState.GroundSliding))
                        IsCompleted = true;
                    break;
                case EWaitForAction.TriggerStep:
                    if (Tutorial.bIsCompleteTriggerStep)
                        IsCompleted = true;
                    break;
            }
        }
    }

    public override void EndStep()
    {
       
    }

    private void OnClickButton()
    {
        IsCompleted = true;
    }
}
