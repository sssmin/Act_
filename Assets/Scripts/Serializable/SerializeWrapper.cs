using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serializable
{
   #region Lite

   [Serializable]
   public class PassiveSaveInfo_Lite
   {
      [SerializeField] public Define.ESkillId skillId;
      [SerializeField] public int level;
      [SerializeField] public bool bCanLevelUp;
      [SerializeField] public int equipIndex;
    
   }
   
   [Serializable]
   public class ItemSaveInfo_Lite
   {
      [SerializeField] public string itemId;
      [SerializeField] public int amount;

      public virtual void CopyFromSO(SO_Item item)
      {
         itemId = item.itemId;
         amount = item.amount;
      }
   }

   [Serializable]
   public class WeaponItemSaveInfo_Lite : ItemSaveInfo_Lite
   {
      [SerializeField] public EWeaponElement element;
      [SerializeField] public int enhanceLevel;
      [SerializeField] public bool isEquipped;

      public override void CopyFromSO(SO_Item item)
      {
         base.CopyFromSO(item);
         element = ((SO_BaseWeapon)item).Element;
         enhanceLevel = ((SO_BaseWeapon)item).EnhanceLevel;
         isEquipped = ((SO_BaseWeapon)item).bIsEquipped;
      }
   }

   [Serializable]
   public class StackableItemSaveInfo_Lite
   {
      [SerializeField] public string itemId;
      [SerializeField] public List<int> amounts;

      public void CopyFromSO(StackableItem stackableItem)
      {
         itemId = stackableItem.item.itemId;
         amounts = stackableItem.amounts;
      }
   }

   [Serializable]
   public class DungeonInfo_Lite
   {
      [SerializeField] public EDungeonType dungeonType;
      [SerializeField] public string desc;
      [SerializeField] public List<DungeonInfoDetail> dungeonInfoDetails;
      
      public void CopyFromSO(SO_DungeonInfo info)
      {
         dungeonType = info.dungeonType;
         desc = info.desc;
         dungeonInfoDetails = info.dungeonInfoDetails;
      }
   }

   #endregion
   
   
   
   [Serializable]
   public class S_PlayerInfo
   {
       public S_PlayerInfo(string inSceneName, Vector2 inPos, float inCurrentHp, 
           Dictionary<Define.ESkillId, SO_PassiveSkill> inPassiveSkills,
           int[] inActiveSkillLevels)
       {
           sceneName = inSceneName;
           pos = inPos;
           currentHp = inCurrentHp;
           activeSkillLevels = inActiveSkillLevels;
           Convert(inPassiveSkills);
       }
       
       [SerializeField] public string sceneName;
       [SerializeField] public Vector2 pos;
       [SerializeField] public float currentHp;
       [SerializeField] public PassiveSkillDictionary passiveSkills;
       [SerializeField] public int[] activeSkillLevels;

       private void Convert(Dictionary<Define.ESkillId, SO_PassiveSkill> inPassiveSkills)//, Dictionary<Define.ESkillId, SO_PassiveSkill> inEquippedPassiveSkills)
       {
           passiveSkills = new PassiveSkillDictionary();
           foreach (KeyValuePair<Define.ESkillId, SO_PassiveSkill> pair in inPassiveSkills)
           {
               PassiveSaveInfo_Lite passiveSaveInfo = new PassiveSaveInfo_Lite()
               {
                   skillId = pair.Key, level = pair.Value.skillLevel, bCanLevelUp = pair.Value.bCanLevelUp, equipIndex = pair.Value.equipIndex
               };
               passiveSkills.Add(pair.Key, passiveSaveInfo);
           }
           passiveSkills.Serialize();
       }

       public void Deserialize()
       {
           passiveSkills.Deserialize();
       }
   }
   

   [Serializable]
   public class S_InventoryInfo
   {
      public S_InventoryInfo(List<SO_Item> inWeaponInventory, List<SO_Item> inArmorInventory, List<SO_Item> inAccInventory, List<SO_Item> inEquippedItems,
         Dictionary<SO_Item.EConsumableType, StackableItem> inConsumableDictionary, 
         Dictionary<string, StackableItem> inEtcDictionary,
         Dictionary<SO_Item.EItemHotkeyOrder, SO_Consumable> inRegisteredHotkeyItemDictionary, 
         GoldInvenCapacity inGoldInvenCapacity)
      {
         Convert(inWeaponInventory, inArmorInventory, inAccInventory,
            inEquippedItems, inConsumableDictionary, inEtcDictionary, inRegisteredHotkeyItemDictionary,
            inGoldInvenCapacity);
      }
      
      [SerializeField] public WeaponItemSaveInfo_Lite[] weaponInventory;
      [SerializeField] public ItemSaveInfo_Lite[] armorInventory;
      [SerializeField] public ItemSaveInfo_Lite[] accInventory;
      [SerializeField] public ConsumableDictionary consumableInventory;
      [SerializeField] public EtcDictionary etcInventory;
      [SerializeField] public RegisteredHotkeyItemDictionary registeredHotkeyItems;
      [SerializeField] public GoldInvenCapacity goldInvenCapacity;

      //직렬화할때 
      private void Convert(List<SO_Item> inWeaponInventory, List<SO_Item> inArmorInventory, List<SO_Item> inAccInventory, List<SO_Item> inEquippedItems,
         Dictionary<SO_Item.EConsumableType, StackableItem> inConsumableDictionary, 
         Dictionary<string, StackableItem> inEtcDictionary,
         Dictionary<SO_Item.EItemHotkeyOrder, SO_Consumable> inRegisteredHotkeyItemDictionary, GoldInvenCapacity inGoldInvenCapacity)
      {
         weaponInventory = new WeaponItemSaveInfo_Lite[inWeaponInventory.Count];
         for (int i = 0; i < inWeaponInventory.Count; i++)
         {
            weaponInventory[i] = new WeaponItemSaveInfo_Lite();
            weaponInventory[i].CopyFromSO(inWeaponInventory[i]);
         }
         
         armorInventory = new ItemSaveInfo_Lite[inArmorInventory.Count];
         for (int i = 0; i < inArmorInventory.Count; i++)
         {
            armorInventory[i] = new ItemSaveInfo_Lite();
            armorInventory[i].CopyFromSO(inArmorInventory[i]);
         }
         
         accInventory = new ItemSaveInfo_Lite[inAccInventory.Count];
         for (int i = 0; i < inAccInventory.Count; i++)
         {
            accInventory[i] = new ItemSaveInfo_Lite();
            accInventory[i].CopyFromSO(inAccInventory[i]);
         }

         consumableInventory = new ConsumableDictionary();
         foreach (KeyValuePair<SO_Item.EConsumableType, StackableItem> pair in inConsumableDictionary)
         {
            StackableItemSaveInfo_Lite itemSaveInfo = new StackableItemSaveInfo_Lite();
            itemSaveInfo.CopyFromSO(pair.Value);
          
            consumableInventory.Add(pair.Key, itemSaveInfo);
         }
         consumableInventory.Serialize();
         
         etcInventory = new EtcDictionary();
         foreach (KeyValuePair<string, StackableItem> pair in inEtcDictionary)
         {
            StackableItemSaveInfo_Lite itemSaveInfo = new StackableItemSaveInfo_Lite();
            itemSaveInfo.CopyFromSO(pair.Value);
            
            etcInventory.Add(pair.Key, itemSaveInfo);
         }
         etcInventory.Serialize();
         
         registeredHotkeyItems = new RegisteredHotkeyItemDictionary();
         foreach (KeyValuePair<SO_Item.EItemHotkeyOrder, SO_Consumable> pair in inRegisteredHotkeyItemDictionary)
         {
            ItemSaveInfo_Lite itemSaveInfo = new ItemSaveInfo_Lite();
            itemSaveInfo.CopyFromSO(pair.Value);
            
            registeredHotkeyItems.Add(pair.Key, itemSaveInfo);
         }
         registeredHotkeyItems.Serialize();
         
         goldInvenCapacity = inGoldInvenCapacity;
      }

      public void Deserialize()
      {
         consumableInventory.Deserialize();
         etcInventory.Deserialize();
         registeredHotkeyItems.Deserialize();
      }
   }

   [Serializable]
   public class S_DungeonInfo
   {
      public S_DungeonInfo(Dictionary<string, SO_DungeonInfo> info)
      {
         Convert(info);
      }
      
      [SerializeField] public DungeonInfoDictionary dungeonInfoDictionary;

      
      private void Convert(Dictionary<string, SO_DungeonInfo> info)
      {
         dungeonInfoDictionary = new DungeonInfoDictionary();
         
         foreach (KeyValuePair<string, SO_DungeonInfo> pair in info)
         {
            EDungeonType type = (EDungeonType)Enum.Parse(typeof(EDungeonType), pair.Key);
            DungeonInfo_Lite dungeonInfo = new DungeonInfo_Lite();
            dungeonInfo.CopyFromSO(pair.Value);
            dungeonInfoDictionary.Add(type, dungeonInfo);
         }
         dungeonInfoDictionary.Serialize();
      }
      
      public void Deserialize()
      {
         dungeonInfoDictionary.Deserialize();
      }
   }
   
}

