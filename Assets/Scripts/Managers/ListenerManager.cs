using System;
using System.Collections.Generic;

public class ListenerManager
{
    public Action<int, Define.ESkillId> onExecuteActiveSkill;
    public Action<int> onTriggerAnim;
    public Action<int, StatManager, DamageInfo> onExecPlayerClone;
    public Action<int, StatManager, DamageInfo> onExecEarthquake;
    public Action<int, List<Stat>> onStatAddModifier;
    public Action<int, List<Stat>> onStatSubModifier;
    public Action<int> giveGoldToPlayer;
    public Action<List<GiveToPlayerItemInfo>> giveItemsToPlayer;
    public Action<GiveToPlayerItemInfo> giveItemToPlayer;
    public Action<SO_Item> useItem;
    public Action<SO_Item.EWeaponType> setActiveSkillCuzEquip;
    public Action<SO_Item> unequip;
    public Action<SO_Item, bool, int> subItem;
    public Action<SO_Item.EItemHotkeyOrder> onPressedItemHotkey;
    public Action<SO_Item.EItemHotkeyOrder, SO_Item> registerItemHotkey;
    public Action<SO_Item, int> buyItem;
    public Action<SO_BaseWeapon, SO_Item> enhance;
    public Action<ActiveSkill_Lite, int> requestActiveSkillLevelUp;
    public Action<PassiveSkill_Lite> requestPassiveSkillLevelUp;
    public Action<ESkillMatId, EActiveSkillOrder> useActiveSkillMat;
    public Action<PassiveSkill_Lite> usePassiveSkillMat;
    public Action<List<SO_ActiveSkill>> checkSkillMatCanLevelUpSkills;
    public Action<Define.ESkillId, int> equipPassiveSkill;
    public Action<Define.ESkillId> unequipPassiveSkill;
    public Action<Define.EDamageType, StatManager> execTakeDamageEffect;
    public Action<bool> switchActionMap;
    public Action initCombo;
    public Action increaseCurrentInventoryNum;
    public Action decreaseCurrentInventoryNum;
    public Action playersNormalAttack;
    public Action enablePlayerControl;
    public Action disablePlayerControl;
    public Func<int, Stats> getStats;
    public Func<int> getPlayerInstId;
    public Func<SO_Item.EItemCategory, List<SO_Item>> getItems;
    public Func<SO_Item.EItemCategory, List<StackableItem>> getStackableItems;
    public Func<List<SO_Item>> getEquippedItems;
    public Func<SO_BaseWeapon> getEquippedWeapon;
    public Func<GoldInvenCapacity> getGoldInvenCapacity;
    public Func<SO_Item, bool, int, bool> addItem;
    public Func<EActiveSkillOrder, SO_Skill> getSkill;
    public Func<EActiveSkillOrder, bool> isSkillReady;
    public Func<List<SO_ActiveSkill>> getCurrentActiveSkills;
    public Func<List<SO_PassiveSkill>> getAllPassiveSkills;
    public Func<SO_Item.EWeaponType> getEquippedWeaponType;
    public Func<EActiveSkillOrder, int> getActiveSkillLevel;
    public Func<SO_Item.EItemCategory, int, int, bool> hasEnoughCraftMat;
    public Func<bool> isEquippedWeapon;
    public Func<bool> playersAttack;
    public Func<bool> isAnyEquippedPassiveSkill;
    public Func<bool> isEnablePlayerControl;

    public void OnExecuteActiveSkill(int instanceId, Define.ESkillId skillId)
    {
        onExecuteActiveSkill?.Invoke(instanceId, skillId);
    }

    public SO_Skill GetSkill(EActiveSkillOrder skillOrder)
    {
        return getSkill?.Invoke(skillOrder);
    }

    public void OnTriggerAnim(int instanceId)
    {
        onTriggerAnim?.Invoke(instanceId);
    }

    public void OnExecPlayerClone(int instanceId, StatManager castStatManager, DamageInfo damageInfo)
    {
        onExecPlayerClone?.Invoke(instanceId, castStatManager, damageInfo);
    }
    
    public void OnExecEarthquake(int instanceId, StatManager castStatManager, DamageInfo damageInfo)
    {
        onExecEarthquake?.Invoke(instanceId, castStatManager, damageInfo);
    }

    public void GiveItemsToPlayer(List<GiveToPlayerItemInfo> giveToPlayerItemInfos)
    {
        giveItemsToPlayer?.Invoke(giveToPlayerItemInfos);
    }

    public void GiveGoldToPlayer(int gold)
    {
        giveGoldToPlayer?.Invoke(gold);
    }
    
    public void GiveItemToPlayer(GiveToPlayerItemInfo giveToPlayerItemInfos)
    {
        giveItemToPlayer?.Invoke(giveToPlayerItemInfos);
    }

    public List<SO_Item> GetItems(SO_Item.EItemCategory itemCategory)
    {
        return getItems?.Invoke(itemCategory);
    }

    public List<StackableItem> GetStackableItems(SO_Item.EItemCategory itemCategory)
    {
        return getStackableItems?.Invoke(itemCategory);
    }

    public void UseItem(SO_Item item)
    {
        useItem?.Invoke(item);
    }

    public List<SO_Item> GetEquippedItems()
    {
        return getEquippedItems?.Invoke();
    }

