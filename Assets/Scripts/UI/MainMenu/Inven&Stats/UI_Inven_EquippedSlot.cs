using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_EquippedSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Sprite itemIconSprite;
    private SO_Item item;
    [SerializeField] Image itemIconImage;
    [SerializeField] public Image elementIconImage;
    [SerializeField] public SO_Item.EItemCategory itemCategory;
    [SerializeField] public SO_Item.EArmorType armorType;
    [SerializeField] public SO_Item.EAccType accType;
    [SerializeField] RectTransform slotTransform;
    [SerializeField] TextMeshProUGUI enhanceLevelText;

    public void InitOnce(SO_Item.EItemCategory category, SO_Item.EArmorType armor, SO_Item.EAccType acc)
    {
        itemCategory = category;
        armorType = armor;
        accType = acc;
    }
    
    public void SetItem(SO_Item inItem, SO_Item.EItemCategory category)
    {
        //방어구나 장신구 카테고리는 Armor, Acc 이렇게로만 되어있어서
        //장갑, 부츠 얘네도 다 하나의 Armor로 구분됨.
        //그래서 여기서 찾아지는게 똑같은 애들만 찾아짐. 헬멧이 있으면 헬멧만 나옴.
        item = inItem;

        if (category == SO_Item.EItemCategory.Weapon)
        {
            SO_BaseWeapon weapon = inItem as SO_BaseWeapon;
            if (weapon)
            {
                enhanceLevelText.text = 
                    weapon.EnhanceLevel > 0 ? enhanceLevelText.text = $"+{weapon.EnhanceLevel}" : enhanceLevelText.text = "";
                
                string elementName = Enum.GetName(typeof(EWeaponElement), weapon.Element);
                if (elementName == "None")
                {
                    elementIconImage.color = Color.clear;
                }
                else
                {
                    elementIconImage.sprite = GI.Inst.ResourceManager.GetStatusSprite(elementName);
                    elementIconImage.color = Color.white;
                }
            }
            itemIconSprite = item.itemIcon;
            itemIconImage.sprite = itemIconSprite;
            itemIconImage.color = Color.white;
        }
        else if (category == SO_Item.EItemCategory.Armor)
        {
            SO_BaseArmor baseArmor = (SO_BaseArmor)item;
            if (baseArmor.armorType == armorType)
            {
                itemIconSprite = item.itemIcon;
                itemIconImage.sprite = itemIconSprite;
                itemIconImage.color = Color.white;
            }
        }
        else if (category == SO_Item.EItemCategory.Acc)
        {
            SO_BaseAcc baseAcc = (SO_BaseAcc)item;
            if (baseAcc.accType == accType)
            {
                itemIconSprite = item.itemIcon;
                itemIconImage.sprite = itemIconSprite;
                itemIconImage.color = Color.white;
            }
        }

    }

    public void Clear()
    {
        item = null;
        itemIconSprite = null;
        itemIconImage.sprite = null;
        itemIconImage.color = Color.clear;
        if (elementIconImage)
            elementIconImage.color = Color.clear;
        if (enhanceLevelText)
            enhanceLevelText.text = "";
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