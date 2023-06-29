using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public struct InventoryIcon
{
    public Item.EItemCategory itemCategory;
    public Sprite icon;
}

public class UI_Inven_CategoryButton : MonoBehaviour
{
    [SerializeField] List<InventoryIcon> iconInfos = new List<InventoryIcon>();
    private Item.EItemCategory itemCategory;
    private Button button;
    private Sprite iconSprite;
    [SerializeField] Image image;
    private Image background;

    
    public void Init(Item.EItemCategory category)
    {
        button = GetComponent<Button>();
        background = GetComponent<Image>();
        itemCategory = category;
        
        foreach (InventoryIcon inventoryIcon in iconInfos)
        {
            if (inventoryIcon.itemCategory == category)
            {
                iconSprite = inventoryIcon.icon;
                image.sprite = iconSprite;
                image.preserveAspect = true;
                break;
            }
        }
        iconInfos.Clear();
    }
    

    private void Start()
    {
        button.onClick.AddListener(OnClickCategoryButton);
    }

    public void OnClickCategoryButton()
    {
        GI.Inst.UIManager.OnClickCategoryButton(itemCategory);
        GI.Inst.UIManager.ActiveAllInvenCategoryBtn();
        DeactivateButton();
    }
    
    public void DeactivateButton()
    {
        image.color = new Color(135f / 255f, 135f / 255f, 135f / 255f, 255f / 255f);
        button.interactable = false;
    }
    
    public void ActivateButton()
    {
        image.color = Color.white;
        button.interactable = true;
    }
}
