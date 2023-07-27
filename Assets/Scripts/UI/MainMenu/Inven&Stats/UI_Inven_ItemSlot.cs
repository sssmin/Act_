using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_Inven_ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected Sprite itemIconSprite;
    protected SO_Item Item { get; set; }
    [SerializeField] protected TextMeshProUGUI itemAmount;
    [SerializeField] protected TextMeshProUGUI enhanceLevel;
    [SerializeField] protected Image itemIconImage;
    [SerializeField] protected Image elementIconImage;
    [SerializeField] protected Image borderImage;
    [SerializeField] protected Image disableImage;
    [SerializeField] protected Transform equippedTransform;
    [SerializeField] protected RectTransform slotTransform;
    protected float lastClickTime = 0f;
    protected float currentClickTime = 0f;
    private bool isEnable;
    

    public void SetItem(SO_Item inItem) //장비
    {
        disableImage.gameObject.SetActive(false);
        isEnable = true;
        Item = inItem;
        itemIconSprite = Item.itemIcon;
        itemIconImage.sprite = itemIconSprite;
        itemIconImage.color = Color.white;
        borderImage.color = Color.white;
        
        itemAmount.text = "";
        SO_Equipment equipment = Item as SO_Equipment;
        if (equipment)
        {
            if (equipment.bIsEquipped)
                equippedTransform.gameObject.SetActive(true);
            SO_BaseWeapon weapon = equipment as SO_BaseWeapon;
            if (weapon)
            {
                if (weapon.EnhanceLevel != 0)
                    enhanceLevel.text = $"+{weapon.EnhanceLevel}";
                
                if (weapon.Element != EWeaponElement.None)
                {
                    string elementName = Enum.GetName(typeof(EWeaponElement), weapon.Element);
                    elementIconImage.sprite = GI.Inst.ResourceManager.GetStatusSprite(elementName);
                    elementIconImage.color = Color.white;
                }
            }
        }
    }

    public virtual void SetStackableItem(SO_Item inItem, int amount) //소모품, 기타
    {
        disableImage.gameObject.SetActive(false);
        isEnable = true;
        Item = inItem;
        itemIconSprite = Item.itemIcon;
        itemIconImage.sprite = itemIconSprite;
        itemIconImage.color = Color.white;
        borderImage.color = Color.white;
        
        itemAmount.text =  $"x{amount:#,0}";
    }

    public void CheckDisableSlot(SO_BaseWeapon original)
    {
        SO_BaseWeapon slotItem = Item as SO_BaseWeapon;
        if (slotItem)
        {
            if (slotItem.bIsEquipped || 
                (slotItem.Element != original.Element) || 
                (slotItem.weaponType != original.weaponType) ||
                (slotItem.rarity != original.rarity) ||
                (slotItem == original))
            {
                //disable
                disableImage.gameObject.SetActive(true);
                isEnable = false;
            }
        }
    }
    
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (isEnable)
        {
            if (GI.Inst.UIManager.MainMenuUI.InventoryStatus == EInventoryStatus.Enhance)
            {
                GI.Inst.UIManager.SetSameEquipment(Item);
            }
            else
            {
                currentClickTime = Time.time;
                if (currentClickTime - lastClickTime < 0.3f)
                {
                    GI.Inst.ListenerManager.UseItem(Item);
                }
                else
                {
                    lastClickTime = currentClickTime;
                }

                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    Vector3 goPos = gameObject.transform.position;
                    Vector3 pos = new Vector3(goPos.x - slotTransform.rect.width, goPos.y);
                    switch (Item.ItemCategory)
                    {
                        case SO_Item.EItemCategory.Weapon:
                        case SO_Item.EItemCategory.Armor:
                        case SO_Item.EItemCategory.Acc:
                            SO_Equipment equipment = (SO_Equipment)Item;
                            if (equipment.bIsEquipped)
                                GI.Inst.UIManager.VisibleInventoryPopup(EInventoryPopupType.EquippedEquipment, Item, pos);
                            else
                                GI.Inst.UIManager.VisibleInventoryPopup(EInventoryPopupType.Equipment, Item, pos);
                            break;
                        case SO_Item.EItemCategory.Consumable:
                            GI.Inst.UIManager.VisibleInventoryPopup(EInventoryPopupType.Consumable, Item, pos);
                            break;
                        case SO_Item.EItemCategory.Etc:
                            GI.Inst.UIManager.VisibleInventoryPopup(EInventoryPopupType.Etc, Item, pos);
                            break;
                    }
                }
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (GI.Inst.UIManager.MainMenuUI.InventoryStatus == EInventoryStatus.Default)
        {
            Vector3 goPos = gameObject.transform.position;
            Vector3 pos = new Vector3(goPos.x - slotTransform.rect.width, goPos.y);
            GI.Inst.UIManager.VisibleItemTooltip(Item, pos, Define.RIGHT_PIVOT);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (GI.Inst.UIManager.MainMenuUI.InventoryStatus == EInventoryStatus.Default)
        {
            GI.Inst.UIManager.InvisibleItemTooltip();
        }
    }
}