    public SO_BaseWeapon GetEquippedWeapon()
    {
        return getEquippedWeapon?.Invoke();
    }

    public Stats GetStats(int instanceId)
    {
        return getStats?.Invoke(instanceId);
    }

    public int GetPlayerInstId()
    {
        return getPlayerInstId.Invoke();
    }

    public void OnStatAddModifier(int instanceId, List<Stat> stats)
    {
        onStatAddModifier?.Invoke(instanceId, stats);
    }
    
    public void OnStatSubModifier(int instanceId, List<Stat> stats)
    {
        onStatSubModifier?.Invoke(instanceId, stats);
    }

    public void SetActiveSkillCuzEquip(SO_Item.EWeaponType type)
    {
        setActiveSkillCuzEquip?.Invoke(type);
        initCombo?.Invoke();
    }

    public bool IsSkillReady(EActiveSkillOrder skillOrder)
    {
        return isSkillReady.Invoke(skillOrder);
    }

    public List<SO_ActiveSkill> GetCurrentActiveSkills()
    {
        return getCurrentActiveSkills?.Invoke();
    }

    public List<SO_PassiveSkill> GetAllPassiveSkills()
    {
        return getAllPassiveSkills?.Invoke();
    }

    public SO_Item.EWeaponType GetEquippedWeaponType()
    {
        return getEquippedWeaponType.Invoke();
    }

    public void Unequip(SO_Item item)
    {
        unequip?.Invoke(item);
    }

    public void SubItem(SO_Item item, bool shouldRefreshUI, int amount = 0)
    {
        subItem?.Invoke(item, shouldRefreshUI, amount);
    }

    public GoldInvenCapacity GetGoldInvenCapacity()
    {
        return getGoldInvenCapacity?.Invoke();
    }

    public void OnPressedItemHotkey(SO_Item.EItemHotkeyOrder order)
    {
        onPressedItemHotkey?.Invoke(order);
    }

    public void RegisterItemHotkey(SO_Item.EItemHotkeyOrder order, SO_Item item)
    {
        registerItemHotkey?.Invoke(order, item);
    }

    public void RequestActiveSkillLevelUp(ActiveSkill_Lite skill, int index)
    {
        requestActiveSkillLevelUp?.Invoke(skill, index);
    }

    public void RequestPassiveSkillLevelUp(ref PassiveSkill_Lite skill)
    {
        requestPassiveSkillLevelUp?.Invoke(skill);
    }

    public void UseActiveSkillMat(ESkillMatId skillMatId, EActiveSkillOrder skillOrder)
    {
        useActiveSkillMat?.Invoke(skillMatId, skillOrder);
    }
    
    public void UsePassiveSkillMat(ref PassiveSkill_Lite skill)
    {
        usePassiveSkillMat?.Invoke(skill);
    }
    
    public void CheckSkillMatCanLevelUpSkills(List<SO_ActiveSkill> skillMatId)
    {
        checkSkillMatCanLevelUpSkills?.Invoke(skillMatId);
    }

    public int GetActiveSkillLevel(EActiveSkillOrder order)
    {
        return getActiveSkillLevel.Invoke(order);
    }

    public void EquipPassiveSkill(Define.ESkillId skillId, int uiIndex)
    {
        equipPassiveSkill?.Invoke(skillId, uiIndex);
    }

    public void UnequipPassiveSkill(Define.ESkillId skillId)
    {
        unequipPassiveSkill?.Invoke(skillId);
    }

    public void ExecTakeDamageEffect(Define.EDamageType causeDamageType, StatManager instigatorStatManager)
    {
        execTakeDamageEffect?.Invoke(causeDamageType, instigatorStatManager);
    }

    public void BuyItem(SO_Item item, int amount)
    {
        buyItem?.Invoke(item, amount);
    }

    public void Enhance(SO_BaseWeapon original, SO_Item weaponMat)
    {
        enhance?.Invoke(original, weaponMat);
    }

    public bool AddItem(SO_Item item, bool shouldRefreshUI, int amount)
    {
        return addItem.Invoke(item, shouldRefreshUI, amount);
    }

    public bool HasEnoughCraftMat(SO_Item.EItemCategory itemCategory, int equipmentMatAmount, int sharedMatAmount)
    {
        return hasEnoughCraftMat.Invoke(itemCategory, equipmentMatAmount, sharedMatAmount);
    }

    public void IncreaseCurrentInventoryNum()
    {
        increaseCurrentInventoryNum?.Invoke();
    }
    
    public void DecreaseCurrentInventoryNum()
    {
        decreaseCurrentInventoryNum?.Invoke();
    }

    public bool IsEquippedWeapon()
    {
        return isEquippedWeapon.Invoke();
    }

    public void PlayersNormalAttack()
    {
        playersNormalAttack?.Invoke();
    }

    public void EnablePlayerControl()
    {
        enablePlayerControl?.Invoke();
    }

    public void DisablePlayerControl()
    {
        disablePlayerControl?.Invoke();
        
    }

    public bool PlayersAttack()
    {
        return playersAttack.Invoke();
    }

    public bool IsAnyEquippedPassiveSkill()
    {
        return isAnyEquippedPassiveSkill.Invoke();
    }

    public bool IsEnablePlayerControl()
    {
        return isEnablePlayerControl.Invoke();
    }

    public void SwitchActionMap(bool isUI)
    {
        switchActionMap?.Invoke(isUI);
    }
}
