using System;
using System.Collections.Generic;
using UnityEngine;

public class ListenerManager
{
    public Action<int, Define.ESkillId> onExecuteActiveSkill;
    public Action<int> onTriggerAnim;
    public Action<int, StatManager, DamageInfo> onExecPlayerClone;
    public Action<int, StatManager, DamageInfo> onExecEarthquake;
    public Action<int,  List<GiveToPlayerItemInfo>> giveItemsToPlayer;
    public Action<int, List<Stat>> onStatAddModifier;
    public Action<int, List<Stat>> onStatSubModifier;
    public Action<Item> useItem;
    public Action<Item.EWeaponType> setActiveSkillCuzEquip;
    public Action<Item> unequip;
    public Action<Item, bool, int> subItem;
    public Action<Item.EItemHotkeyOrder> onPressedItemHotkey;
    public Action<Item.EItemHotkeyOrder, Item> registerItemHotkey;
    public Action<Item, int> buyItem;
    public Action<ActiveSkill_ShortVer, int> requestActiveSkillLevelUp;
    public Action<PassiveSkill_ShortVer> requestPassiveSkillLevelUp;
    public Action<ESkillMatId, EActiveSkillOrder> useActiveSkillMat;
    public Action<PassiveSkill_ShortVer> usePassiveSkillMat;
    public Action<List<SO_ActiveSkill>> checkSkillMatCanLevelUpSkills;
    public Action<Define.ESkillId, int> equipPassiveSkill;
    public Action<Define.ESkillId> unequipPassiveSkill;
    public Action<Define.EDamageType, StatManager> execTakeDamageEffect;
    public Action<bool> switchActionMap;
    public Action initCombo;
    public Action initUltSkillChargeAmount;
    public Action increaseCurrentInventoryNum;
    public Action decreaseCurrentInventoryNum;
    public Action playersNormalAttack;
    public Func<int, Stats> getStats;
    public Func<int> getPlayerInstId;
    public Func<Item.EItemCategory, List<Item>> getItems;
    public Func<Item.EItemCategory, List<StackableItem>> getStackableItems;
    public Func<List<Item>> getEquippedItems;
    public Func<BaseWeapon> getEquippedWeapon;
    public Func<GoldInvenCapacity> getGoldInvenCapacity;
    public Func<Item, bool, int, bool> addItem;
    public Func<EActiveSkillOrder, SO_Skill> getSkill;
    public Func<EActiveSkillOrder, bool> isSkillReady;
    public Func<List<SO_ActiveSkill>> getCurrentActiveSkills;
    public Func<List<SO_PassiveSkill>> getAllPassiveSkills;
    public Func<Item.EWeaponType> getEquippedWeaponType;
    public Func<EActiveSkillOrder, int> getActiveSkillLevel;
    public Func<float> getUltSkillChargeAmount;
    public Func<string, int, bool> hasEnoughEtcItems;
    public Func<bool> isEquippedWeapon;
    public Func<bool> playersAttack;

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

    public void GiveItemsToPlayer(int instanceId, List<GiveToPlayerItemInfo> giveToPlayerItemInfos)
    {
        giveItemsToPlayer?.Invoke(instanceId, giveToPlayerItemInfos);
    }

    public List<Item> GetItems(Item.EItemCategory itemCategory)
    {
        return getItems?.Invoke(itemCategory);
    }

    public List<StackableItem> GetStackableItems(Item.EItemCategory itemCategory)
    {
        return getStackableItems?.Invoke(itemCategory);
    }

    public void UseItem(Item item)
    {
        useItem?.Invoke(item);
    }

    public List<Item> GetEquippedItems()
    {
        return getEquippedItems?.Invoke();
    }

    public BaseWeapon GetEquippedWeapon()
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

    public void SetActiveSkillCuzEquip(Item.EWeaponType type)
    {
        setActiveSkillCuzEquip?.Invoke(type);
        initCombo?.Invoke();
    }

    public void InitUltSkillChargeAmount()
    {
        initUltSkillChargeAmount?.Invoke();
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

    public Item.EWeaponType GetEquippedWeaponType()
    {
        return getEquippedWeaponType.Invoke();
    }

    public void Unequip(Item item)
    {
        unequip?.Invoke(item);
    }

    public void SubItem(Item item, bool shouldRefreshUI, int amount = 0)
    {
        subItem?.Invoke(item, shouldRefreshUI, amount);
    }

    public GoldInvenCapacity GetGoldInvenCapacity()
    {
        return getGoldInvenCapacity?.Invoke();
    }

    public void OnPressedItemHotkey(Item.EItemHotkeyOrder order)
    {
        onPressedItemHotkey?.Invoke(order);
    }

    public void RegisterItemHotkey(Item.EItemHotkeyOrder order, Item item)
    {
        registerItemHotkey?.Invoke(order, item);
    }

    public void RequestActiveSkillLevelUp(ActiveSkill_ShortVer skill, int index)
    {
        requestActiveSkillLevelUp?.Invoke(skill, index);
    }

    public void RequestPassiveSkillLevelUp(ref PassiveSkill_ShortVer skill)
    {
        requestPassiveSkillLevelUp?.Invoke(skill);
    }

    public void UseActiveSkillMat(ESkillMatId skillMatId, EActiveSkillOrder skillOrder)
    {
        useActiveSkillMat?.Invoke(skillMatId, skillOrder);
    }
    
    public void UsePassiveSkillMat(ref PassiveSkill_ShortVer skill)
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

    public float GetUltSkillChargeAmount()
    {
        return getUltSkillChargeAmount.Invoke();
    }

    public void BuyItem(Item item, int amount)
    {
        buyItem?.Invoke(item, amount);
    }

    public bool AddItem(Item item, bool shouldRefreshUI, int amount)
    {
        return addItem.Invoke(item, shouldRefreshUI, amount);
    }

    public bool HasEnoughEtcItems(string itemId, int amount)
    {
        return hasEnoughEtcItems.Invoke(itemId, amount);
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

    public bool PlayersAttack()
    {
        return playersAttack.Invoke();
    }

    public void SwitchActionMap(bool isUI)
    {
        switchActionMap?.Invoke(isUI);
    }
}
