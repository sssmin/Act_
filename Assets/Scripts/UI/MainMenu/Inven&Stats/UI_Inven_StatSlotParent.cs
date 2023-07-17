using System.Collections.Generic;
using UnityEngine;


public class UI_Inven_StatSlotParent : MonoBehaviour
{
    private List<UI_Inven_StatSlot> statSlots = new List<UI_Inven_StatSlot>();

    
    private void Init()
    {
        ClearStatSlots();
        
        int playerInstId = GI.Inst.ListenerManager.GetPlayerInstId(); 
        Stats stats = GI.Inst.ListenerManager.GetStats(playerInstId);
        int maxStatNum = (int)Define.EStatType.Max - 1;
        
        for (int i = 1; i <= maxStatNum; i++)
        {
            if (i == (int)Define.EStatType.AttackIncValue || 
                i == (int)Define.EStatType.DefenceIncValue || 
                i == (int)Define.EStatType.CurrentHp) continue;
            GameObject go =
                GI.Inst.ResourceManager.Instantiate("UI_Inven_StatSlot", transform);
            UI_Inven_StatSlot invenStatSlot = go.GetComponent<UI_Inven_StatSlot>();
            invenStatSlot.Init(stats, (Define.EStatType)i);
            statSlots.Add(invenStatSlot);
        }
    }

    private void ClearStatSlots()
    {
        if (statSlots.Count > 0)
        {
            for (int i = statSlots.Count - 1; i >= 0; i--)
            {
                GI.Inst.ResourceManager.Destroy(statSlots[i].gameObject);
            }
            statSlots.Clear();
        }
    }

    public void RefreshInventoryUI()
    {
        Init();
    }
}