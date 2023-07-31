using System.Collections.Generic;
using UnityEngine;

public class Tutorial_NewGame : Tutorial
{
    protected override void StepInit()
    {
        tutorialDialogUI = GI.Inst.UIManager.CreateTutorialDialog();
        tutorialDialogUI.gameObject.SetActive(false);
        
        TutorialStep_HideDialog hideDialogStep = new TutorialStep_HideDialog(tutorialDialogUI);
        TutorialStep_Dialog typingAndWaitActionStep = new TutorialStep_Dialog(tutorialDialogUI, EDialogType.TypingAndWaitAction);
        TutorialStep_Dialog typingAndNextStepStep = new TutorialStep_Dialog(tutorialDialogUI, EDialogType.TypingAndNextStep);
        TutorialStep_Dialog noTypingTransitionCompleteStep = new TutorialStep_Dialog(tutorialDialogUI, EDialogType.NoTypingTransitionComplete);
        TutorialStep_Callback callbackStep = new TutorialStep_Callback();
        TutorialStep_TitleTutorial titleStep = new TutorialStep_TitleTutorial();

        tutorialSteps = new List<TutorialStep>();
        
        tutorialSteps.Add(titleStep.AddTitle("기본 튜토리얼 시작"));
        
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog("A,D 키를 눌러 좌,우로 움직여보세요.", 700f, 250f));
        tutorialSteps.Add(new TutorialStep_WaitForAction(KeyCode.A, KeyCode.D, EWaitForAction.KeyPress));
        tutorialSteps.Add(noTypingTransitionCompleteStep);
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog($"{GI.Inst.Player.PlayerController.GetBindingKeyString(EBindKeyType.Jump)}키를 눌러 점프해보세요." ,700f, 250f));
        tutorialSteps.Add(new TutorialStep_WaitForAction(EBindKeyType.Jump, EWaitForAction.KeyPress));
        tutorialSteps.Add(noTypingTransitionCompleteStep);
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(callbackStep.AddCallback(() =>
        {
            GI.Inst.UIManager.VisibleMainUIComponent(EMainUIComponent.HealthBar);
            GI.Inst.UIManager.VisibleMainUIComponent(EMainUIComponent.EffectBar);
        }));

        tutorialSteps.Add(typingAndNextStepStep.AddDialog("플레이어의 체력이 표시되고, 아래의 빈칸은 패시브 스킬 쿨타임이나, 스킬 지속시간이 표시돼요." ,-240f, 370f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog($"{GI.Inst.Player.PlayerController.GetBindingKeyString(EBindKeyType.InventoryWindow)}키를 눌러 인벤토리를 열어보세요." ,700f, 250f));
        tutorialSteps.Add(new TutorialStep_WaitForAction(EBindKeyType.InventoryWindow, EWaitForAction.KeyPress));
        tutorialSteps.Add(noTypingTransitionCompleteStep);
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(callbackStep.AddCallback(() =>
        {
            GI.Inst.UIManager.VisibleMainUIComponent(EMainUIComponent.HotkeyBar);
        }));

        tutorialSteps.Add(new TutorialStep_GiveItemToPlayer("WoodDagger", SO_Item.EItemCategory.Weapon));
        
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog("기본 무기를 지급해드렸어요. 무기를 더블 클릭하거나 우클릭을 이용하여 장착해보세요." ,700f, 250f));
        tutorialSteps.Add(new TutorialStep_WaitForAction(EWaitForAction.EquipEquipment));
        tutorialSteps.Add(noTypingTransitionCompleteStep);
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog($"{GI.Inst.Player.PlayerController.GetBindingKeyString(EBindKeyType.NormalAttack)}키를 눌러 공격해보세요." ,700f, 250f));
        tutorialSteps.Add(new TutorialStep_WaitForAction(EBindKeyType.NormalAttack, EWaitForAction.KeyPress));
        tutorialSteps.Add(noTypingTransitionCompleteStep);
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("무기는 3가지 종류가 있어요. 종류마다 장착했을 때의 스킬이 달라져요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("무기, 방어구, 장신구는 재료를 이용해서 제작할 수 있고, 등급별로 확률이 정해져 있어요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("마을에 있는 NPC를 통해 제작이 가능해요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog($"{GI.Inst.Player.PlayerController.GetBindingKeyString(EBindKeyType.SkillWindow)}키를 눌러 스킬 창을 열어보세요." ,700f, 250f));        
        tutorialSteps.Add(new TutorialStep_WaitForAction(EBindKeyType.SkillWindow, EWaitForAction.KeyPress));
        tutorialSteps.Add(noTypingTransitionCompleteStep);
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("액티브 스킬은 앞서 설명한 것처럼 무기에 따라 달라지고, 패시브는 고정된 5개 중 3개를 드래그를 이용하여 장착할 수 있어요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog("아무 패시브 스킬을 드래그하여 장착해보세요." ,700f, 250f));
        tutorialSteps.Add(new TutorialStep_WaitForAction(EWaitForAction.EquipPassiveSkill));
        tutorialSteps.Add(noTypingTransitionCompleteStep);
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("또한 액티브 스킬 레벨업 재료, 패시브 스킬 레벨업 재료를 이용하여 스킬을 레벨업 할 수 있어요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("무기가 달라져도 동일한 위치의 액티브 스킬 레벨은 유지돼요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("특정 레벨 구간부터 요구하는 재료 개수가 달라져요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog($"{GI.Inst.Player.PlayerController.GetBindingKeyString(EBindKeyType.Dash)}키를 눌러 가려는 방향으로 대쉬해보세요." ,700f, 250f));
        tutorialSteps.Add(new TutorialStep_WaitForAction(EWaitForAction.GroundSliding));
        tutorialSteps.Add(noTypingTransitionCompleteStep);
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("공중에서도 대쉬할 수 있어요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(typingAndNextStepStep.AddDialog("벽을 향해 점프하면 벽을 탈 수 있고, 벽을 탄 상태에서 점프하면 벽 점프를 해요." ,700f, 250f));
        tutorialSteps.Add(hideDialogStep);
        
        tutorialSteps.Add(callbackStep.AddCallback(() => //깃발 포커스
        {
            Transform flagTransform = GameObject.Find("Flag")?.transform;
            GI.Inst.CinemachineTarget.FocusOnlyThisObj(flagTransform);
        }));
        
        tutorialSteps.Add(typingAndWaitActionStep.AddDialog("대쉬와 벽 점프를 이용해서 깃발로 가보세요." ,700f, 250f));
      
        tutorialSteps.Add(callbackStep.AddCallback(() => //포커스 해제
        {
            GI.Inst.CinemachineTarget.RemoveOneFocus();
            GI.Inst.CinemachineTarget.ActivateCamera();
        }));
        tutorialSteps.Add(new TutorialStep_WaitForAction(EWaitForAction.TriggerStep, this));

        tutorialSteps.Add(new TutorialStep_DestroyDialog(tutorialDialogUI));

        tutorialSteps.Add(titleStep.AddTitle("기본 튜토리얼 완료"));
        tutorialSteps.Add(new TutorialStep_Complete(ETutorial.Max));
        tutorialSteps.Add(new TutorialStep_LoadScene("Town", 5f));
        
    }

    public override void SkipTutorial()
    {
        base.SkipTutorial();
        
        tutorialSteps.Add(new TutorialStep_GiveItemToPlayer("WoodDagger", SO_Item.EItemCategory.Weapon));
        tutorialSteps.Add(new TutorialStep_Complete(ETutorial.Max));
        tutorialSteps.Add(new TutorialStep_LoadScene("Town", 1f));
        SetNextTutorialStep();
    }
}
