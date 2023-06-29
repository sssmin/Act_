using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Inven_EquippedSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Sprite itemIconSprite;
    private Item item;
    [SerializeField] Image itemIconImage;
    [SerializeField] public Item.EItemCategory itemCategory;
    [SerializeField] public Item.EArmorType armorType;
    [SerializeField] public Item.EAccType accType;
    [SerializeField] RectTransform slotTransform;

    public void SetItem(List<Item> inItem)
    {
        Item findItem = inItem.FirstOrDefault(i => i.ItemCategory == itemCategory);
        item = findItem;
        if (item == null)
        {
            //Debug.Log("처음엔 장착된 장비가 없어요.");
            return;
        }

        if (item.ItemCategory == Item.EItemCategory.Armor)
        {
            BaseArmor baseArmor = (BaseArmor)item;
            if (baseArmor.armorType == armorType)
            {
                itemIconSprite = item.itemIcon;
                itemIconImage.sprite = itemIconSprite;
                itemIconImage.color = Color.white;
            }
        }
        else if (item.ItemCategory == Item.EItemCategory.Acc)
        {
            BaseAcc baseAcc = (BaseAcc)item;
            if (baseAcc.accType == accType)
            {
                itemIconSprite = item.itemIcon;
                itemIconImage.sprite = itemIconSprite;
                itemIconImage.color = Color.white;
            }
        }
        else
        {
            itemIconSprite = item.itemIcon;
            itemIconImage.sprite = itemIconSprite;
            itemIconImage.color = Color.white;
        }
        
    }

    public void Clear()
    {
        item = null;
        itemIconSprite = null;
        itemIconImage.sprite = null;
        itemIconImage.color = Color.clear;
    }

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        Vector3 goPos = gameObject.transform.position;
        Vector3 pos = new Vector3(goPos.x + slotTransform.rect.width, goPos.y);
        GI.Inst.UIManager.VisibleItemTooltip(item, pos, Define.LEFT_PIVOT);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GI.Inst.UIManager.InvisibleItemTooltip();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                GI.Inst.ListenerManager.Unequip(item);
            }
        }
    }
    
}