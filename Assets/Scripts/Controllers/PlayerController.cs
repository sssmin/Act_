using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class PlayerController : BaseController
{
    private PlayerControl PlayerControl { get; set; }
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
        PlayerControl.Player.Jump.performed += Jump;
        PlayerControl.Player.Dash.performed += Dash;
        PlayerControl.Player.NormalAttack.performed += NormalAttack;
        PlayerControl.Player.FirstSkill.performed += FirstSkill;
        PlayerControl.Player.SecondSkill.performed += SecondSkill;
        PlayerControl.Player.ThirdSkill.performed += ThirdSkill;
        PlayerControl.Player.FourthSkill.performed += FourthSkill;
        PlayerControl.Player.InventoryWindow.performed += ToggleInventory;
        PlayerControl.Player.SkillWindow.performed += ToggleSkill;
        PlayerControl.Player.FirstItemHotkey.performed += FirstItemHotkey;
        PlayerControl.Player.SecondItemHotkey.performed += SecondItemHotkey;
        PlayerControl.Player.ThirdItemHotkey.performed += ThirdItemHotkey;
        PlayerControl.Player.FourthItemHotkey.performed += FourthItemHotkey;
        PlayerControl.Player.FifthItemHotkey.performed += FifthItemHotkey;
        PlayerControl.Player.Interaction.performed += Interaction;
    }

    private void OnDisable()
    {
        MoveInputValue.Disable();
        PlayerControl.Player.Jump.performed -= Jump;
        PlayerControl.Player.Dash.performed -= Dash;
        PlayerControl.Player.NormalAttack.performed -= NormalAttack;
        PlayerControl.Player.FirstSkill.performed -= FirstSkill;
        PlayerControl.Player.SecondSkill.performed -= SecondSkill;
        PlayerControl.Player.ThirdSkill.performed -= ThirdSkill;
        PlayerControl.Player.FourthSkill.performed -= FourthSkill;
        PlayerControl.Player.InventoryWindow.performed -= ToggleInventory;
        PlayerControl.Player.SkillWindow.performed -= ToggleSkill;
        PlayerControl.Player.FirstItemHotkey.performed -= FirstItemHotkey;
        PlayerControl.Player.SecondItemHotkey.performed -= SecondItemHotkey;
        PlayerControl.Player.ThirdItemHotkey.performed -= ThirdItemHotkey;
        PlayerControl.Player.FourthItemHotkey.performed -= FourthItemHotkey;
        PlayerControl.Player.FifthItemHotkey.performed -= FifthItemHotkey;
        PlayerControl.Player.Interaction.performed -= Interaction;
    }

    void Start()
    {
        InitDelegate();
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

    void Jump(InputAction.CallbackContext context)
    {
        if (CheckStateCanJump())
        {
            TransitionState(Define.EPlayerState.InAir);
        }
    }

    

    bool IsJumpingState()
    {
        bool condition  = CheckCurrentState(Define.EPlayerState.InAir) ||
                          CheckCurrentState(Define.EPlayerState.Falling) ||
                          CheckCurrentState(Define.EPlayerState.JumpEnd);
        return condition;
    }
    
    void NormalAttack(InputAction.CallbackContext context)
    {
        if (CheckStateCanNormalAttack())
        {
            AttackDir = CurrentDir;
            if (GI.Inst.ListenerManager.GetEquippedWeaponType() == Item.EWeaponType.Dagger)
                TransitionState(Define.EPlayerState.DaggerNormalAttack);
            else if (GI.Inst.ListenerManager.GetEquippedWeaponType() == Item.EWeaponType.Axe)
                TransitionState(Define.EPlayerState.AxeNormalAttack);
            else if (GI.Inst.ListenerManager.GetEquippedWeaponType() == Item.EWeaponType.Bow)
                TransitionState(Define.EPlayerState.BowNormalAttack);
        }
        else if (ControlledPlayer.StateMachine.CurrentState.IsAttacking)
        {
            //선입력 0.3초 후에 풀리게
            IsReserveNormalAttack = true;
            normalAttackReserveTimer = normalAttackReserveDuration;
        }
    }

    void InitDelegate()
    {
        
    }
    
    bool CheckStateCanJump()
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

    bool CheckStateCanGroundSlide()
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

    bool CheckStateCanNormalAttack()
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

    protected override void TransitionMoveState()
    {
        TransitionState(Define.EPlayerState.Move);
    }
    
    public override bool IsWallDetect()
    {
        if (Physics2D.Raycast(wallDetectObject.transform.position, new Vector2(MoveDir.x, 0f), WallDetectDist, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }

    bool CanDash(EActiveSkillOrder skillOrder)
    {
        if (IsDead()) return false;
        bool condition = (CheckCurrentState(Define.EPlayerState.Idle) || CheckCurrentState(Define.EPlayerState.Move) || IsJumpingState()) 
                         
                         && GI.Inst.ListenerManager.IsSkillReady(skillOrder);
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

    bool IsDead()
    {
        return CheckCurrentState(Define.EPlayerState.Dead);
    }

    private void FirstSkill(InputAction.CallbackContext context)
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
    

    private void SecondSkill(InputAction.CallbackContext context)
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
    
    private void ThirdSkill(InputAction.CallbackContext context)
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
    
    private void FourthSkill(InputAction.CallbackContext context)
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
    
    void Dash(InputAction.CallbackContext context)
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

    void FirstItemHotkey(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(Item.EItemHotkeyOrder.First);
    }
    
    void SecondItemHotkey(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(Item.EItemHotkeyOrder.Second);
    }
    
    void ThirdItemHotkey(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(Item.EItemHotkeyOrder.Third);
    }
    
    void FourthItemHotkey(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(Item.EItemHotkeyOrder.Fourth);
    }
    
    void FifthItemHotkey(InputAction.CallbackContext context)
    {
        GI.Inst.ListenerManager.OnPressedItemHotkey(Item.EItemHotkeyOrder.Fifth);
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
    
    protected void TransitionState(Define.EPlayerState playerState)
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

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        GI.Inst.UIManager.ToggleMainMenu(Define.EMainMenuType.Inventory);
    }
    
    public void ToggleSkill(InputAction.CallbackContext context)
    {
        GI.Inst.UIManager.ToggleMainMenu(Define.EMainMenuType.Skill);
    }
    
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

    public void Interaction(InputAction.CallbackContext context)
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
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(wallDetectObject.transform.position, new Vector3(wallDetectObject.transform.position.x + wallDetectDist * MoveDir.x, wallDetectObject.transform.position.y, wallDetectObject.transform.position.z));
    // }
}
