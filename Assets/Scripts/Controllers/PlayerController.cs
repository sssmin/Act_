using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public enum EBindKeyType
{
    None,
    Jump,
    Dash,
    NormalAttack,
    FirstSkill,
    SecondSkill,
    ThirdSkill,
    FourthSkill,
    InventoryWindow,
    SkillWindow,
    FirstItemHotkey,
    SecondItemHotkey,
    ThirdItemHotkey,
    FourthItemHotkey,
    FifthItemHotkey,
    Interaction
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : BaseController
{
    public PlayerControl PlayerControl { get; set; }
    private InputAction MoveInputValue { get; set; }
    private bool bNormalAttackReserve;
    public bool IsReserveNormalAttack
    {
        get => bNormalAttackReserve;
        private set => bNormalAttackReserve = value;
    }
    private float normalAttackReserveDuration;
    private float normalAttackReserveTimer;
    
    public Player ControlledPlayer { get; private set; }
    private bool bIsCharging;
    public bool IsCharging
    {
        get => bIsCharging;
        private set => bIsCharging = value;
    }
    private bool bChargeCompleted;
    public bool ChargeCompleted
    {
        get => bChargeCompleted;
        private set => bChargeCompleted = value;
    }
    
    private float currentChargeAmount;
    public float CurrentChargeAmount
    {
        get => currentChargeAmount;
        private set => currentChargeAmount = value;
    }
    private float chargeTimer;
    private float maxChargingTime;
    private KeyCode chargeKeyCode;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    
    public Vector2 AttackDir { get; private set; }

    private Vector2 moveDir;
    public Vector2 MoveDir
    {
        get => moveDir;
        private set
        {
            moveDir = value;
         
            if (moveDir.x < 0f)
            {
                CurrentDir = moveDir;
            }
            else if (moveDir.x > 0f)
            {
                CurrentDir = moveDir;
            }
        }
    }

    public override Vector2 CurrentDir
    {
        protected set
        {
            base.CurrentDir = value;
            if (CurrentDir.x < 0f)
            {
                if (!ControlledPlayer.DoNotFlipState())
                {
                    Flip(EDir.Left);
                }
                if (CheckStateCanMove() && !IsWallDetect())
                {
                    TransitionMoveState();
                }
            }
            else if (CurrentDir.x > 0f)
            {
                if (!ControlledPlayer.DoNotFlipState())
                {
                    Flip(EDir.Right);
                }
                
                if (CheckStateCanMove() && !IsWallDetect())
                {
                    TransitionMoveState();
                }
            }
        }
    }

    private void SwitchActionMap(bool isUI)
    {
        if (isUI)
        {
            PlayerControl.Player.Esc.Disable();
            
            PlayerControl.UI.Esc.Enable();
        }
        else
        {
            PlayerControl.Player.Esc.Enable();
            
            PlayerControl.UI.Esc.Disable();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        ControlledPlayer = GetComponent<Player>();
        PlayerControl = new PlayerControl();
        normalAttackReserveDuration = 0.3f;
        WallDetectDist = 0.3f;
    }
    
    private void OnEnable()
    {
        PlayerControl.Enable();
        MoveInputValue = PlayerControl.Player.Move;
        RemoveAction();
        AddAction();
    }
    
    private void OnDisable()
    {
        MoveInputValue.Disable();
        RemoveAction();
    }

    private void AddAction()
    {
        PlayerControl.Player.Jump.performed += JumpAction;
        PlayerControl.Player.Dash.performed += DashAction;
        PlayerControl.Player.NormalAttack.performed += NormalAttackAction;
        PlayerControl.Player.FirstSkill.performed += FirstSkillAction;
        PlayerControl.Player.SecondSkill.performed += SecondSkillAction;
        PlayerControl.Player.ThirdSkill.performed += ThirdSkillAction;
        PlayerControl.Player.FourthSkill.performed += FourthSkillAction;
        PlayerControl.Player.InventoryWindow.performed += ToggleInventoryWindowAction;
        PlayerControl.Player.SkillWindow.performed += ToggleSkillWindowAction;
        PlayerControl.Player.FirstItemHotkey.performed += FirstItemHotkeyAction;
        PlayerControl.Player.SecondItemHotkey.performed += SecondItemHotkeyAction;
        PlayerControl.Player.ThirdItemHotkey.performed += ThirdItemHotkeyAction;
        PlayerControl.Player.FourthItemHotkey.performed += FourthItemHotkeyAction;
        PlayerControl.Player.FifthItemHotkey.performed += FifthItemHotkeyAction;
        PlayerControl.Player.Interaction.performed += InteractionAction;
        PlayerControl.Player.Esc.performed += EscAction;
        PlayerControl.UI.Esc.performed += CloseToUIAction;
        
        GI.Inst.ListenerManager.switchActionMap += SwitchActionMap;
        GI.Inst.ListenerManager.enablePlayerControl += EnablePlayerControl;
        GI.Inst.ListenerManager.disablePlayerControl += DisablePlayerControl;
        GI.Inst.ListenerManager.isEnablePlayerControl += IsEnablePlayerControl;
    }
    
    private void RemoveAction()
    {
        PlayerControl.Player.Jump.performed -= JumpAction;
        PlayerControl.Player.Dash.performed -= DashAction;
        PlayerControl.Player.NormalAttack.performed -= NormalAttackAction;
        PlayerControl.Player.FirstSkill.performed -= FirstSkillAction;
        PlayerControl.Player.SecondSkill.performed -= SecondSkillAction;
        PlayerControl.Player.ThirdSkill.performed -= ThirdSkillAction;
        PlayerControl.Player.FourthSkill.performed -= FourthSkillAction;
        PlayerControl.Player.InventoryWindow.performed -= ToggleInventoryWindowAction;
        PlayerControl.Player.SkillWindow.performed -= ToggleSkillWindowAction;
        PlayerControl.Player.FirstItemHotkey.performed -= FirstItemHotkeyAction;
        PlayerControl.Player.SecondItemHotkey.performed -= SecondItemHotkeyAction;
        PlayerControl.Player.ThirdItemHotkey.performed -= ThirdItemHotkeyAction;
        PlayerControl.Player.FourthItemHotkey.performed -= FourthItemHotkeyAction;
        PlayerControl.Player.FifthItemHotkey.performed -= FifthItemHotkeyAction;
        PlayerControl.Player.Interaction.performed -= InteractionAction;
        PlayerControl.Player.Esc.performed -= EscAction;
        PlayerControl.UI.Esc.performed -= CloseToUIAction;
        
        GI.Inst.ListenerManager.switchActionMap -= SwitchActionMap;
        GI.Inst.ListenerManager.enablePlayerControl -= EnablePlayerControl;
        GI.Inst.ListenerManager.disablePlayerControl -= DisablePlayerControl;
        GI.Inst.ListenerManager.isEnablePlayerControl -= IsEnablePlayerControl;
    }

    void Update()
    {
        MoveDir = MoveInputValue.ReadValue<Vector2>();
        normalAttackReserveTimer -= Time.deltaTime;
        if (normalAttackReserveTimer < 0f)
        {
            IsReserveNormalAttack = false;
        }

        if (bIsCharging)
        {
            if (Input.GetKey(chargeKeyCode))
            {
                chargeTimer += Time.deltaTime;
                CurrentChargeAmount = Mathf.Clamp(CurrentChargeAmount + (Time.deltaTime / maxChargingTime) * 100f, 0f, 100f);
                
                if (chargeTimer >= maxChargingTime)
                {
                    bChargeCompleted = true;
                }
            }
            else
            {
                bIsCharging = false;
            }
        }
    }

    protected override void TransitionMoveState()
    {
        TransitionState(Define.EPlayerState.Move);
    }
    
    private void InitCharge(KeyCode keycode, float skillMaxChargeTime)
    {
        bIsCharging = true;
        maxChargingTime = skillMaxChargeTime;
        chargeKeyCode = keycode;
        bChargeCompleted = false;
        chargeTimer = 0f;
        CurrentChargeAmount = 0f;
    }
    
    private void ChargeSkill(SO_Skill skill, string keyString)
    {
        SO_ActiveSkill activeSkill = skill as SO_ActiveSkill;
        if (activeSkill && activeSkill.isChargeSkill)
        {
            InitCharge(KeyToKeyCode(keyString), activeSkill.maxChargingTime);
        }
    }
    
    private void TransitionState(Define.EPlayerState playerState)
    {
        ControlledPlayer.TransitionState(playerState);
    }

    public void DaggerUltEnd()
    {
        ControlledPlayer.TransitionState(Define.EPlayerState.Idle);
    }

    public void PiercingArrowEnd()
    {
        IsCharging = false;
        ChargeCompleted = false;
    }

    private void EnablePlayerControl()
    {
        PlayerControl.Enable();
        PlayerControl.UI.Disable();
    }
    
    private void DisablePlayerControl()
    {
        PlayerControl.Disable();
    }

    private bool IsEnablePlayerControl() => PlayerControl.Player.enabled;

    #region Wrapper

    public static KeyCode KeyToKeyCode(string key, KeyCode unknownKey = KeyCode.None)
    {
        switch (key)
        {
            case "None":              return KeyCode.None;
            case "Space":             return KeyCode.Space;
            case "Enter":             return KeyCode.Return;
            case "Tab":               return KeyCode.Tab;
            case "Backquote":         return KeyCode.BackQuote;
            case "Quote":             return KeyCode.Quote;
            case "Semicolon":         return KeyCode.Semicolon;
            case "Comma":             return KeyCode.Comma;
            case "Period":            return KeyCode.Period;
            case "Slash":             return KeyCode.Slash;
            case "Backslash":         return KeyCode.Backslash;
            case "LeftBracket":       return KeyCode.LeftBracket;
            case "RightBracket":      return KeyCode.RightBracket;
            case "Minus":             return KeyCode.Minus;
            case "Equals":            return KeyCode.Equals;
            case "A":                 return KeyCode.A;
            case "B":                 return KeyCode.B;
            case "C":                 return KeyCode.C;
            case "D":                 return KeyCode.D;
            case "E":                 return KeyCode.E;
            case "F":                 return KeyCode.F;
            case "G":                 return KeyCode.G;
            case "H":                 return KeyCode.H;
            case "I":                 return KeyCode.I;
            case "J":                 return KeyCode.J;
            case "K":                 return KeyCode.K;
            case "L":                 return KeyCode.L;
            case "M":                 return KeyCode.M;
            case "N":                 return KeyCode.N;
            case "O":                 return KeyCode.O;
            case "P":                 return KeyCode.P;
            case "Q":                 return KeyCode.Q;
            case "R":                 return KeyCode.R;
            case "S":                 return KeyCode.S;
            case "T":                 return KeyCode.T;
            case "U":                 return KeyCode.U;
            case "V":                 return KeyCode.V;
            case "W":                 return KeyCode.W;
            case "X":                 return KeyCode.X;
            case "Y":                 return KeyCode.Y;
            case "Z":                 return KeyCode.Z;
            case "Digit1":            return KeyCode.Alpha1;
            case "Digit2":            return KeyCode.Alpha2;
            case "Digit3":            return KeyCode.Alpha3;
            case "Digit4":            return KeyCode.Alpha4;
            case "Digit5":            return KeyCode.Alpha5;
            case "Digit6":            return KeyCode.Alpha6;
            case "Digit7":            return KeyCode.Alpha7;
            case "Digit8":            return KeyCode.Alpha8;
            case "Digit9":            return KeyCode.Alpha9;
            case "Digit0":            return KeyCode.Alpha0;
            case "LeftShift":         return KeyCode.LeftShift;
            case "RightShift":        return KeyCode.RightShift;
            case "LeftAlt":           return KeyCode.LeftAlt;
            case "RightAlt":          return KeyCode.RightAlt;
            case "LeftCtrl":          return KeyCode.LeftControl;
            case "RightCtrl":         return KeyCode.RightControl;
            case "LeftCommand":       return KeyCode.LeftCommand;
            case "RightCommand":      return KeyCode.RightCommand;
            case "ContextMenu":       return unknownKey;
            case "Escape":            return KeyCode.Escape;
            case "LeftArrow":         return KeyCode.LeftArrow;
            case "RightArrow":        return KeyCode.RightArrow;
            case "UpArrow":           return KeyCode.UpArrow;
            case "DownArrow":         return KeyCode.DownArrow;
            case "Backspace":         return KeyCode.Backspace;
            case "PageDown":          return KeyCode.PageDown;
            case "PageUp":            return KeyCode.PageUp;
            case "Home":              return KeyCode.Home;
            case "End":               return KeyCode.End;
            case "Insert":            return KeyCode.Insert;
            case "Delete":            return KeyCode.Delete;
            case "CapsLock":          return KeyCode.CapsLock;
            case "NumLock":           return KeyCode.Numlock;
            case "PrintScreen":       return KeyCode.Print;
            case "ScrollLock":        return KeyCode.ScrollLock;
            case "Pause":             return KeyCode.Pause;
            case "NumpadEnter":       return KeyCode.KeypadEnter;
            case "NumpadDivide":      return KeyCode.KeypadDivide;
            case "NumpadMultiply":    return KeyCode.KeypadMultiply;
            case "NumpadPlus":        return KeyCode.KeypadPlus;
            case "NumpadMinus":       return KeyCode.KeypadMinus;
            case "NumpadPeriod":      return KeyCode.KeypadPeriod;
            case "NumpadEquals":      return KeyCode.KeypadEquals;
            case "Numpad0":           return KeyCode.Keypad0;
            case "Numpad1":           return KeyCode.Keypad1;
            case "Numpad2":           return KeyCode.Keypad2;
            case "Numpad3":           return KeyCode.Keypad3;
            case "Numpad4":           return KeyCode.Keypad4;
            case "Numpad5":           return KeyCode.Keypad5;
            case "Numpad6":           return KeyCode.Keypad6;
            case "Numpad7":           return KeyCode.Keypad7;
            case "Numpad8":           return KeyCode.Keypad8;
            case "Numpad9":           return KeyCode.Keypad9;
            case "F1":                return KeyCode.F1;
            case "F2":                return KeyCode.F2;
            case "F3":                return KeyCode.F3;
            case "F4":                return KeyCode.F4;
            case "F5":                return KeyCode.F5;
            case "F6":                return KeyCode.F6;
            case "F7":                return KeyCode.F7;
            case "F8":                return KeyCode.F8;
            case "F9":                return KeyCode.F9;
            case "F10":               return KeyCode.F10;
            case "F11":               return KeyCode.F11;
            case "F12":               return KeyCode.F12;
            case "OEM1":              return unknownKey;
            case "OEM2":              return unknownKey;
            case "OEM3":              return unknownKey;
            case "OEM4":              return unknownKey;
            case "OEM5":              return unknownKey;
            case "IMESelected":       return unknownKey;
            default:                    return unknownKey;
        }
    }
    
    public string GetBindingKeyString(EBindKeyType bindKeyType)
    {
        switch (bindKeyType)
        {
            case EBindKeyType.Jump:
                return PlayerControl.Player.Jump.GetBindingDisplayString();
            case EBindKeyType.Dash:
                return PlayerControl.Player.Dash.GetBindingDisplayString();
            case EBindKeyType.NormalAttack:
                return PlayerControl.Player.NormalAttack.GetBindingDisplayString();
            case EBindKeyType.FirstSkill:
                return PlayerControl.Player.FirstSkill.GetBindingDisplayString();
            case EBindKeyType.SecondSkill:
                return PlayerControl.Player.SecondSkill.GetBindingDisplayString();
            case EBindKeyType.ThirdSkill:
                return PlayerControl.Player.ThirdSkill.GetBindingDisplayString();
            case EBindKeyType.FourthSkill:
                return PlayerControl.Player.FourthSkill.GetBindingDisplayString();
            case EBindKeyType.InventoryWindow:
                return PlayerControl.Player.InventoryWindow.GetBindingDisplayString();
            case EBindKeyType.SkillWindow:
                return PlayerControl.Player.SkillWindow.GetBindingDisplayString();
            case EBindKeyType.FirstItemHotkey:
                return PlayerControl.Player.FirstItemHotkey.GetBindingDisplayString();
            case EBindKeyType.SecondItemHotkey:
                return PlayerControl.Player.SecondItemHotkey.GetBindingDisplayString();
            case EBindKeyType.ThirdItemHotkey:
                return PlayerControl.Player.ThirdItemHotkey.GetBindingDisplayString();
            case EBindKeyType.FourthItemHotkey:
                return PlayerControl.Player.FourthItemHotkey.GetBindingDisplayString();
            case EBindKeyType.FifthItemHotkey:
                return PlayerControl.Player.FifthItemHotkey.GetBindingDisplayString();
            case EBindKeyType.Interaction:
                return PlayerControl.Player.Interaction.GetBindingDisplayString();
        }
        
        return "";
    }

    #endregion //wrapper

    #region Rebind

    private string ConvertInputKey(string path)
    {
        string extractedKey = "";
        string[] pathParts = path.Split('/');
        if (pathParts.Length > 1)
        {
            extractedKey = pathParts[pathParts.Length - 1];
        }

        return extractedKey;
    }

    private string ConvertPlayerControlKey(string path)
    {
        string extractedKey = "";

        string[] pathParts = path.Split('/');
        if (pathParts.Length > 1)
        {
            extractedKey = pathParts[pathParts.Length - 1];
        }

        int lastSlashIndex = path.LastIndexOf('/');
        if (lastSlashIndex >= 0 && lastSlashIndex < path.Length - 1)
        {
            extractedKey = path.Substring(lastSlashIndex + 1);
        }

        return extractedKey;
    }

    private void StartInteractiveRebind(InputAction actionToRebind)
    {
        actionToRebind.Disable();
        
        rebindingOperation = actionToRebind.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Keyboard>/p")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f) //0.1초 기다림
            .OnPotentialMatch(operation =>
            {
                string inputKey = ConvertInputKey(operation.selectedControl.path); 
        
                foreach (InputAction inputAction in PlayerControl)
                {
                    foreach (InputBinding binding in inputAction.bindings) 
                    {
                        string playerControlKey = ConvertPlayerControlKey(binding.effectivePath);
        
                        if (inputKey == playerControlKey)
                        {
                            ReplaceBinding(actionToRebind, binding, inputAction);
                            break;
                        }
                    }
                }
            })
            .OnComplete(operation =>
            {
                RebindCompleted();
                actionToRebind.Enable();
            });
        
        rebindingOperation.Start();
    }

    private void ReplaceBinding(InputAction actionToRebind, InputBinding binding, InputAction inputAction)
    {
        foreach (InputBinding inputBinding in actionToRebind.bindings)
        {
            InputBinding newBinding = new InputBinding(inputBinding.path, binding.action,
                binding.groups, binding.processors, binding.interactions, binding.name);
          
            inputAction.ChangeBinding(binding).Erase();
            inputAction.AddBinding(newBinding);
        }
    }

    private void RebindCompleted()
    {
        rebindingOperation.Dispose();
        GI.Inst.SaveBindKeyData();
        GI.Inst.UIManager.RefreshBindKeyUI();
        GI.Inst.UIManager.RefreshHotKeyMainUI(); //HotkeyBar Refresh
    }
    
     public void RebindKey(EBindKeyType bindKeyType)
    {
        switch (bindKeyType)
        {
            case EBindKeyType.Jump:
                StartInteractiveRebind(PlayerControl.Player.Jump);
                break;
            case EBindKeyType.Dash:
                StartInteractiveRebind(PlayerControl.Player.Dash);
                break;
            case EBindKeyType.FirstSkill:
                StartInteractiveRebind(PlayerControl.Player.FirstSkill);
                break;
            case EBindKeyType.SecondSkill:
                StartInteractiveRebind(PlayerControl.Player.SecondSkill);
                break;
            case EBindKeyType.ThirdSkill:
                StartInteractiveRebind(PlayerControl.Player.ThirdSkill);
                break;
            case EBindKeyType.FourthSkill:
                StartInteractiveRebind(PlayerControl.Player.FourthSkill);
                break;
            case EBindKeyType.FirstItemHotkey:
                StartInteractiveRebind(PlayerControl.Player.FirstItemHotkey);
                break;
            case EBindKeyType.SecondItemHotkey:
                StartInteractiveRebind(PlayerControl.Player.SecondItemHotkey);
                break;
            case EBindKeyType.ThirdItemHotkey:
                StartInteractiveRebind(PlayerControl.Player.ThirdItemHotkey);
                break;
            case EBindKeyType.FourthItemHotkey:
                StartInteractiveRebind(PlayerControl.Player.FourthItemHotkey);
                break;
            case EBindKeyType.FifthItemHotkey:
                StartInteractiveRebind(PlayerControl.Player.FifthItemHotkey);
                break;
            case EBindKeyType.InventoryWindow:
                StartInteractiveRebind(PlayerControl.Player.InventoryWindow);
                break;
            case EBindKeyType.SkillWindow:
                StartInteractiveRebind(PlayerControl.Player.SkillWindow);
                break;
            case EBindKeyType.Interaction:
                StartInteractiveRebind(PlayerControl.Player.Interaction);
                break;
            case EBindKeyType.NormalAttack:
                StartInteractiveRebind(PlayerControl.Player.NormalAttack);
                break;
        }
    }

    #endregion //Rebind

    #region Action

    private void JumpAction(InputAction.CallbackContext context)
    {
        if (CheckStateCanJump())
        {
            TransitionState(Define.EPlayerState.InAir);
        }
        else if (CheckStateCanWallJump())
        {
            TransitionState(Define.EPlayerState.WallJump);
        }
    }
    
    private void NormalAttackAction(InputAction.CallbackContext context)
    {
        if (CheckStateCanNormalAttack())
        {
            AttackDir = CurrentDir;
            if (GI.Inst.ListenerManager.GetEquippedWeaponType() == SO_Item.EWeaponType.Dagger)
                TransitionState(Define.EPlayerState.DaggerNormalAttack);
            else if (GI.Inst.ListenerManager.GetEquippedWeaponType() == SO_Item.EWeaponType.Axe)
                TransitionState(Define.EPlayerState.AxeNormalAttack);
            else if (GI.Inst.ListenerManager.GetEquippedWeaponType() == SO_Item.EWeaponType.Bow)
                TransitionState(Define.EPlayerState.BowNormalAttack);
        }
        else if (ControlledPlayer.StateMachine.CurrentState.IsAttacking)
        {
            //선입력 0.3초 후에 풀리게
            IsReserveNormalAttack = true;
            normalAttackReserveTimer = normalAttackReserveDuration;
        }
    }
    
    private void FirstSkillAction(InputAction.CallbackContext context)
    {
        if (!CanUseSkill(EActiveSkillOrder.First)) return;
        
        AttackDir = CurrentDir;
        
        SO_ActiveSkill skill = (SO_ActiveSkill)GI.Inst.ListenerManager.GetSkill(EActiveSkillOrder.First);
        
        if (context.control is KeyControl keyControl)
        {
            string keyString = keyControl.displayName;
            ChargeSkill(skill, keyString);
        }
        
        TransitionState(skill.skillState);
    }
    

    private void SecondSkillAction(InputAction.CallbackContext context)
    {
        if (!CanUseSkill(EActiveSkillOrder.Second)) return;
        
        AttackDir = CurrentDir;
        
        SO_ActiveSkill skill = (SO_ActiveSkill)GI.Inst.ListenerManager.GetSkill(EActiveSkillOrder.Second);

        if (context.control is KeyControl keyControl)
        {
            string keyString = keyControl.displayName;
            ChargeSkill(skill, keyString);
        }

        TransitionState(skill.skillState);
    }
    
    private void ThirdSkillAction(InputAction.CallbackContext context)
    {
        if (!CanUseSkill(EActiveSkillOrder.Third)) return;
        
        AttackDir = CurrentDir;
      
        
        SO_ActiveSkill skill = (SO_ActiveSkill)GI.Inst.ListenerManager.GetSkill(EActiveSkillOrder.Third);
        
        if (context.control is KeyControl keyControl)
        {
            string keyString = keyControl.displayName;
            ChargeSkill(skill, keyString);
        }
        
        TransitionState(skill.skillState);
    }
    
    private void FourthSkillAction(InputAction.CallbackContext context)
    {
        if (!CanUseSkill(EActiveSkillOrder.Fourth)) return;
        
        AttackDir = CurrentDir;
        
        SO_ActiveSkill skill = (SO_ActiveSkill)GI.Inst.ListenerManager.GetSkill(EActiveSkillOrder.Fourth);

        if (context.control is KeyControl keyControl)
        {
            string keyString = keyControl.displayName;
            ChargeSkill(skill, keyString);
        }
        
        TransitionState(skill.skillState);
    }
    
    private void DashAction(InputAction.CallbackContext context)
    {
        if (!CanDash(EActiveSkillOrder.Fifth)) return;

        if (CheckStateCanGroundSlide())
        {
            TransitionState(Define.EPlayerState.GroundSliding);
        }
        else
        {
            TransitionState(Define.EPlayerState.Dash);
        }
    }

    private void FirstItemHotkeyAction(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(SO_Item.EItemHotkeyOrder.First);
    }
    
    private void SecondItemHotkeyAction(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(SO_Item.EItemHotkeyOrder.Second);
    }
    
    void ThirdItemHotkeyAction(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(SO_Item.EItemHotkeyOrder.Third);
    }
    
    private void FourthItemHotkeyAction(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(SO_Item.EItemHotkeyOrder.Fourth);
    }
    
    private void FifthItemHotkeyAction(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(SO_Item.EItemHotkeyOrder.Fifth);
    }
    
    private void EscAction(InputAction.CallbackContext context)
    {
        GI.Inst.UIManager.ToggleEsc();
    }
    
    private void ToggleInventoryWindowAction(InputAction.CallbackContext context)
    {
        GI.Inst.UIManager.ToggleMainMenu(Define.EMainMenuType.Inventory);
    }
    
    private void ToggleSkillWindowAction(InputAction.CallbackContext context)
    {
        GI.Inst.UIManager.ToggleMainMenu(Define.EMainMenuType.Skill);
    }
    
    private void InteractionAction(InputAction.CallbackContext context)
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 3f, LayerMask.GetMask("Merchant"));
        if (col)
        {
            Merchant merchant = col.GetComponent<Merchant>();
            if (merchant)
            {
                GI.Inst.UIManager.VisibleMerchantUI(EMerchantType.Buy, merchant);
            }
        }
    }
    
    private void CloseToUIAction(InputAction.CallbackContext context)
    {
        GI.Inst.UIManager.ClosePopup();
    }

    #endregion //Action

    #region CheckState

    private bool IsJumpingState()
    {
        bool condition  = CheckCurrentState(Define.EPlayerState.InAir) ||
                          CheckCurrentState(Define.EPlayerState.Falling) ||
                          CheckCurrentState(Define.EPlayerState.JumpEnd);
        return condition;
    }

    private bool CheckStateCanJump()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.Idle) || CheckCurrentState(Define.EPlayerState.Move);
        return condition;
    }
    
    public bool CheckStateCanWallJump()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.WallSliding);
        return condition;
    }

    private bool CheckStateCanGroundSlide()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.Move);
        return condition;
    }
    
    public bool CheckStateCanWallSlide()
    {
        if (IsDead()) return false;
        bool condition = ControlledPlayer.StateMachine.CurrentState != ControlledPlayer.GetState(Define.EPlayerState.WallJump);
     
        return condition;
    }

    private bool CheckStateCanNormalAttack()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.Idle) ||
                         CheckCurrentState(Define.EPlayerState.Move);
        return condition;
    }
    
    protected override bool CheckStateCanMove()
    {
        if (IsDead()) return false;
        bool condition = CheckCurrentState(Define.EPlayerState.Idle);
        return condition;
    }
    
    public override bool IsWallDetect()
    {
        if (Physics2D.Raycast(wallDetectObject.transform.position, new Vector2(MoveDir.x, 0f), WallDetectDist, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }

    private bool CanDash(EActiveSkillOrder skillOrder)
    {
        if (IsDead()) return false;
        bool condition = (CheckCurrentState(Define.EPlayerState.Idle) || CheckCurrentState(Define.EPlayerState.Move) || IsJumpingState())
                         && GI.Inst.ListenerManager.IsSkillReady(skillOrder) && GI.Inst.ListenerManager.IsEquippedWeapon();
        
        return condition;
    }
    
    private bool CanUseSkill(EActiveSkillOrder skillOrder)
    {
        if (IsDead()) return false;
        bool condition = (CheckCurrentState(Define.EPlayerState.Idle) || CheckCurrentState(Define.EPlayerState.Move)) 
                         && GI.Inst.ListenerManager.IsSkillReady(skillOrder);
        return condition;
    }

    public bool CheckCurrentState(Define.EPlayerState playerState)
    {
        return ControlledPlayer.GetState(playerState) == ControlledPlayer.StateMachine.CurrentState;
    }

    private bool IsDead()
    {
        return CheckCurrentState(Define.EPlayerState.Dead);
    }

    #endregion //CheckState
    
}
