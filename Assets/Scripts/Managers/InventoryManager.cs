using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GoldInvenCapacity
{
   [SerializeField] public int maxInvenNum;
   [SerializeField] public int currentInvenNum;
   [SerializeField] public int gold;
}

[Serializable]
public class StackableItem
{
   [SerializeField] public Item item;
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

[Serializable]
public class ConsumableDictionary : SerializableDictionary<Item.EConsumableType, StackableItemSaveInfo>
{
}

[Serializable]
public class EtcDictionary : SerializableDictionary<string, StackableItemSaveInfo>
{
}

[Serializable]
public class RegisteredHotkeyItemDictionary : SerializableDictionary<Item.EItemHotkeyOrder, ItemSaveInfo>
{
}

[Serializable]
public class ItemSaveInfo
{
   [SerializeField] public string itemId;
   [SerializeField] public int amount;

   public void SetInfo(Item item)
   {
      itemId = item.itemId;
      amount = item.amount;
   }
}

[Serializable]
public class StackableItemSaveInfo
{
   [SerializeField] public string itemId;
   [SerializeField] public List<int> amounts;

   public void SetInfo(StackableItem item)
   {
      itemId = item.item.itemId;
      amounts = item.amounts;
   }
}

[Serializable]
public class InventoryInfo
{
   public InventoryInfo(List<Item> inWeaponInventory, List<Item> inArmorInventory, List<Item> inAccInventory, List<Item> inEquippedItems,
      Dictionary<Item.EConsumableType, StackableItem> inConsumableDictionary, 
      Dictionary<string, StackableItem> inEtcDictionary,
      Dictionary<Item.EItemHotkeyOrder, Consumable> inRegisteredHotkeyItemDictionary, 
      GoldInvenCapacity inGoldInvenCapacity)
   {
      Convert(inWeaponInventory, inArmorInventory, inAccInventory,
         inEquippedItems, inConsumableDictionary, inEtcDictionary, inRegisteredHotkeyItemDictionary,
         inGoldInvenCapacity);
   }
   
   [SerializeField] public ItemSaveInfo[] weaponInventory;
   [SerializeField] public ItemSaveInfo[] armorInventory;
   [SerializeField] public ItemSaveInfo[] accInventory;
   [SerializeField] public ItemSaveInfo[] equippedItems;
   [SerializeField] public ConsumableDictionary consumableInventory;
   [SerializeField] public EtcDictionary etcInventory;
   [SerializeField] public RegisteredHotkeyItemDictionary registeredHotkeyItems;
   [SerializeField] public GoldInvenCapacity goldInvenCapacity;

   private InventoryInfo Convert(List<Item> inWeaponInventory, List<Item> inArmorInventory, List<Item> inAccInventory, List<Item> inEquippedItems,
      Dictionary<Item.EConsumableType, StackableItem> inConsumableDictionary, 
      Dictionary<string, StackableItem> inEtcDictionary,
      Dictionary<Item.EItemHotkeyOrder, Consumable> inRegisteredHotkeyItemDictionary, GoldInvenCapacity inGoldInvenCapacity)
   {
      weaponInventory = new ItemSaveInfo[inWeaponInventory.Count];
      for (int i = 0; i < inWeaponInventory.Count; i++)
      {
         weaponInventory[i] = new ItemSaveInfo();
         weaponInventory[i].SetInfo(inWeaponInventory[i]);
      }
      
      armorInventory = new ItemSaveInfo[inArmorInventory.Count];
      for (int i = 0; i < inArmorInventory.Count; i++)
      {
         armorInventory[i] = new ItemSaveInfo();
         armorInventory[i].SetInfo(inArmorInventory[i]);
      }
      
      accInventory = new ItemSaveInfo[inAccInventory.Count];
      for (int i = 0; i < inAccInventory.Count; i++)
      {
         accInventory[i] = new ItemSaveInfo();
         accInventory[i].SetInfo(inAccInventory[i]);
      }
      
      equippedItems = new ItemSaveInfo[inEquippedItems.Count];
      for (int i = 0; i < inEquippedItems.Count; i++)
      {
         equippedItems[i] = new ItemSaveInfo();
         equippedItems[i].SetInfo(inEquippedItems[i]);
      }

      consumableInventory = new ConsumableDictionary();
      foreach (KeyValuePair<Item.EConsumableType, StackableItem> pair in inConsumableDictionary)
      {
         StackableItemSaveInfo itemSaveInfo = new StackableItemSaveInfo()
         {
            itemId = pair.Value.item.itemId, amounts = pair.Value.amounts
         };
         consumableInventory.Add(pair.Key, itemSaveInfo);
      }
      consumableInventory.Serialize();
      
      etcInventory = new EtcDictionary();
      foreach (KeyValuePair<string, StackableItem> pair in inEtcDictionary)
      {
         StackableItemSaveInfo itemSaveInfo = new StackableItemSaveInfo()
         {
            itemId = pair.Value.item.itemId, amounts = pair.Value.amounts
         };
         etcInventory.Add(pair.Key, itemSaveInfo);
      }
      etcInventory.Serialize();
      
      registeredHotkeyItems = new RegisteredHotkeyItemDictionary();
      foreach (KeyValuePair<Item.EItemHotkeyOrder, Consumable> pair in inRegisteredHotkeyItemDictionary)
      {
         ItemSaveInfo itemSaveInfo = new ItemSaveInfo()
         {
            itemId = pair.Value.itemId, amount = pair.Value.amount
         };
         registeredHotkeyItems.Add(pair.Key, itemSaveInfo);
      }
      registeredHotkeyItems.Serialize();


      goldInvenCapacity = inGoldInvenCapacity;
      return this;
   }

