using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Inven_ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected Sprite itemIconSprite;
    public Item Item { get; protected set; }
    [SerializeField] protected TextMeshProUGUI itemAmount;
    [SerializeField] protected Image itemIconImage;
    [SerializeField] protected Image borderImage;
    [SerializeField] protected Transform equippedTransform;
    [SerializeField] protected RectTransform slotTransform;
    protected float lastClickTime = 0f;
    protected float currentClickTime = 0f;

    private void Awake()
    {
        itemAmount = GetComponentInChildren<TextMeshProUGUI>();
    }

    public virtual void SetItem(Item inItem) //장비
    {
        Item = inItem;
        itemIconSprite = Item.itemIcon;
        itemIconImage.sprite = itemIconSprite;
        itemIconImage.color = Color.white;
        borderImage.color = Color.white;
        
        itemAmount.text = "";
        Equipment equipment = Item as Equipment;
        if (equipment.bIsEquipped)
            equippedTransform.gameObject.SetActive(true);
       
    }

    public virtual void SetStackableItem(Item inItem, int amount) //소모품, 기타
    {
        Item = inItem;
        itemIconSprite = Item.itemIcon;
        itemIconImage.sprite = itemIconSprite;
        itemIconImage.color = Color.white;
        borderImage.color = Color.white;
        
        itemAmount.text =  $"x{amount:#,0}";
    }
    
    public virtual void OnPointerClick(PointerEventData eventData)
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
                case Item.EItemCategory.Weapon:
                case Item.EItemCategory.Armor:
                case Item.EItemCategory.Acc:
                    Equipment equipment = (Equipment)Item;
                    if (equipment.bIsEquipped)
                        GI.Inst.UIManager.VisibleInventoryPopup(EInventoryPopupType.EquippedEquipment, Item, pos);
                    else
                        GI.Inst.UIManager.VisibleInventoryPopup(EInventoryPopupType.Equipment, Item, pos);
                    break;
                case Item.EItemCategory.Consumable:
                    GI.Inst.UIManager.VisibleInventoryPopup(EInventoryPopupType.Consumable, Item, pos);
                    break;
                case Item.EItemCategory.Etc:
                    GI.Inst.UIManager.VisibleInventoryPopup(EInventoryPopupType.Etc, Item, pos);
                    break;
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 goPos = gameObject.transform.position;
        Vector3 pos = new Vector3(goPos.x - slotTransform.rect.width, goPos.y);
        GI.Inst.UIManager.VisibleItemTooltip(Item, pos, Define.RIGHT_PIVOT);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        GI.Inst.UIManager.InvisibleItemTooltip();
    }
}
