using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GoldInvenCapacity
{
   [SerializeField] public int maxInvenNum;
   [SerializeField] public int currentInvenNum;
   [SerializeField] public int gold;
   public Sprite goldIcon;
}

[Serializable]
public class StackableItem
{
   [SerializeField] public SO_Item item;
   [SerializeField] public List<int> amounts = new List<int>();
      
   public void AddAmount(int inAmount)
   {
      if (amounts.Count <= 0)
      {
         amounts.Add(0);
         GI.Inst.ListenerManager.IncreaseCurrentInventoryNum();
      }
      if ((amounts[amounts.Count - 1] + inAmount) > item.maxStackSize)
      {
         int canStackAmount = item.maxStackSize - amounts[amounts.Count - 1]; 
         int remainStackAmount = inAmount - canStackAmount; 
         amounts[amounts.Count - 1] = item.maxStackSize;

         amounts.Add(0);
         GI.Inst.ListenerManager.IncreaseCurrentInventoryNum();
            
         AddAmount(remainStackAmount);
      }
      else
      {
         amounts[amounts.Count - 1] += inAmount;
      }
   }

   public void SubAmount(int inAmount)
   {
      if (amounts.Count > 0)
      {
         if ((amounts[amounts.Count - 1] - inAmount) <= 0)
         {
            int remainSubAmount = inAmount - amounts[amounts.Count - 1];
               
            amounts.RemoveAt(amounts.Count - 1);
            GI.Inst.ListenerManager.DecreaseCurrentInventoryNum();
               
            if (remainSubAmount > 0)
               SubAmount(remainSubAmount);
         }
         else
         {
            amounts[amounts.Count - 1] -= inAmount;
         }
      }
   }
}


public class InventoryManager : MonoBehaviour //todo ScriptableObject
{
   private int playerInstId;
   
   private List<SO_Item> WeaponInventory { get; set; } = new List<SO_Item>();
   private List<SO_Item> ArmorInventory { get; set; } = new List<SO_Item>();
   private List<SO_Item> AccInventory { get; set; } = new List<SO_Item>();
   
   private Dictionary<SO_Item.EConsumableType, StackableItem> ConsumableInventory { get; set; } = new Dictionary<SO_Item.EConsumableType, StackableItem>();
   private Dictionary<string, StackableItem> EtcInventory { get; set; } = new Dictionary<string, StackableItem>(); //key : itemId


   private List<SO_Item> EquippedItems { get; set; } = new List<SO_Item>();

   private Dictionary<SO_Item.EItemHotkeyOrder, SO_Consumable> RegisteredHotkeyItems { get; set; } = new Dictionary<SO_Item.EItemHotkeyOrder, SO_Consumable>();
   
   private GoldInvenCapacity GoldInvenCapacity { get; set; }

   private void Awake()
   {
      GoldInvenCapacity = new GoldInvenCapacity()
      {
         maxInvenNum = 100, currentInvenNum = 0, gold = 0, goldIcon = GI.Inst.ResourceManager.GetItemSprite("Gold")
      };
      
      playerInstId = GI.Inst.ListenerManager.GetPlayerInstId();
   }

   public void BindAction()
   {
      GI.Inst.ListenerManager.giveItemsToPlayer -= GiveItemsToPlayer;
      GI.Inst.ListenerManager.giveItemsToPlayer += GiveItemsToPlayer;
      GI.Inst.ListenerManager.giveItemToPlayer -= GiveItemToPlayer;
      GI.Inst.ListenerManager.giveItemToPlayer += GiveItemToPlayer;
      GI.Inst.ListenerManager.getItems -= GetItems;
      GI.Inst.ListenerManager.getItems += GetItems;
      GI.Inst.ListenerManager.getStackableItems -= GetStackableItems;
      GI.Inst.ListenerManager.getStackableItems += GetStackableItems;
      GI.Inst.ListenerManager.useItem -= UseItem;
      GI.Inst.ListenerManager.useItem += UseItem;
      GI.Inst.ListenerManager.getEquippedItems -= GetEquippedItems;
      GI.Inst.ListenerManager.getEquippedItems += GetEquippedItems;
      GI.Inst.ListenerManager.getEquippedWeapon -= GetEquippedWeapon;
      GI.Inst.ListenerManager.getEquippedWeapon += GetEquippedWeapon;
      GI.Inst.ListenerManager.getEquippedWeaponType -= GetEquippedWeaponType;
      GI.Inst.ListenerManager.getEquippedWeaponType += GetEquippedWeaponType;
      GI.Inst.ListenerManager.unequip -= Unequip;
      GI.Inst.ListenerManager.unequip += Unequip;
      GI.Inst.ListenerManager.subItem -= SubItem;
      GI.Inst.ListenerManager.subItem += SubItem;
      GI.Inst.ListenerManager.getGoldInvenCapacity -= GetGoldInvenCapacity;
      GI.Inst.ListenerManager.getGoldInvenCapacity += GetGoldInvenCapacity;
      GI.Inst.ListenerManager.registerItemHotkey -= RegisterItemHotkey;
      GI.Inst.ListenerManager.registerItemHotkey += RegisterItemHotkey;
      GI.Inst.ListenerManager.onPressedItemHotkey -= PressedItemHotkey;
      GI.Inst.ListenerManager.onPressedItemHotkey += PressedItemHotkey;
      GI.Inst.ListenerManager.useActiveSkillMat -= UseActiveSkillMat;
      GI.Inst.ListenerManager.useActiveSkillMat += UseActiveSkillMat;
      GI.Inst.ListenerManager.usePassiveSkillMat -= UsePassiveSkillMat;
      GI.Inst.ListenerManager.usePassiveSkillMat += UsePassiveSkillMat;
      GI.Inst.ListenerManager.checkSkillMatCanLevelUpSkills -= CheckMatCanLevelUpActiveSkills;
      GI.Inst.ListenerManager.checkSkillMatCanLevelUpSkills += CheckMatCanLevelUpActiveSkills;
      GI.Inst.ListenerManager.buyItem -= BuyItem;
      GI.Inst.ListenerManager.buyItem += BuyItem;
      GI.Inst.ListenerManager.addItem -= AddItem;
      GI.Inst.ListenerManager.addItem += AddItem;
      GI.Inst.ListenerManager.hasEnoughCraftMat -= HasEnoughCraftMat;
      GI.Inst.ListenerManager.hasEnoughCraftMat += HasEnoughCraftMat;
      GI.Inst.ListenerManager.increaseCurrentInventoryNum -= IncreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.increaseCurrentInventoryNum += IncreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.decreaseCurrentInventoryNum -= DecreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.decreaseCurrentInventoryNum += DecreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.isEquippedWeapon -= IsEquippedWeapon;
      GI.Inst.ListenerManager.isEquippedWeapon += IsEquippedWeapon;
      GI.Inst.ListenerManager.enhance -= Enhance;
      GI.Inst.ListenerManager.enhance += Enhance;
      GI.Inst.ListenerManager.giveGoldToPlayer -= AddGold;
      GI.Inst.ListenerManager.giveGoldToPlayer += AddGold;

   }