   public void Deserialize()
   {
      consumableInventory.Deserialize();
      etcInventory.Deserialize();
      registeredHotkeyItems.Deserialize();
   }
}

public class InventoryManager : MonoBehaviour //todo ScriptableObject
{
   private int playerInstId;
   
   private List<Item> WeaponInventory { get; set; } = new List<Item>();
   private List<Item> ArmorInventory { get; set; } = new List<Item>();
   private List<Item> AccInventory { get; set; } = new List<Item>();
   
   private Dictionary<Item.EConsumableType, StackableItem> ConsumableInventory { get; set; } = new Dictionary<Item.EConsumableType, StackableItem>();
   //private ConsumableDictionary ConsumableInventory { get; set; }= new ConsumableDictionary();
   private Dictionary<string, StackableItem> EtcInventory { get; set; } = new Dictionary<string, StackableItem>(); //key : itemId
   //private EtcDictionary EtcInventory { get; set; } = new EtcDictionary(); //key : itemId

   
   private List<Item> EquippedItems { get; set; } = new List<Item>();

   public Dictionary<Item.EItemHotkeyOrder, Consumable> RegisteredHotkeyItems { get; set; } = new Dictionary<Item.EItemHotkeyOrder, Consumable>();
   //private RegisteredHotkeyItemDictionary RegisteredHotkeyItems { get; set; } = new RegisteredHotkeyItemDictionary();
   private GoldInvenCapacity GoldInvenCapacity { get; set; }

   private void Awake()
   {
      GoldInvenCapacity = new GoldInvenCapacity()
      {
         maxInvenNum = 100, currentInvenNum = 0, gold = 0
      };
      
      playerInstId = GI.Inst.ListenerManager.GetPlayerInstId();
   }

   public void BindAction()
   {
      GI.Inst.ListenerManager.giveItemsToPlayer -= GiveItemsToPlayer;
      GI.Inst.ListenerManager.giveItemsToPlayer += GiveItemsToPlayer;
      GI.Inst.ListenerManager.getItems -= GetItems;
      GI.Inst.ListenerManager.getItems += GetItems;
      GI.Inst.ListenerManager.getStackableItems -= GetStackableItems;
      GI.Inst.ListenerManager.getStackableItems += GetStackableItems;
      GI.Inst.ListenerManager.useItem -= UseItem;
      GI.Inst.ListenerManager.useItem += UseItem;
      GI.Inst.ListenerManager.getEquippedItems -= GetEquippedItems;
      GI.Inst.ListenerManager.getEquippedItems += GetEquippedItems;
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
      GI.Inst.ListenerManager.hasEnoughEtcItems -= HasEnoughEtcItems;
      GI.Inst.ListenerManager.hasEnoughEtcItems += HasEnoughEtcItems;
      GI.Inst.ListenerManager.increaseCurrentInventoryNum -= IncreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.increaseCurrentInventoryNum += IncreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.decreaseCurrentInventoryNum -= DecreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.decreaseCurrentInventoryNum += DecreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.isEquippedWeapon -= IsEquippedWeapon;
      GI.Inst.ListenerManager.isEquippedWeapon += IsEquippedWeapon;
      
   }

