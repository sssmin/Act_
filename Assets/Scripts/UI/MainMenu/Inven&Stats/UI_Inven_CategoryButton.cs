using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct InventoryIcon
{
    public SO_Item.EItemCategory itemCategory;
    public Sprite icon;
}

public class UI_Inven_CategoryButton : MonoBehaviour
{
    [SerializeField] List<InventoryIcon> iconInfos = new List<InventoryIcon>();
    public SO_Item.EItemCategory itemCategory;
    private Button button;
    private Sprite iconSprite;
    [SerializeField] Image image;

    private void Start()
    {
        button.onClick.RemoveListener(OnClickCategoryButton);
        button.onClick.AddListener(OnClickCategoryButton);
    }
    
    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClickCategoryButton);
    }
    
    public void InitOnce(SO_Item.EItemCategory category)
    {
        button = GetComponent<Button>();
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
        
        GI.Inst.UIManager.SetNormalButtonColorPreset(button);
    }

    private void OnClickCategoryButton()
    {
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
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
