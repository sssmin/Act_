using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Merchant_CraftLine : MonoBehaviour
{
    [SerializeField] private Button craftButton;
    [SerializeField] private Image weaponArmorAccMatIcon;
    [SerializeField] private TextMeshProUGUI craftText;
    [SerializeField] private TextMeshProUGUI requireSharedMatText;
    
    private SO_Item.EItemCategory ItemCategory { get; set; }

    private ItemCraft ItemCraft { get; set; }
    private RequireMatAmount requireMatAmount;
    
    public void InitOnce(SO_Item.EItemCategory category)
    {
        ItemCategory = category;
        craftButton.onClick.RemoveListener(OnClickCraftButton);
        craftButton.onClick.AddListener(OnClickCraftButton);
        
        GI.Inst.UIManager.SetNormalButtonColorPreset(craftButton);
        
        GI.Inst.ResourceManager.CreateItemCraft(Define.ELabel.ItemCraft, craft =>
        {
            switch (ItemCategory)
            {
                case SO_Item.EItemCategory.Weapon:
                    weaponArmorAccMatIcon.sprite = craft.weaponMatIcon;
                    craftText.text = "무기 제작";
                    break;
                case SO_Item.EItemCategory.Armor: 
                    weaponArmorAccMatIcon.sprite = craft.armorMatIcon;
                    craftText.text = "방어구 제작";
                    break;
                case SO_Item.EItemCategory.Acc: 
                    weaponArmorAccMatIcon.sprite = craft.accMatIcon;
                    craftText.text = "장신구 제작";
                    break;
            }
            requireMatAmount = craft.requireMatAmounts.FirstOrDefault(r => r.itemCategory == ItemCategory);
            requireSharedMatText.text = requireMatAmount.requireSharedMatAmount.ToString();
        });
    }

    private void OnDestroy()
    {
        craftButton.onClick.RemoveListener(OnClickCraftButton);
    }

    public void Init(ItemCraft itemCraft)
    {
        ItemCraft = itemCraft;
        RefreshCraftLine();
    }
    
    public void RefreshCraftLine()
    {
        int requireEquipmentMatAmount = requireMatAmount.requireEquipmentMatAmount;
        int requireSharedMatAmount = requireMatAmount.requireSharedMatAmount;

        if (GI.Inst.ListenerManager.HasEnoughCraftMat(ItemCategory, requireEquipmentMatAmount, requireSharedMatAmount))
        {
            craftButton.interactable = true;
        }
        else
        {
            craftButton.interactable = false;
        }
    }

    private void OnClickCraftButton()
    {
        ItemCraft.CreateRandomItem(ItemCategory);
        GI.Inst.UIManager.RefreshCraftLines();
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
    }

    public void Close()
    {
        Destroy(ItemCraft);
        ItemCraft = null;
    }
    
    public Button GetWeaponCraftButton()
    {
        return craftButton;
    }
    
}