   private void OnDestroy()
   {
      GI.Inst.ListenerManager.giveItemsToPlayer -= GiveItemsToPlayer;
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
      GI.Inst.ListenerManager.hasEnoughEtcItems -= HasEnoughEtcItems;
      GI.Inst.ListenerManager.increaseCurrentInventoryNum -= IncreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.decreaseCurrentInventoryNum -= DecreaseCurrentInventoryNum;
      GI.Inst.ListenerManager.isEquippedWeapon -= IsEquippedWeapon;
   }

   public void Init()
   {
      
   }
   

   private void Start()
   {
      
   }

   //test
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.H))
      { 
         Item item = GI.Inst.ResourceManager.GetItemData("InfinityBow");
         AddItemTest(item);
         item = GI.Inst.ResourceManager.GetItemData("WeaponMat");
         AddItemTest(item);
         item = GI.Inst.ResourceManager.GetItemData("SharedMat");
         AddItemTest(item);
         item = GI.Inst.ResourceManager.GetItemData("HpPotion");
         AddItemTest(item);
         item = GI.Inst.ResourceManager.GetItemData("ActiveNormalMat");
         AddItemTest(item);
         item = GI.Inst.ResourceManager.GetItemData("PassiveMat");
         AddItemTest(item);
         AddGold(10000);
      }
   }

   public void SetStartItem()
   {
      Item item = GI.Inst.ResourceManager.GetItemData("WoodDagger");
      AddItemTest(item);
   }

   public void IncreaseCurrentInventoryNum()
   {
      GoldInvenCapacity.currentInvenNum++;
      GI.Inst.UIManager.RefreshGoldInvenCapacityUI();
   }

   public void DecreaseCurrentInventoryNum()
   {
      GoldInvenCapacity.currentInvenNum--;
      GI.Inst.UIManager.RefreshGoldInvenCapacityUI();
   }

   public void AddItemTest(Item item)
   {
      AddItem(item, true,1);
   }

   public bool AddItem(Item item, bool shouldRefreshUI, int amount)
   {
      if (GoldInvenCapacity.currentInvenNum >= GoldInvenCapacity.maxInvenNum) return false;
      
      switch (item.ItemCategory)
      {
         case Item.EItemCategory.Weapon:
            WeaponInventory.Add(item);
            {
               Equipment equipment = (Equipment)item;
               equipment.Init(GI.Inst.Player.StatManager);
            }
            IncreaseCurrentInventoryNum();
            break;
         case Item.EItemCategory.Armor:
            ArmorInventory.Add(item);
            {
               Equipment equipment = (Equipment)item;
               equipment.Init(GI.Inst.Player.StatManager);
            }
            IncreaseCurrentInventoryNum();
            break;
         case Item.EItemCategory.Acc:
            AccInventory.Add(item);
            IncreaseCurrentInventoryNum();
            break;
         case Item.EItemCategory.Consumable:
            Consumable consumable = (Consumable)item;
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
               //IncreaseCurrentInventoryNum();
            }
            break;
         case Item.EItemCategory.Etc: 
            Etc etc = (Etc)item;
            
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
               //IncreaseCurrentInventoryNum();
            }
            if (etc.etcCategory == Item.EEtcCategory.SkillMat)
            {
               SkillMat skillMat = (SkillMat)etc;
               CheckSkillMatOnly(skillMat.skillMatId);
            }
            break;
      }
      if (shouldRefreshUI)
         RefreshInventoryUI();
      return true;
   }

   public void SubItem(Item item, bool shouldRefreshUI, int amount = 0) //버릴 때, 사용했을 때
   {
      switch (item.ItemCategory)
      {
         case Item.EItemCategory.Weapon:
            if (WeaponInventory.Contains(item))
            {
               WeaponInventory.Remove(item);
               DecreaseCurrentInventoryNum();
            }
            break;
         case Item.EItemCategory.Armor:
            if (ArmorInventory.Contains(item))
            {
               ArmorInventory.Remove(item);
               DecreaseCurrentInventoryNum();
            }
            break;
         case Item.EItemCategory.Acc:
            if (AccInventory.Contains(item))
            {
               AccInventory.Remove(item);
               DecreaseCurrentInventoryNum();
            }
            break;
         case Item.EItemCategory.Consumable:
            Consumable consumable = (Consumable)item;
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
         case Item.EItemCategory.Etc:
            Etc etc = (Etc)item;
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
   public void CheckSkillMatOnly(ESkillMatId skillMatId)
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

   public void BuyItem(Item item, int amount)
   {
      if (!AddItem(item, false, amount)) return;
      Consumable consumable = item as Consumable;
      AddGold(-(consumable.storeSellPrice * amount));
      RefreshInventoryUI();
   }

   public bool HasEnoughEtcItems(string itemId, int amount)
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
   public void CheckMatCanLevelUpActiveSkills(List<SO_ActiveSkill> skills)
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
   }
   
   private void GiveItemsToPlayer(int instanceId, List<GiveToPlayerItemInfo> giveToPlayerItemInfos )
   {
      if (playerInstId != instanceId) return;
      foreach (GiveToPlayerItemInfo itemInfo in giveToPlayerItemInfos)
      {
         AddItem(itemInfo.item, true, itemInfo.amount);
      }
   }
   
   public List<Item> GetItems(Item.EItemCategory itemCategory)
   {
      switch (itemCategory)
      {
         case Item.EItemCategory.Weapon:
            return WeaponInventory;
         case Item.EItemCategory.Armor:
            return ArmorInventory;
         case Item.EItemCategory.Acc:
            return AccInventory;
         
      }
      return null;
   }

   public List<StackableItem> GetStackableItems(Item.EItemCategory itemCategory)
   {
      switch (itemCategory)
      {
         case Item.EItemCategory.Consumable:
            return ConsumableInventory.Values.ToList();
         case Item.EItemCategory.Etc:
            return EtcInventory.Values.ToList();
      }

      return null;
   }

   private void UseItem(Item item)
   {
      Debug.Log(GetInstanceID());
      switch (item.ItemCategory)
      {
         case Item.EItemCategory.Weapon:
            if (WeaponInventory.Contains(item))
            {
               if (CheckEquippedItems(item)) return;
               Equip(item);
               return;
            }
            Debug.Log("에러. 인벤토리에 존재하지 않는 무기입니다.");
            break;
         case Item.EItemCategory.Armor:
            if (ArmorInventory.Contains(item))
            {
               if (CheckEquippedItems(item)) return;
               Equip(item);
               return;
            }
            Debug.Log("에러. 인벤토리에 존재하지 않는 방어구입니다.");
            break;
         case Item.EItemCategory.Acc: 
            if (AccInventory.Contains(item))
            {
               if (CheckEquippedItems(item)) return;
               Equip(item);
               return;
            }
            Debug.Log("에러. 인벤토리에 존재하지 않는 장신구입니다.");
            break;
         case Item.EItemCategory.Consumable:
            Consumable consumable = (Consumable)item;
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

   private bool CheckEquippedItems(Item item)
   {
      foreach (Item equippedItem in EquippedItems)
      {
         if (equippedItem.ItemCategory == item.ItemCategory)
         {
            EquipExchange(equippedItem, item);
            return true;
         }
      }
      return false;
   }

   public void Equip(Item item, bool shouldRefreshUI = true)
   {
      Equipment equipment = (Equipment)item;
      equipment.bIsEquipped = true;
      EquippedItems.Add(item);
      
      foreach (Effect effect in equipment.effects)
      {
          effect.effectInfo.onExecuteIncreaseStat?.Invoke();
      }
      //기본 스탯 적용
      GI.Inst.ListenerManager.OnStatAddModifier(playerInstId, equipment.itemStats);
      
      if (equipment.ItemCategory == Item.EItemCategory.Weapon)
         GI.Inst.ListenerManager.SetActiveSkillCuzEquip(((BaseWeapon)equipment).weaponType);
      if (shouldRefreshUI)
         RefreshInventoryUI();
   }

   public void EquipExchange(Item equippedItem, Item newItem)
   {
      Unequip(equippedItem);
      Equip(newItem);
   }

   public void Unequip(Item equippedItem)
   {
      Equipment equipment = (Equipment)equippedItem;
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

   void RegisterItemHotkey(Item.EItemHotkeyOrder order, Item item)
   {
      foreach (KeyValuePair<Item.EItemHotkeyOrder, Consumable> pair in RegisteredHotkeyItems)
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
         RegisteredHotkeyItems.Add(order, (Consumable)item);
         GI.Inst.UIManager.RefreshItemHotkeyUI(order, item);
      }
      else
      {
         RegisteredHotkeyItems.Add(order, (Consumable)item);
         GI.Inst.UIManager.RefreshItemHotkeyUI(order, item);
      }
      
   }

   void PressedItemHotkey(Item.EItemHotkeyOrder order)
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

   private void RefreshItemHotkeyUI(Item item)
   {
      Consumable consumable = (Consumable)item;
      foreach (KeyValuePair<Item.EItemHotkeyOrder,Consumable> pair in RegisteredHotkeyItems)
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
   
   
   
   #region Wrapper

   private List<Item> GetEquippedItems()
   {
      return EquippedItems;
   }
   
   public Item.EWeaponType GetEquippedWeaponType()
   {
      foreach (Item item in EquippedItems)
      {
         if (item.ItemCategory == Item.EItemCategory.Weapon)
         {
            BaseWeapon weapon = (BaseWeapon)item;
            return weapon.weaponType;
         }
      }
      return Item.EWeaponType.None;
   }

   public GoldInvenCapacity GetGoldInvenCapacity()
   {
      return GoldInvenCapacity;
   }

   void RefreshInventoryUI()
   {
      GI.Inst.UIManager.RefreshInventoryUI();
   }

   void RefreshGoldInvenCapacityUI()
   {
      GI.Inst.UIManager.RefreshGoldInvenCapacityUI();
   }

   bool IsEquippedWeapon()
   {
      foreach (Item item in EquippedItems)
      {
         if (item.ItemCategory == Item.EItemCategory.Weapon)
            return true;
      }

      return false;
   }

   //for save
   public InventoryInfo GetSerializeInventoryInfo()
   {
      InventoryInfo inventoryInfo = new InventoryInfo(WeaponInventory, ArmorInventory, AccInventory, EquippedItems, ConsumableInventory, EtcInventory, RegisteredHotkeyItems, GoldInvenCapacity);
      return inventoryInfo;
   }

   //load
   public void SetDeserializeInventoryInfo(InventoryInfo inventoryInfo)
   {
      inventoryInfo.Deserialize();
      
      ItemSaveInfo[] infos = inventoryInfo.weaponInventory;
      foreach (ItemSaveInfo info in infos)
      {
         Item item = GI.Inst.ResourceManager.GetItemData(info.itemId);
         WeaponInventory.Add(item);
      }

      infos = inventoryInfo.armorInventory;
      foreach (ItemSaveInfo info in infos)
      {
         Item item = GI.Inst.ResourceManager.GetItemData(info.itemId);
         ArmorInventory.Add(item);
      }
      
      infos = inventoryInfo.accInventory;
      foreach (ItemSaveInfo info in infos)
      {
         Item item = GI.Inst.ResourceManager.GetItemData(info.itemId);
         AccInventory.Add(item);
      }
      
      infos = inventoryInfo.equippedItems;
      foreach (ItemSaveInfo info in infos)
      {
         Item item = GI.Inst.ResourceManager.GetItemData(info.itemId);
         Equip(item, false);
      }

      ConsumableDictionary consumableInventory = inventoryInfo.consumableInventory;
      foreach (var pair in consumableInventory)
      {
         Item item = GI.Inst.ResourceManager.GetItemData(pair.Value.itemId);
         StackableItem stackableItem = new StackableItem()
         {
            item = item,
            amounts = pair.Value.amounts
         };
         ConsumableInventory.Add(pair.Key, stackableItem);
         Debug.Log(pair.Key);
      }
      
      EtcDictionary etcInventory = inventoryInfo.etcInventory;
      foreach (var pair in etcInventory)
      {
         Item item = GI.Inst.ResourceManager.GetItemData(pair.Value.itemId);
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
         Item item = GI.Inst.ResourceManager.GetItemData(pair.Value.itemId);
         item.amount = pair.Value.amount;
         RegisteredHotkeyItems.Add(pair.Key, (Consumable)item);
         GI.Inst.UIManager.RefreshItemHotkeyUI(pair.Key, item);
      }

      GoldInvenCapacity = inventoryInfo.goldInvenCapacity;
      
      GI.Inst.UIManager.RefreshGoldInvenCapacityUI();
      RefreshInventoryUI();
      
   }
   
   #endregion
   
}