   private void OnDestroy()
   {
      GI.Inst.ListenerManager.giveItemsToPlayer -= GiveItemsToPlayer;
      GI.Inst.ListenerManager.giveItemToPlayer -= GiveItemToPlayer;
      GI.Inst.ListenerManager.getItems -= GetItems;
      GI.Inst.ListenerManager.getStackableItems -= GetStackableItems;
      GI.Inst.ListenerManager.useItem -= UseItem;
      GI.Inst.ListenerManager.getEquippedItems -= GetEquippedItems;
      GI.Inst.ListenerManager.getEquippedWeaponType -= GetEquippedWeaponType;
      GI.Inst.ListenerManager.unequip -= Unequip;
      GI.Inst.ListenerManager.subItem -= SubItem;
      GI.Inst.ListenerManager.getGoldInvenCapacity -= GetGoldInvenCapacity;
      GI.Inst.ListenerManager.registerItemHotkey -= RegisterItemHotkey;
      GI.Inst.ListenerManager.onPressedItemHotkey -= PressedItemHotkey;
      GI.Inst.ListenerManager.useActiveSkillMat -= UseActiveSkillMat;
      GI.Inst.ListenerManager.usePassiveSkillMat -= UsePassiveSkillMat;
      GI.Inst.ListenerManager.checkSkillMatCanLevelUpSkills -= CheckMatCanLevelUpActiveSkills;
      GI.Inst.ListenerManager.buyItem -= BuyItem;
      GI.Inst.ListenerManager.addItem -= AddItem;
      GI.Inst.ListenerManager.hasEnoughCraftMat -= HasEnoughCraftMat;
      GI.Inst.ListenerManager.increaseCurrentInventoryNum -= IncreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.decreaseCurrentInventoryNum -= DecreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.isEquippedWeapon -= IsEquippedWeapon;
      GI.Inst.ListenerManager.giveGoldToPlayer -= AddGold;
   }
   
