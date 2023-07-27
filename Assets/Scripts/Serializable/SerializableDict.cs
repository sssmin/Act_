using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void Serialize()
    {
        keys.Clear();
        values.Clear();
        
        foreach (KeyValuePair<TKey,TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void Deserialize()
    {
        Clear();
        
        if (keys.Count != values.Count)
            Debug.Log("error");

        for (int i = 0; i < keys.Count; i++)
        {
            Add(keys[i], values[i]);
        }
    }
}

//GI
[Serializable]
public class DungeonInfoDictionary : SerializableDictionary<EDungeonType, DungeonInfo> { }

//TutorialManager
[Serializable]
public class TutorialStatusDictionary : SerializableDictionary<ETutorial, bool> { }

//SkillManager
[Serializable]
public class PassiveSkillDictionary : SerializableDictionary<Define.ESkillId, PassiveSaveInfo> { }

//InventoryManager
[Serializable]
public class ConsumableDictionary : SerializableDictionary<Item.EConsumableType, StackableItemSaveInfo> { }

//InventoryManager
[Serializable]
public class EtcDictionary : SerializableDictionary<string, StackableItemSaveInfo> { }

//InventoryManager
[Serializable]
public class RegisteredHotkeyItemDictionary : SerializableDictionary<Item.EItemHotkeyOrder, ItemSaveInfo> { }