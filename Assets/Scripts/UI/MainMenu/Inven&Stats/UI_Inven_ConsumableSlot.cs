using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_ConsumableSlot : UI_Inven_ItemSlot
{
    [SerializeField] Image cooltimeImage;

    void Update()
    {
        cooltimeImage.fillAmount = GI.Inst.CooltimeManager.GetItemIconFillAmount(Item.itemId);
        //Debug.Log("cooltimeImage.fillAmount : " + cooltimeImage.fillAmount);    
        
    }
    
    public override void SetStackableItem(SO_Item inItem, int amount)
    {
        Item = inItem;
        itemIconSprite = Item.itemIcon;
        itemIconImage.sprite = itemIconSprite;
        itemIconImage.color = Color.white;
        borderImage.color = Color.white;
        
        itemAmount.text =  $"x{amount:#,0}";
    }
    
    
    public override void OnPointerClick(PointerEventData eventData)
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
                case SO_Item.EItemCategory.Consumable:
                    GI.Inst.UIManager.VisibleInventoryPopup(EInventoryPopupType.Consumable, Item, pos);
                    break;
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 goPos = gameObject.transform.position;
        Vector3 pos = new Vector3(goPos.x - slotTransform.rect.width, goPos.y);
        GI.Inst.UIManager.VisibleItemTooltip(Item, pos, Define.RIGHT_PIVOT);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        GI.Inst.UIManager.InvisibleItemTooltip();
    }
    
}
