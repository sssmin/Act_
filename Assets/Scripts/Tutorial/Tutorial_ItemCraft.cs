using System.Collections.Generic;

public class Tutorial_ItemCraft : Tutorial
{
    protected override void StepInit()
    {
        tutorialSteps = new List<TutorialStep>();

        tutorialDialogUI = GI.Inst.UIManager.CreateTutorialDialog();
        tutorialDialogUI.gameObject.SetActive(false);
        
        TutorialStep_HideDialog hideDialogStep = new TutorialStep_HideDialog(tutorialDialogUI);
        TutorialStep_Dialog typingAndWaitActionStep = new TutorialStep_Dialog(tutorialDialogUI, EDialogType.TypingAndWaitAction);
        TutorialStep_Dialog typingAndNextStepStep = new TutorialStep_Dialog(tutorialDialogUI, EDialogType.TypingAndNextStep);
        TutorialStep_Dialog noTypingTransitionCompleteStep = new TutorialStep_Dialog(tutorialDialogUI, EDialogType.NoTypingTransitionComplete);
        TutorialStep_Callback callbackStep = new TutorialStep_Callback();
        TutorialStep_TitleTutorial titleStep = new TutorialStep_TitleTutorial();
        
        tutorialSteps.Add(titleStep.AddTitle("장비 제작 튜토리얼 시작"));
        
        tutorialSteps.Add(callbackStep.AddCallback(() =>
        {
            //UI 잠그기
            GI.Inst.ListenerManager.SwitchActionMap(false);
            //구매탭 버튼 상호작용 막기
            GI.Inst.UIManager.DisableMerchantCategoryBtn(EMerchantType.Buy);
        }));
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("장비는 재료를 이용하여 제작할 수 있어요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        tutorialSteps.Add(new TutorialStep_GiveItemToPlayer("WeaponMat", SO_Item.EItemCategory.Etc));
        tutorialSteps.Add(new TutorialStep_GiveItemToPlayer("SharedMat", SO_Item.EItemCategory.Etc));
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog("재료를 인벤토리에 넣어드렸으니, 무기를 제작해보세요." ,700f, 250f));
        tutorialSteps.Add(callbackStep.AddCallback(() =>
        {
            GI.Inst.UIManager.RefreshCraftLines();
        }));
       
        tutorialSteps.Add(new TutorialStep_WaitForAction(GI.Inst.UIManager.GetCraftButton(), EWaitForAction.ButtonPress));
        tutorialSteps.Add(noTypingTransitionCompleteStep);
        tutorialSteps.Add(hideDialogStep);
        tutorialSteps.Add(new TutorialStep_DestroyDialog(tutorialDialogUI));
        
        tutorialSteps.Add(titleStep.AddTitle("장비 제작 튜토리얼 완료"));
        tutorialSteps.Add(new TutorialStep_Complete(ETutorial.ItemCraft));
        
        tutorialSteps.Add(callbackStep.AddCallback(() =>
        {
            //UI 되돌리기
            GI.Inst.ListenerManager.SwitchActionMap(true);
            //구매탭 버튼 상호작용 풀기
            GI.Inst.UIManager.EnableMerchantCategoryBtn(EMerchantType.Buy);
        }));
        
        
    }
    
    
}
