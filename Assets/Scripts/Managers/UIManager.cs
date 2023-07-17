using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class UIManager : MonoBehaviour
{
    #region delegate
    public Action<EActiveSkillOrder, float> setSkillCooltimeUI;
    public Action<EActiveSkillOrder> resetCooltimeUI;
    public Action refreshInventoryUI;
    public Func<UI_InventoryWrapper> getInventoryWrapper;
    public Action refreshGoldInvenCapacityUI;
    public Action refreshHotKeyMainUI;
    public Action refreshBindKeyUI;
    public Action disableCategoryButtonCantEnhance;
    public Action enableCategoryButton;
    public Action<BaseWeapon> enableCanWeaponMat;
    public Action<List<Sprite>> refreshSkillHotkeyMainUI;
    public Action<Item.EItemHotkeyOrder, Item> refreshItemHotkeyUI;
    public Action<Item.EItemHotkeyOrder> clearItemHotkeyUI;
    public Action<Item.EItemCategory> onClickCategoryButton;
    public Action activeAllInvenCategoryBtn;
    public Action<List<SO_ActiveSkill>> refreshActiveSkillSlots;
    public Action clearActiveSkillSlots;
    public Action clearActiveSkillHotkeySlots;
    public Action<PassiveSkill_ShortVer> checkEquippedPassive;
    public Action<bool> blinkEquipPassiveSkillSlot;
    public Action<SO_PassiveSkill> setEquipPassive;
    public Action<float> updateFillAmount;
    public Action refreshCraftLines;
    public Action<string> setCraftResult;
    public Action clearCraftResult;
    public Action<Item> setSameEquipment;
    #endregion
    
    private UI_Main MainUI { get; set; }
    
    public UI_MainMenu MainMenuUI { get; private set; }
    private UI_ItemTooltip ItemTooltipUI { get; set; }
    private UI_SkillTooltip SkillTooltipUI { get; set; }
    private UI_Merchant_Menu MerchantMenuUI { get; set; }
    private UI_Option OptionUI { get; set; }
    private UI_Esc EscUI { get; set; }

    private Stack<UI_Popup> Popups { get; set; } = new Stack<UI_Popup>();

    private Define.EMainMenuType CurrentActiveMainMenu { get; set; }
    
    public void Init()
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_MainMenu", transform);
        MainMenuUI = go.GetComponent<UI_MainMenu>();
        MainMenuUI.InitOnce();
        go.SetActive(false);
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Main", transform);
        MainUI = go.GetComponent<UI_Main>();
        MainUI.InitOnce();

        go = GI.Inst.ResourceManager.Instantiate("UI_ItemTooltip", transform);
        ItemTooltipUI = go.GetComponent<UI_ItemTooltip>();
        go.SetActive(false);
        
        go = GI.Inst.ResourceManager.Instantiate("UI_SkillTooltip", transform);
        SkillTooltipUI = go.GetComponent<UI_SkillTooltip>();
        go.SetActive(false);
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_Menu", transform);
        MerchantMenuUI = go.GetComponent<UI_Merchant_Menu>();
        MerchantMenuUI.InitOnce();
        go.SetActive(false);
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Option", transform);
        OptionUI = go.GetComponent<UI_Option>();
        OptionUI.InitOnce();
        go.SetActive(false);

        go = GI.Inst.ResourceManager.Instantiate("UI_Esc", transform);
        EscUI = go.GetComponent<UI_Esc>();
        go.SetActive(false);

    }

    public void ToggleMainMenu(Define.EMainMenuType menuType)
    {
        if (CurrentActiveMainMenu == menuType)
            ClosePopup();
        else
            VisibleMainMenuSetting(menuType);
        
        InvisibleItemTooltip();
    }
    
    public void ToggleEsc()
    {
        if (EscUI.gameObject.activeSelf)
            ClosePopup();
        else
            VisibleEsc();
    }
    
    public void VisibleMainMenuSetting(Define.EMainMenuType menuType)
    {
        CurrentActiveMainMenu = menuType;
        GI.Inst.CinemachineTarget.DeactivateCamera();
        if (!MainMenuUI.gameObject.activeSelf)
            Popups.Push(MainMenuUI); 
        MainMenuUI.gameObject.SetActive(true);
        MainMenuUI.OnVisible(menuType);
        
        GI.Inst.ListenerManager.SwitchActionMap(true);
    }

    public void VisibleEsc()
    {
        GI.Inst.CinemachineTarget.DeactivateCamera();
        Popups.Push(EscUI); 
        EscUI.gameObject.SetActive(true);
        GI.Inst.ListenerManager.SwitchActionMap(true);
    }

    //장착, 장착해제, 버리기 등
    public void VisibleInventoryPopup(EInventoryPopupType type, Item item, Vector3 pos)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_Popup", transform);
        UI_Inven_Popup invenPopupUI = go.GetComponent<UI_Inven_Popup>();
        invenPopupUI.Init(type, item, pos);
        Popups.Push(invenPopupUI);
        GI.Inst.ListenerManager.SwitchActionMap(true);
    }

    public void VisibleTBPopup(EThrowawayBuyPopupType type, Item item)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_TB_Popup", transform);
        UI_ThrowawayBuyPopup throwawayBuyPopupUI = go.GetComponent<UI_ThrowawayBuyPopup>();
        throwawayBuyPopupUI.Init(type, item);
        Popups.Push(throwawayBuyPopupUI);
        GI.Inst.ListenerManager.SwitchActionMap(true);
    }

    public void VisibleEnhancePopup(Item item)
    {
        BaseWeapon weapon = item as BaseWeapon;
        if (weapon)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_EnhancePopup", transform);
            UI_Popup_Enhance enhancePopupUI = go.GetComponent<UI_Popup_Enhance>();
            enhancePopupUI.InitOnce(weapon);
            
            Popups.Push(enhancePopupUI);
            GI.Inst.ListenerManager.SwitchActionMap(true);
        }
    }
    
    public void VisibleMerchantUI(EMerchantType type, Merchant merchant)
    {
        GI.Inst.CinemachineTarget.DeactivateCamera();
        Popups.Push(MerchantMenuUI);
        MerchantMenuUI.gameObject.SetActive(true); 
        MerchantMenuUI.Open(type, merchant);
        GI.Inst.ListenerManager.SwitchActionMap(true);
    }

    public void VisibleOption(Define.EOptionType type, bool isMainMenu)
    {
        if (GI.Inst.CinemachineTarget)
            GI.Inst.CinemachineTarget.DeactivateCamera();
        Popups.Push(OptionUI);
        OptionUI.gameObject.SetActive(true);
        OptionUI.OnVisible(type, isMainMenu);
        GI.Inst.ListenerManager.SwitchActionMap(true);
    }
    
    public void VisibleOptionCuzPressedHolderButton(Define.EOptionType optionType)
    {
        if (GI.Inst.CinemachineTarget)
            GI.Inst.CinemachineTarget.DeactivateCamera();
        OptionUI.OnVisible(optionType);
    }
    
    
    public void InvisibleEsc(bool isInGame)
    {
        GI.Inst.CinemachineTarget.ActivateCamera();
        Popups.Pop(); 
        EscUI.gameObject.SetActive(false);
        GI.Inst.ListenerManager.SwitchActionMap(!isInGame);
    }

    public void ClosePopup()
    {
        Popups.Pop().Close();

        if (Popups.Count <= 0)
        {
            CurrentActiveMainMenu = Define.EMainMenuType.None;
            if (GI.Inst.CinemachineTarget)
                GI.Inst.CinemachineTarget.ActivateCamera();
            GI.Inst.ListenerManager.SwitchActionMap(false);
        }
    }
    

    public void PressedCloseButtonMainMenu()
    {
        ToggleMainMenu(CurrentActiveMainMenu);
    }
    
    public void SetHpBar(float ratio)
    {
        MainUI.SetHpBar(ratio);
    }
    
    public void SetEffectSlot(EDurationEffectId effectId, Sprite icon)
    {
        MainUI.SetEffectSlot(effectId, icon);
    }

    public void SetPassiveCooltimeSlot(Define.ESkillId skillId, Sprite icon)
    {
        MainUI.SetPassiveCooltimeSlot(skillId, icon);
    }

    public void SetVisibleBindKeyPopup(bool isVisible)
    {
        OptionUI.SetVisibleBindKeyPopup(isVisible);
    }
    
    public ColorBlock GetPressedButtonPreset(float normalColor)
    {
        return new ColorBlock
        {
            normalColor = new Color(normalColor/255f, normalColor/255f, normalColor/255f, 1f),
            highlightedColor = new Color(176f/255f, 176f/255f, 176f/255f, 1f),
            pressedColor = new Color(176f/255f, 176f/255f, 176f/255f, 1f),
            selectedColor = new Color(normalColor/255f, normalColor/255f, normalColor/255f, 1f),
            colorMultiplier = 1f
        };
    }

    public void SpawnDamageText(Define.EDamageTextType damageTextType, Vector3 spawnLocation, float damage)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("DamageText", spawnLocation, Quaternion.identity);
        UI_Main_DamageText damageText = go.GetComponentInChildren<UI_Main_DamageText>();
        damageText.Init(damageTextType, damage);
    }

    public RectTransform GetMainUIRectTransform()
    {
        return MainUI.rectTransform;
    }
    #region Tooltip

    public void VisibleItemTooltip(Item item, Vector3 slotPos, int pivot)
    {
        ItemTooltipUI.gameObject.SetActive(true);
        ItemTooltipUI.Init(item, slotPos, pivot);
    }

    public void InvisibleItemTooltip()
    {
        if (ItemTooltipUI.gameObject.activeSelf)
            ItemTooltipUI.gameObject.SetActive(false);
    }
    
    public void VisibleActiveSkillTooltip(ActiveSkill_ShortVer skill, Vector3 slotPos, int pivot)
    {
        SkillTooltipUI.gameObject.SetActive(true);
        SkillTooltipUI.Init(skill, slotPos, pivot);
    }
    
    public void VisiblePassiveSkillTooltip(PassiveSkill_ShortVer skill, Vector3 slotPos, int pivot)
    {
        SkillTooltipUI.gameObject.SetActive(true);
        SkillTooltipUI.Init(skill, slotPos, pivot);
    }

    public void InvisibleSkillTooltip()
    {
        if (SkillTooltipUI.gameObject.activeSelf)
            SkillTooltipUI.gameObject.SetActive(false);
    }

    #endregion

    #region Callback

    public void SetSkillCooltimeUI(EActiveSkillOrder order, float cooltime)
    {
        setSkillCooltimeUI?.Invoke(order, cooltime);
    }

    public void ResetCooltimeUI(EActiveSkillOrder order)
    {
        resetCooltimeUI?.Invoke(order);
    }

    public void RefreshInventoryUI()
    {
        refreshInventoryUI?.Invoke();
    }

    public UI_InventoryWrapper GetInventoryWrapper()
    {
        return getInventoryWrapper.Invoke();
    }

    public void RefreshGoldInvenCapacityUI()
    {
        refreshGoldInvenCapacityUI?.Invoke();
    }

    public void RefreshHotKeyMainUI()
    {
        refreshHotKeyMainUI?.Invoke();
    }

    public void RefreshBindKeyUI()
    {
        refreshBindKeyUI?.Invoke();
    }

    public void RefreshActiveSkillUI(List<SO_ActiveSkill> skills)
    {
        refreshActiveSkillSlots?.Invoke(skills);
    }

    public void ClearActiveSkillSlots()
    {
        clearActiveSkillSlots?.Invoke();
    }

    public void ClearActiveSkillHotkeySlots()
    {
        clearActiveSkillHotkeySlots?.Invoke();
    }
    
    public void RefreshPassiveSkillUI(List<SO_PassiveSkill> skills)
    {
        MainMenuUI.RefreshPassiveSkillUI(skills);
    }

    public void RefreshSkillHotkeyMainUI(List<Sprite> icons)
    {
        refreshSkillHotkeyMainUI?.Invoke(icons);
    }
    
    public void RefreshItemHotkeyUI(Item.EItemHotkeyOrder order, Item item)
    {
        refreshItemHotkeyUI?.Invoke(order, item);
    }

    public void ClearItemHotkeyUI(Item.EItemHotkeyOrder order)
    {
        clearItemHotkeyUI?.Invoke(order);
    }
    
    public void ActiveAllInvenCategoryBtn()
    {
        activeAllInvenCategoryBtn?.Invoke();
    }

    public void OnClickCategoryButton(Item.EItemCategory itemCategory)
    {
        onClickCategoryButton?.Invoke(itemCategory);
        InvisibleItemTooltip();
    }

    public void CheckEquippedPassive(PassiveSkill_ShortVer passiveSkill)
    {
        checkEquippedPassive?.Invoke(passiveSkill);
    }

    public void BlinkEquipPassiveSkillSlot(bool cond)
    {
        blinkEquipPassiveSkillSlot?.Invoke(cond);
    }

    public void SetEquipPassive(SO_PassiveSkill passiveSkill)
    {
        setEquipPassive?.Invoke(passiveSkill);
    }

    public void UpdateFillAmount(float amount)
    {
        updateFillAmount?.Invoke(amount);
    }

    public void RefreshCraftLines()
    {
        refreshCraftLines?.Invoke();
    }

    public void SetCraftResult(string itemName)
    {
        setCraftResult?.Invoke(itemName);
    }

    public void ClearCraftResult()
    {
        clearCraftResult?.Invoke();
    }

    public void SetSameEquipment(Item weapon)
    {
        setSameEquipment?.Invoke(weapon);
    }

    public void DisableCategoryButtonCantEnhance()
    {
        disableCategoryButtonCantEnhance?.Invoke();
    }

    public void EnableCategoryButton()
    {
        enableCategoryButton?.Invoke();
    }

    public void EnableCanWeaponMat(BaseWeapon weapon)
    {
        enableCanWeaponMat?.Invoke(weapon);
    }

    #endregion
    
    
}
