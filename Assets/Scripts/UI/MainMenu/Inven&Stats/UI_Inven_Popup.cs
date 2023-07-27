using System;
using UnityEngine;
using UnityEngine.UI;


public class UI_Inven_Popup : UI_Popup
{
    [SerializeField] Button popupBg;
    [SerializeField] private RectTransform buttonParent;

    public void Init(EInventoryPopupType type, SO_Item item, Vector3 pos)
    {
        buttonParent.transform.position = pos;
        popupBg.onClick.RemoveListener(() => { GI.Inst.UIManager.ClosePopup(); });
        popupBg.onClick.AddListener(() => { GI.Inst.UIManager.ClosePopup(); });
        switch (type)
        {
             case EInventoryPopupType.EquippedEquipment:
                 CreateButton(item, "장착 해제", (i) =>
                 {
                     GI.Inst.ListenerManager.Unequip(i);
                 });
                 
                 break;
            case EInventoryPopupType.Equipment: //장착, 버리기, 강화
                CreateButton(item, "장착", (i) =>
                {
                    GI.Inst.ListenerManager.UseItem(i);
                });
                
                CreateButton(item, "버리기", (i) =>
                {
                    GI.Inst.UIManager.VisibleTBPopup(EThrowawayBuyPopupType.ThrowAwayConfirm, i);
                });
                SO_BaseWeapon weapon = item as SO_BaseWeapon;
                if (weapon && (weapon.EnhanceLevel < 10))
                {
                    CreateButton(item, "강화", (i) =>
                    {
                        GI.Inst.UIManager.VisibleEnhancePopup(i);
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
                    GI.Inst.ListenerManager.RegisterItemHotkey(SO_Item.EItemHotkeyOrder.First, i);
                });
                CreateButton(item, "퀵 슬롯 2 등록", (i) =>
                {
                    GI.Inst.ListenerManager.RegisterItemHotkey(SO_Item.EItemHotkeyOrder.Second, i);
                });
                CreateButton(item, "퀵 슬롯 3 등록", (i) =>
                {
                    GI.Inst.ListenerManager.RegisterItemHotkey(SO_Item.EItemHotkeyOrder.Third, i);
                });
                CreateButton(item, "퀵 슬롯 4 등록", (i) =>
                {
                    GI.Inst.ListenerManager.RegisterItemHotkey(SO_Item.EItemHotkeyOrder.Fourth, i);
                });
                CreateButton(item, "퀵 슬롯 5 등록", (i) =>
                {
                    GI.Inst.ListenerManager.RegisterItemHotkey(SO_Item.EItemHotkeyOrder.Fifth, i);
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

    private void OnDestroy()
    {
        popupBg.onClick.RemoveListener(() => { GI.Inst.UIManager.ClosePopup(); });
    }

    private void CreateButton(SO_Item item, string buttonLabel, Action<SO_Item> callback)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Inven_PopupButton", buttonParent.transform);
        UI_Inven_PopupButton button = go.GetComponent<UI_Inven_PopupButton>();
        button.Init(buttonLabel, item, callback);
    }
    
    public override void Close()
    {
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
