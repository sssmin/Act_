using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//무기냐, 방어구냐, 소모품, 기타 등에 따라 해당하는 가진 아이템들
//ItemSlot_UI에 초기화하면서 여기 Grid에 하나씩 채워야함.
public class UI_Inven_StatSlotParent : MonoBehaviour
{
    private List<UI_Inven_StatSlot> statSlots = new List<UI_Inven_StatSlot>();

    
    public void Init()
    {
        ClearStatSlots();
        
        int playerInstId = GI.Inst.ListenerManager.GetPlayerInstId(); 
        Stats stats = GI.Inst.ListenerManager.GetStats(playerInstId);
        int maxStatNum = (int)Define.EStatType.Max - 1;
        
        for (int i = 1; i <= maxStatNum; i++)
        {
            GameObject go =
                GI.Inst.ResourceManager.Instantiate("UI_Inven_StatSlot", transform);
            UI_Inven_StatSlot invenStatSlot = go.GetComponent<UI_Inven_StatSlot>();
            invenStatSlot.Init(stats, (Define.EStatType)i);
            statSlots.Add(invenStatSlot);
        }
    }

    public void ClearStatSlots()
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