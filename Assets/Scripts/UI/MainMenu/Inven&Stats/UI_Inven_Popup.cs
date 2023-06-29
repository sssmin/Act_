using System;
using UnityEngine;
using UnityEngine.UI;


public class UI_Inven_Popup : UI_Popup
{
    [SerializeField] Button popupBg;
    [SerializeField] private RectTransform buttonParent;

    public void Init(EInventoryPopupType type, Item item, Vector3 pos)
    {
        buttonParent.transform.position = pos;
        popupBg.onClick.AddListener(() =>
        {
            GI.Inst.UIManager.ClosePopup();
        });
        switch (type)
        {
             case EInventoryPopupType.EquippedEquipment:
                 {
                     CreateButton(item, "장착 해제", (i) =>
                     {
                         GI.Inst.ListenerManager.Unequip(i);
                     });
                 }
                 break;
            case EInventoryPopupType.Equipment: //장착, 버리기
                {
                    CreateButton(item, "장착", (i) =>
                    {
                        GI.Inst.ListenerManager.UseItem(i);
                    });
                    
                    CreateButton(item, "버리기", (i) =>
                    {
                        GI.Inst.UIManager.VisibleTBPopup(EThrowawayBuyPopupType.ThrowAwayConfirm, i);
                        //GI.Inst.ListenerManager.ThrowAwayItem(i);
                    });
                }
                break;
            case EInventoryPopupType.Consumable: //사용, 퀵슬롯 등록, 버리기
                CreateButton(item, "사용", (i) =>
                {
                    GI.Inst.ListenerManager.UseItem(i);
                });
                CreateButton(item, "퀵 슬롯 1 등록", (i) =>
                {
                    GI.Inst.ListenerManager.RegisterItemHotkey(Item.EItemHotkeyOrder.First, i);
                });
                CreateButton(item, "퀵 슬롯 2 등록", (i) =>
                {
                    GI.Inst.ListenerManager.RegisterItemHotkey(Item.EItemHotkeyOrder.Second, i);
                });
                CreateButton(item, "퀵 슬롯 3 등록", (i) =>
                {
                    GI.Inst.ListenerManager.RegisterItemHotkey(Item.EItemHotkeyOrder.Third, i);
                });
                CreateButton(item, "퀵 슬롯 4 등록", (i) =>
                {
                    GI.Inst.ListenerManager.RegisterItemHotkey(Item.EItemHotkeyOrder.Fourth, i);
                });
                CreateButton(item, "퀵 슬롯 5 등록", (i) =>
                {
                    GI.Inst.ListenerManager.RegisterItemHotkey(Item.EItemHotkeyOrder.Fifth, i);
                });
                CreateButton(item, "버리기", (i) =>
                {
                    GI.Inst.UIManager.VisibleTBPopup(EThrowawayBuyPopupType.ThrowAway, i);
                });
                break;
            case EInventoryPopupType.Etc: //버리기
                CreateButton(item, "버리기", (i) =>
                {
                    GI.Inst.UIManager.VisibleTBPopup(EThrowawayBuyPopupType.ThrowAway, i);
                });
                break;
        }
    }

    private void CreateButton(Item item, string buttonLabel, Action<Item> callback)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_PopupButton", buttonParent.transform);
        UI_Inven_PopupButton button = go.GetComponent<UI_Inven_PopupButton>();
        button.Init(buttonLabel, item, callback);
    }
    
    // public void OnDestroy()
    // {
    //     foreach (UI_InventoryPopupButton popupButton in popupButtons)
    //     {
    //         GI.Inst.ResourceManager.Destroy(popupButton.gameObject);
    //     }
    // }
}