   //test
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.H))
      { 
         SO_Item item = GI.Inst.ResourceManager.GetItemDataCopy("InfinityBow");
         ((SO_BaseWeapon)item).Element = (EWeaponElement)Random.Range(0, 4);
         AddItem(item, true,1);
         item = GI.Inst.ResourceManager.GetItemData("WeaponMat");
         AddItem(item, true,1);
         item = GI.Inst.ResourceManager.GetItemData("SharedMat");
         AddItem(item, true,1);
         item = GI.Inst.ResourceManager.GetItemData("HpPotion");
         AddItem(item, true,1);
         item = GI.Inst.ResourceManager.GetItemData("ActiveNormalMat");
         AddItem(item, true,1);
         item = GI.Inst.ResourceManager.GetItemData("PassiveMat");
         AddItem(item, true,1);
         AddGold(10000);
      }
   }

  

   private void IncreaseCurrentInventoryNum()
   {
      GoldInvenCapacity.currentInvenNum++;
      RefreshGoldInvenCapacityUI();
   }

   private void DecreaseCurrentInventoryNum()
   {
      GoldInvenCapacity.currentInvenNum--;
      RefreshGoldInvenCapacityUI();
   }

   private bool AddItem(SO_Item item, bool shouldRefreshUI, int amount)
   {
      if (GoldInvenCapacity.currentInvenNum >= GoldInvenCapacity.maxInvenNum) return false;
      
      switch (item.ItemCategory)
      {
         case SO_Item.EItemCategory.Weapon:
         {
               Comparer<SO_Item> comparer = Comparer<SO_Item>.Create((a, b) => string.Compare(a.itemName, b.itemName, StringComparison.Ordinal));
               int index = WeaponInventory.BinarySearch(item, comparer);
               if (index < 0)
                  WeaponInventory.Insert(~index, item);
               else
                  WeaponInventory.Insert(index, item);
               
               SO_Equipment equipment = (SO_Equipment)item;
               equipment.Init(GI.Inst.Player.StatManager);
            }
            IncreaseCurrentInventoryNum();
            break;
         case SO_Item.EItemCategory.Armor:
            {
               Comparer<SO_Item> comparer = Comparer<SO_Item>.Create((a, b) => string.Compare(a.itemName, b.itemName, StringComparison.Ordinal));
               int index = ArmorInventory.BinarySearch(item, comparer);
               if (index < 0)
                  ArmorInventory.Insert(~index, item);
               else
                  ArmorInventory.Insert(index, item);
               
               SO_Equipment equipment = (SO_Equipment)item;
               equipment.Init(GI.Inst.Player.StatManager);
            }
            IncreaseCurrentInventoryNum();
            break;
         case SO_Item.EItemCategory.Acc:
            {
               Comparer<SO_Item> comparer = Comparer<SO_Item>.Create((a, b) => string.Compare(a.itemName, b.itemName, StringComparison.Ordinal));
               int index = AccInventory.BinarySearch(item, comparer);
               if (index < 0)
                  AccInventory.Insert(~index, item);
               else
                  AccInventory.Insert(index, item);
            }
            IncreaseCurrentInventoryNum();
            break;
         case SO_Item.EItemCategory.Consumable:
            SO_Consumable consumable = (SO_Consumable)item;
            if (ConsumableInventory.ContainsKey(consumable.consumableType))
            {
               ConsumableInventory[consumable.consumableType].AddAmount(amount);
               ConsumableInventory[consumable.consumableType].item.amount += amount;
               RefreshItemHotkeyUI(ConsumableInventory[consumable.consumableType].item);
            }
            else
            {
               StackableItem stackableItem = new StackableItem();
               stackableItem.item = consumable;
               ConsumableInventory.Add(consumable.consumableType, stackableItem);
               ConsumableInventory[consumable.consumableType].AddAmount(amount);
               ConsumableInventory[consumable.consumableType].item.amount = amount;
            }
            break;
         case SO_Item.EItemCategory.Etc: 
            SO_Etc etc = (SO_Etc)item;
            
            if (EtcInventory.ContainsKey(etc.itemId))
            {
               EtcInventory[etc.itemId].AddAmount(amount);
               EtcInventory[etc.itemId].item.amount += amount;
            }
            else
            {
               StackableItem stackableItem = new StackableItem();
               stackableItem.item = etc;
               EtcInventory.Add(etc.itemId, stackableItem);
               EtcInventory[etc.itemId].AddAmount(amount);
               EtcInventory[etc.itemId].item.amount = amount;
            }
            if (etc.etcCategory == SO_Item.EEtcCategory.SkillMat)
            {
               SO_SkillMat skillMat = (SO_SkillMat)etc;
               CheckSkillMatOnly(skillMat.skillMatId);
            }
            break;
      }
      if (shouldRefreshUI)
         RefreshInventoryUI();
      return true;
   }

   private void SubItem(SO_Item item, bool shouldRefreshUI, int amount = 0) //버릴 때, 사용했을 때
   {
      switch (item.ItemCategory)
      {
         case SO_Item.EItemCategory.Weapon:
            if (WeaponInventory.Contains(item))
            {
               WeaponInventory.Remove(item);
               DecreaseCurrentInventoryNum();
            }
            break;
         case SO_Item.EItemCategory.Armor:
            if (ArmorInventory.Contains(item))
            {
               ArmorInventory.Remove(item);
               DecreaseCurrentInventoryNum();
            }
            break;
         case SO_Item.EItemCategory.Acc:
            if (AccInventory.Contains(item))
            {
               AccInventory.Remove(item);
               DecreaseCurrentInventoryNum();
            }
            break;
         case SO_Item.EItemCategory.Consumable:
            SO_Consumable consumable = (SO_Consumable)item;
            if (ConsumableInventory.ContainsKey(consumable.consumableType))
            {
               if (ConsumableInventory[consumable.consumableType].item.amount < amount)
               {
                  Debug.Log("보유한 수량보다 많은 양을 제거하려 함");
                  return;
               }
               ConsumableInventory[consumable.consumableType].SubAmount(amount);
               ConsumableInventory[consumable.consumableType].item.amount -= amount;
               RefreshItemHotkeyUI(ConsumableInventory[consumable.consumableType].item);
               if (ConsumableInventory[consumable.consumableType].item.amount <= 0)
               {
                  ConsumableInventory.Remove(consumable.consumableType);
               }
            }
            break;
         case SO_Item.EItemCategory.Etc:
            SO_Etc etc = (SO_Etc)item;
            if (EtcInventory.ContainsKey(etc.itemId))
            {
               if (EtcInventory[etc.itemId].item.amount < amount)
               {
                  Debug.Log("보유한 수량보다 많은 양을 제거하려 함");
                  return;
               }
               EtcInventory[etc.itemId].SubAmount(amount);
               EtcInventory[etc.itemId].item.amount -= amount;
               if (EtcInventory[etc.itemId].item.amount  <= 0)
               {
                  EtcInventory.Remove(etc.itemId);
               }
            }
            break;
      }
      if (shouldRefreshUI)
         RefreshInventoryUI();
   }
   
   //갯수만 체크 후 레벨업 버튼 활성화/비활성화 (AddItem할 때) => 모든 스킬 확인
   private void CheckSkillMatOnly(ESkillMatId skillMatId)
   {
      string itemId = Enum.GetName(typeof(ESkillMatId), skillMatId);
      switch (skillMatId)
      {
         case ESkillMatId.ActiveNormalMat:
         {
            List<SO_ActiveSkill> skills = GI.Inst.ListenerManager.GetCurrentActiveSkills();
            CalcAndSetCanLevelUp(skills, itemId, ESkillMatId.ActiveNormalMat);
            GI.Inst.UIManager.RefreshActiveSkillUI(skills);
         }
            break;
         case ESkillMatId.ActiveUltMat:
         {
            List<SO_ActiveSkill> skills = GI.Inst.ListenerManager.GetCurrentActiveSkills();
            CalcAndSetCanLevelUp(skills, itemId, ESkillMatId.ActiveUltMat);
            GI.Inst.UIManager.RefreshActiveSkillUI(skills);
         }
            break;
         case ESkillMatId.PassiveMat:
         {
            List<SO_PassiveSkill> skills = GI.Inst.ListenerManager.GetAllPassiveSkills();
            CalcAndSetCanLevelUp(skills, itemId);
            GI.Inst.UIManager.RefreshPassiveSkillUI(skills);
         }
            break;
      }
   }

   private void BuyItem(SO_Item item, int amount)
   {
      if ((amount <= 0) || (!AddItem(item, false, amount))) return;
      SO_Consumable consumable = item as SO_Consumable;
      AddGold(-(consumable.storeSellPrice * amount));
      RefreshInventoryUI();
   }

   private bool HasEnoughCraftMat(SO_Item.EItemCategory itemCategory, int equipmentMatAmount, int sharedMatAmount)
   {
      string requireEquipmentMat = "";
      
      switch (itemCategory)
      {
         case SO_Item.EItemCategory.Weapon:
            requireEquipmentMat = "WeaponMat";
            break;
         case SO_Item.EItemCategory.Armor:
            requireEquipmentMat = "ArmorMat";
            break;
         case SO_Item.EItemCategory.Acc:
            requireEquipmentMat = "AccMat";
            break;
      }

      return HasEnoughEtcItems(requireEquipmentMat, equipmentMatAmount) && HasEnoughEtcItems("SharedMat", sharedMatAmount);
   }

   private bool HasEnoughEtcItems(string itemId, int amount)
   {
      if (EtcInventory.ContainsKey(itemId))
      {
         if (EtcInventory[itemId].item.amount >= amount)
            return true;
      }
      return false;
   }

   #region ActiveSkillMat
   //액티브 스킬 강화 재료 사용했을 때
   public void UseActiveSkillMat(ESkillMatId skillMatId, EActiveSkillOrder skillOrder) 
   {
      string itemId = Enum.GetName(typeof(ESkillMatId), skillMatId);
      if (EtcInventory.ContainsKey(itemId))
      {
         CheckActiveSkillMatSubAmount(skillMatId, skillOrder);
      }
   }
   
   //레벨업 누른거 필요 갯수 계산 후 인벤에서 빼고, 뺀 후의 갯수로 레벨업 버튼 활성화/비활성화 (강화 재료 사용할 때)
   public void CheckActiveSkillMatSubAmount(ESkillMatId skillMatId, EActiveSkillOrder skillOrder)
   {
      List<SO_ActiveSkill> skills = GI.Inst.ListenerManager.GetCurrentActiveSkills();
      string itemId = Enum.GetName(typeof(ESkillMatId), skillMatId);
      switch (skillMatId)
      {
         case ESkillMatId.ActiveNormalMat:
            ActiveSkillMatSubAmountPrep(skillOrder, skills, itemId, ESkillMatId.ActiveNormalMat);
            CalcAndSetCanLevelUp(skills, itemId, ESkillMatId.ActiveNormalMat);
            break;
         case ESkillMatId.ActiveUltMat:
            ActiveSkillMatSubAmountPrep(skillOrder, skills, itemId, ESkillMatId.ActiveUltMat);
            CalcAndSetCanLevelUp(skills, itemId, ESkillMatId.ActiveUltMat);
            break;
      }
      GI.Inst.UIManager.RefreshActiveSkillUI(skills);
   }
   
   //갯수 차감 선처리
   private void ActiveSkillMatSubAmountPrep(EActiveSkillOrder skillOrder, List<SO_ActiveSkill> skills, string itemId, ESkillMatId skillMatId)
   {
      foreach (SO_ActiveSkill skill in skills)
      {
         if (skillMatId == ESkillMatId.ActiveNormalMat)
         {
            if (skill.activeSkillOrder == EActiveSkillOrder.Fourth || skill.activeSkillOrder == EActiveSkillOrder.Fifth)
               continue;
         }
         else
         {
            if (skill.activeSkillOrder != EActiveSkillOrder.Fourth)
               continue;
         }
            
         int skillLevel = GI.Inst.ListenerManager.GetActiveSkillLevel(skill.activeSkillOrder);
         if (skillLevel >= 10) continue;

         if (skill.activeSkillOrder == skillOrder)
         {
            if (EtcInventory.ContainsKey(itemId))
            {
               int requireAmount = GetRequireMatAmount(skillLevel);
               SubItem(EtcInventory[itemId].item, true, requireAmount);
            }
            break;
         }
      }
   }
   
   //필요 개수 계산 후 CanLevelUp 세팅
   private void CalcAndSetCanLevelUp(List<SO_ActiveSkill> skills, string itemId, ESkillMatId skillMatId)
   {
      foreach (SO_ActiveSkill skill in skills)
      {
         if (skillMatId == ESkillMatId.ActiveNormalMat)
         {
            if (skill.activeSkillOrder == EActiveSkillOrder.Fourth || skill.activeSkillOrder == EActiveSkillOrder.Fifth)
               continue;
         }
         else
         {
            if (skill.activeSkillOrder != EActiveSkillOrder.Fourth)
               continue;
         }
         int skillLevel = GI.Inst.ListenerManager.GetActiveSkillLevel(skill.activeSkillOrder);
         if (skillLevel >= 10)
         {
            skill.bCanLevelUp = false;
            continue;
         }

         int requireAmount = 1;
         if (skill.activeSkillOrder != EActiveSkillOrder.Fourth)
            requireAmount  = GetRequireMatAmount(skillLevel);

         if (EtcInventory.ContainsKey(itemId))
            skill.bCanLevelUp = EtcInventory[itemId].item.amount >= requireAmount;
         else
            skill.bCanLevelUp = false;
      }
   }
   
   #endregion

   #region PassiveSkillMat

   //패시브 스킬 강화 재료 사용했을 때
   public void UsePassiveSkillMat(PassiveSkill_ShortVer skill) 
   {
      string itemId = Enum.GetName(typeof(ESkillMatId), skill.itemIdForLevelUp);
      if (EtcInventory.ContainsKey(itemId))
      {
         CheckPassiveSkillMatSubAmount(ref skill, itemId);
      }
   }
   
   //레벨업 누른거 필요 갯수 계산 후 인벤에서 빼고, 뺀 후의 갯수로 레벨업 버튼 활성화/비활성화 (강화 재료 사용할 때)
   public void CheckPassiveSkillMatSubAmount(ref PassiveSkill_ShortVer passiveSkill, string itemId)
   {
      List<SO_PassiveSkill> skills = GI.Inst.ListenerManager.GetAllPassiveSkills();
      
      PassiveSkillMatSubAmountPrep(passiveSkill, itemId, skills);
      CalcAndSetCanLevelUp(skills, itemId);
      GI.Inst.UIManager.RefreshPassiveSkillUI(skills);
   }

   //갯수 차감 선처리
   private void PassiveSkillMatSubAmountPrep(PassiveSkill_ShortVer passiveSkill, string itemId, List<SO_PassiveSkill> skills)
   {
      foreach (SO_PassiveSkill skill in skills)
      {
         if (skill.skillLevel >= 10) continue;
         if (skill.skillId == passiveSkill.skillId)
         {
            if (EtcInventory.ContainsKey(itemId))
            {
               int requireAmount = GetRequireMatAmount(skill.skillLevel);
               SubItem(EtcInventory[itemId].item, true, requireAmount);
            }

            break;
         }
      }
   }

   //필요 개수 계산 후 CanLevelUp 세팅
   private void CalcAndSetCanLevelUp(List<SO_PassiveSkill> skills, string itemId)
   {
      foreach (SO_PassiveSkill skill in skills)
      {
         if (skill.skillLevel >= 10)
         {
            skill.bCanLevelUp = false;
            continue;
         }

         int requireAmount = GetRequireMatAmount(skill.skillLevel);

         if (EtcInventory.ContainsKey(itemId))
            skill.bCanLevelUp = EtcInventory[itemId].item.amount >= requireAmount;
         else
            skill.bCanLevelUp = false;
      }
   }

   #endregion
   
   //아이템 장착할 때 변경되는 액티브 스킬들을 체크
   private void CheckMatCanLevelUpActiveSkills(List<SO_ActiveSkill> skills)
   {
      foreach (SO_ActiveSkill skill in skills)
      {
         int skillLevel = GI.Inst.ListenerManager.GetActiveSkillLevel(skill.activeSkillOrder);
         if (skillLevel >= 10)
         {
            skill.bCanLevelUp = false;
            continue;
         }
         
         string itemId = Enum.GetName(typeof(ESkillMatId), skill.itemIdForLevelUp);
         int requireAmount = GetRequireMatAmount(skillLevel);
         switch (skill.itemIdForLevelUp)
         {
            case ESkillMatId.ActiveNormalMat:
               if (skill.activeSkillOrder != EActiveSkillOrder.Fourth)
               {
                  if (EtcInventory.ContainsKey(itemId))
                     skill.bCanLevelUp = EtcInventory[itemId].item.amount >= requireAmount;
                  else
                     skill.bCanLevelUp = false;
               }
               break;
            case ESkillMatId.ActiveUltMat:
               if (skill.activeSkillOrder == EActiveSkillOrder.Fourth)
               {
                  if (EtcInventory.ContainsKey(itemId))
                     skill.bCanLevelUp = EtcInventory[itemId].item.amount >= requireAmount;
                  else
                     skill.bCanLevelUp = false;
               }
               break;
            case ESkillMatId.PassiveMat:
               if (EtcInventory.ContainsKey(itemId))
                  skill.bCanLevelUp = EtcInventory[itemId].item.amount >= requireAmount;
               else
                  skill.bCanLevelUp = false;
               break;
         }
      }
      GI.Inst.UIManager.RefreshActiveSkillUI(skills);
   }

   public void AddGold(int value)
   {
      GoldInvenCapacity.gold = Mathf.Clamp(GoldInvenCapacity.gold + value, 0, Int32.MaxValue);
      GI.Inst.UIManager.RefreshGoldInvenCapacityUI();
      
      if (value > 0)
         GI.Inst.UIManager.CreateGetAnGoldSlot(GoldInvenCapacity.goldIcon, value.ToString());
   }
   
   private void GiveItemsToPlayer(List<GiveToPlayerItemInfo> giveToPlayerItemInfos)
   {
      foreach (GiveToPlayerItemInfo itemInfo in giveToPlayerItemInfos)
      {
         switch (itemInfo.itemCategory) //장비는 사실 의미 없다.
         {
            case SO_Item.EItemCategory.Weapon:
            case SO_Item.EItemCategory.Armor:
            case SO_Item.EItemCategory.Acc:
               SO_Item equipment = GI.Inst.ResourceManager.GetItemDataCopy(itemInfo.itemId);
               AddItem(equipment, true, 1);
               GI.Inst.UIManager.CreateGetAnItemSlot(equipment.itemIcon, equipment.itemName, "1");
               break;
            case SO_Item.EItemCategory.Consumable:
            case SO_Item.EItemCategory.Etc:
               SO_Item item = GI.Inst.ResourceManager.GetItemData(itemInfo.itemId);
               AddItem(item, true, itemInfo.amount);
               GI.Inst.UIManager.CreateGetAnItemSlot(item.itemIcon, item.itemName, itemInfo.amount.ToString());
               break;
         }
      }
   }

   private void GiveItemToPlayer(GiveToPlayerItemInfo itemInfo)
   {
      switch (itemInfo.itemCategory)
      {
         case SO_Item.EItemCategory.Weapon:
         case SO_Item.EItemCategory.Armor:
         case SO_Item.EItemCategory.Acc:
            SO_Item equipment = GI.Inst.ResourceManager.GetItemDataCopy(itemInfo.itemId);
            AddItem(equipment, true, 1);
            GI.Inst.UIManager.CreateGetAnItemSlot(equipment.itemIcon, equipment.itemName, "1");
            break;
         case SO_Item.EItemCategory.Consumable:
         case SO_Item.EItemCategory.Etc:
            SO_Item item = GI.Inst.ResourceManager.GetItemData(itemInfo.itemId);
            AddItem(item, true, itemInfo.amount);
            GI.Inst.UIManager.CreateGetAnItemSlot(item.itemIcon, item.itemName, itemInfo.amount.ToString());
            break;
      }
   }
   
   public List<SO_Item> GetItems(SO_Item.EItemCategory itemCategory)
   {
      switch (itemCategory)
      {
         case SO_Item.EItemCategory.Weapon:
            return WeaponInventory;
         case SO_Item.EItemCategory.Armor:
            return ArmorInventory;
         case SO_Item.EItemCategory.Acc:
            return AccInventory;
      }
      return null;
   }

   private List<StackableItem> GetStackableItems(SO_Item.EItemCategory itemCategory)
   {
      List<StackableItem> returnList = new List<StackableItem>();
      switch (itemCategory)
      {
         case SO_Item.EItemCategory.Consumable:
            {
               returnList = ConsumableInventory.Values.ToList();
               Comparer<StackableItem> comparer = Comparer<StackableItem>.Create((a, b) =>
                  string.Compare(a.item.itemName, b.item.itemName, StringComparison.Ordinal)); 
               returnList.Sort(comparer);
            }
            return returnList;
         case SO_Item.EItemCategory.Etc:
            {
               returnList = EtcInventory.Values.ToList();
            
               Comparer<StackableItem> comparer = Comparer<StackableItem>.Create((a, b) =>
                  string.Compare(a.item.itemName, b.item.itemName, StringComparison.Ordinal)); 
               returnList.Sort(comparer);
            }
            return returnList;
      }

      return null;
   }

   private void UseItem(SO_Item item)
   {
      switch (item.ItemCategory)
      {
         case SO_Item.EItemCategory.Weapon:
            if (WeaponInventory.Contains(item))
            {
               if (CheckEquippedItems(item)) return;
               Equip(item);
               return;
            }
            Debug.Log("에러. 인벤토리에 존재하지 않는 무기입니다.");
            break;
         case SO_Item.EItemCategory.Armor:
            if (ArmorInventory.Contains(item))
            {
               if (CheckEquippedItems(item)) return;
               Equip(item);
               return;
            }
            Debug.Log("에러. 인벤토리에 존재하지 않는 방어구입니다.");
            break;
         case SO_Item.EItemCategory.Acc: 
            if (AccInventory.Contains(item))
            {
               if (CheckEquippedItems(item)) return;
               Equip(item);
               return;
            }
            Debug.Log("에러. 인벤토리에 존재하지 않는 장신구입니다.");
            break;
         case SO_Item.EItemCategory.Consumable:
            SO_Consumable consumable = (SO_Consumable)item;
            if (consumable.itemCooltime > 0f) //쿨타임이 있는 소모품
            {
               if (GI.Inst.CooltimeManager.IsReadyItem(item.itemId))
               {
                  GI.Inst.CooltimeManager.SetItemCooltime(consumable.itemId, consumable.itemCooltime);
                  consumable.UseItem(GI.Inst.Player.StatManager);
                  SubItem(consumable, false, 1);
               }
            }
            else
            {
               consumable.UseItem(GI.Inst.Player.StatManager);
               SubItem(consumable, false, 1);
            }
            RefreshInventoryUI();
            break;
      }
   }

   private bool CheckEquippedItems(SO_Item item)
   {
      foreach (SO_Item equippedItem in EquippedItems)
      {
         if (equippedItem.ItemCategory == item.ItemCategory)
         {
            EquipExchange(equippedItem, item);
            return true;
         }
      }
      return false;
   }

   private void Equip(SO_Item item, bool shouldRefreshUI = true)
   {
      SO_Equipment equipment = (SO_Equipment)item;
      equipment.bIsEquipped = true;
      EquippedItems.Add(item);
      
      foreach (Effect effect in equipment.effects)
      {
          effect.effectInfo.onExecuteIncreaseStat?.Invoke();
      }
      //기본 스탯 적용
      GI.Inst.ListenerManager.OnStatAddModifier(playerInstId, equipment.itemStats);

      if (equipment.ItemCategory == SO_Item.EItemCategory.Weapon)
      {
         SO_BaseWeapon weapon = (SO_BaseWeapon)equipment;
         GI.Inst.ListenerManager.SetActiveSkillCuzEquip(weapon.weaponType);
      }
      if (shouldRefreshUI)
         RefreshInventoryUI();
   }

   public void EquipExchange(SO_Item equippedItem, SO_Item newItem)
   {
      Unequip(equippedItem);
      Equip(newItem);
   }

   public void Unequip(SO_Item equippedItem)
   {
      SO_Equipment equipment = (SO_Equipment)equippedItem;
      equipment.bIsEquipped = false;
      EquippedItems.Remove(equippedItem);
      
      foreach (Effect effect in equipment.effects)
      {
         effect.effectInfo.onExecuteDecreaseStat?.Invoke();
      }
      GI.Inst.ListenerManager.OnStatSubModifier(playerInstId, equipment.itemStats);
      RefreshInventoryUI();
      GI.Inst.UIManager.ClearActiveSkillSlots();
      GI.Inst.UIManager.ClearActiveSkillHotkeySlots();
   }

   void RegisterItemHotkey(SO_Item.EItemHotkeyOrder order, SO_Item item)
   {
      foreach (KeyValuePair<SO_Item.EItemHotkeyOrder, SO_Consumable> pair in RegisteredHotkeyItems)
      {
         if (pair.Value.itemId == item.itemId)
         {
            RegisteredHotkeyItems.Remove(pair.Key);
            GI.Inst.UIManager.ClearItemHotkeyUI(pair.Key);
            break;
         }
      }

      if (RegisteredHotkeyItems.ContainsKey(order))
      {
         RegisteredHotkeyItems.Remove(order);
         RegisteredHotkeyItems.Add(order, (SO_Consumable)item);
         GI.Inst.UIManager.RefreshItemHotkeyUI(order, item);
      }
      else
      {
         RegisteredHotkeyItems.Add(order, (SO_Consumable)item);
         GI.Inst.UIManager.RefreshItemHotkeyUI(order, item);
      }
      
   }

   void PressedItemHotkey(SO_Item.EItemHotkeyOrder order)
   {
      if (RegisteredHotkeyItems.ContainsKey(order))
      {
         if (GI.Inst.CooltimeManager.IsReadyItem(RegisteredHotkeyItems[order].itemId))
         {
            RegisteredHotkeyItems[order].UseItem(GI.Inst.Player.StatManager);
            GI.Inst.CooltimeManager.SetItemCooltime(RegisteredHotkeyItems[order].itemId, RegisteredHotkeyItems[order].itemCooltime);
            SubItem(RegisteredHotkeyItems[order],  true, 1);
            GI.Inst.UIManager.RefreshItemHotkeyUI(order, RegisteredHotkeyItems[order]);
         }
      }
   }

   private void RefreshItemHotkeyUI(SO_Item item)
   {
      SO_Consumable consumable = (SO_Consumable)item;
      foreach (KeyValuePair<SO_Item.EItemHotkeyOrder,SO_Consumable> pair in RegisteredHotkeyItems)
      {
         if (pair.Value.consumableType == consumable.consumableType)
         {
            GI.Inst.UIManager.RefreshItemHotkeyUI(pair.Key, item);
         }
      }
   }

   private int GetRequireMatAmount(int currentLevel)
   {
      if (currentLevel < 4)
         return 1;
      if (currentLevel < 7)
         return 2;
      if (currentLevel < 10)
         return 3;
      return 0;
   }

   private void Enhance(SO_BaseWeapon original, SO_Item weaponMat)
   {
      original.EnhanceLevel++;
      original.Init(GI.Inst.Player.StatManager);
      original.InitEnhanceStat();
      SubItem(weaponMat, true);
   }
   

   #region Wrapper

   private List<SO_Item> GetEquippedItems()
   {
      return EquippedItems;
   }

   private SO_BaseWeapon GetEquippedWeapon()
   {
      foreach (SO_Item item in EquippedItems)
      {
         SO_BaseWeapon baseWeapon = item as SO_BaseWeapon;
         if (baseWeapon) return baseWeapon;
      }

      return null;
   }
   
   private SO_Item.EWeaponType GetEquippedWeaponType()
   {
      foreach (SO_Item item in EquippedItems)
      {
         if (item.ItemCategory == SO_Item.EItemCategory.Weapon)
         {
            SO_BaseWeapon weapon = (SO_BaseWeapon)item;
            return weapon.weaponType;
         }
      }
      return SO_Item.EWeaponType.None;
   }

   private GoldInvenCapacity GetGoldInvenCapacity()
   {
      return GoldInvenCapacity;
   }

   private void RefreshInventoryUI()
   {
      GI.Inst.UIManager.RefreshInventoryUI();
   }

   private void RefreshGoldInvenCapacityUI()
   {
      GI.Inst.UIManager.RefreshGoldInvenCapacityUI();
   }

   private bool IsEquippedWeapon()
   {
      foreach (SO_Item item in EquippedItems)
      {
         if (item.ItemCategory == SO_Item.EItemCategory.Weapon)
            return true;
      }

      return false;
   }

   #endregion //Wrapper
   
   #region Serialize

   //for save
   public Serializable.S_InventoryInfo GetSerializeInventoryInfo() => new Serializable.S_InventoryInfo(WeaponInventory, ArmorInventory, AccInventory, EquippedItems, ConsumableInventory, EtcInventory, RegisteredHotkeyItems, GoldInvenCapacity);
   

   //load
   public void SetDeserializeInventoryInfo(Serializable.S_InventoryInfo inventoryInfo)
   {
      inventoryInfo.Deserialize();
      
      Serializable.WeaponItemSaveInfo_Lite[] weaponInfos = inventoryInfo.weaponInventory;
      foreach (Serializable.WeaponItemSaveInfo_Lite info in weaponInfos)
      {
         SO_Item item = GI.Inst.ResourceManager.GetItemDataCopy(info.itemId);
         SO_BaseWeapon weapon = item as SO_BaseWeapon;
         if (weapon)
         {
            weapon.Element = info.element;
            weapon.EnhanceLevel = info.enhanceLevel;
            weapon.Init(GI.Inst.Player.StatManager);
            weapon.InitEnhanceStat();
            
            WeaponInventory.Add(weapon);
            if (info.isEquipped)
            {
               Equip(item, false);
            }
         }
      }

      Serializable.ItemSaveInfo_Lite[] infos = inventoryInfo.armorInventory;
      foreach (Serializable.ItemSaveInfo_Lite info in infos)
      {
         SO_Item item = GI.Inst.ResourceManager.GetItemDataCopy(info.itemId);
         ArmorInventory.Add(item);
      }
      
      infos = inventoryInfo.accInventory;
      foreach (Serializable.ItemSaveInfo_Lite info in infos)
      {
         SO_Item item = GI.Inst.ResourceManager.GetItemDataCopy(info.itemId);
         AccInventory.Add(item);
      }

      ConsumableDictionary consumableInventory = inventoryInfo.consumableInventory;
      foreach (var pair in consumableInventory)
      {
         SO_Item item = GI.Inst.ResourceManager.GetItemData(pair.Value.itemId);
         StackableItem stackableItem = new StackableItem()
         {
            item = item,
            amounts = pair.Value.amounts
         };
         ConsumableInventory.Add(pair.Key, stackableItem);
      }
      
      EtcDictionary etcInventory = inventoryInfo.etcInventory;
      foreach (var pair in etcInventory)
      {
         SO_Item item = GI.Inst.ResourceManager.GetItemData(pair.Value.itemId);
         StackableItem stackableItem = new StackableItem()
         {
            item = item,
            amounts = pair.Value.amounts
         };
         
         EtcInventory.Add(pair.Key, stackableItem);
      }
      
      RegisteredHotkeyItemDictionary registeredHotkeyItemDictionary = inventoryInfo.registeredHotkeyItems;
      foreach (var pair in registeredHotkeyItemDictionary)
      {
         SO_Item item = GI.Inst.ResourceManager.GetItemData(pair.Value.itemId);
         item.amount = pair.Value.amount;
         RegisteredHotkeyItems.Add(pair.Key, (SO_Consumable)item);
         GI.Inst.UIManager.RefreshItemHotkeyUI(pair.Key, item);
      }

      GoldInvenCapacity = inventoryInfo.goldInvenCapacity;
      GoldInvenCapacity.goldIcon = GI.Inst.ResourceManager.GetItemSprite("Gold");
      
      RefreshGoldInvenCapacityUI();
      RefreshInventoryUI();
      
   }

   #endregion //Serializable
   
}

