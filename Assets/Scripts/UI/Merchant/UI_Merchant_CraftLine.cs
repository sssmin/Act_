using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Merchant_CraftLine : MonoBehaviour
{
    [SerializeField] private Button craftButton;
    [SerializeField] private Image weaponArmorAccMatIcon;
    [SerializeField] private TextMeshProUGUI craftText;
    [SerializeField] private TextMeshProUGUI requireSharedMatText;
    
    public Item.EItemCategory ItemCategory { get; private set; }

    private ItemCraft ItemCraft { get; set; }
    private RequireMatAmount requireMatAmount;
    
    public void InitOnce(Item.EItemCategory category)
    {
        ItemCategory = category;
        craftButton.onClick.RemoveListener(OnClickCraftButton);
        craftButton.onClick.AddListener(OnClickCraftButton);
        
        GI.Inst.ResourceManager.CreateItemCraft(Define.ELabel.ItemCraft, craft =>
        {
            switch (ItemCategory)
            {
                case Item.EItemCategory.Weapon:
                    weaponArmorAccMatIcon.sprite = craft.weaponMatIcon;
                    craftText.text = "무기 제작";
                    break;
                case Item.EItemCategory.Armor: 
                    weaponArmorAccMatIcon.sprite = craft.armorMatIcon;
                    craftText.text = "방어구 제작";
                    break;
                case Item.EItemCategory.Acc: 
                    weaponArmorAccMatIcon.sprite = craft.accMatIcon;
                    craftText.text = "장신구 제작";
                    break;
            }
            requireMatAmount = craft.requireMatAmounts.FirstOrDefault(r => r.itemCategory == ItemCategory);
            requireSharedMatText.text = requireMatAmount.requireSharedMatAmount.ToString();
        });
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
        string requireEquipmentMat = null;
        string requireSharedMat = "SharedMat";
        switch (ItemCategory)
        {
            case Item.EItemCategory.Weapon:
                requireEquipmentMat = "WeaponMat";
                break;
            case Item.EItemCategory.Armor:
                requireEquipmentMat = "ArmorMat";
                break;
            case Item.EItemCategory.Acc:
                requireEquipmentMat = "AccMat";
                break;
        }

        if (GI.Inst.ListenerManager.HasEnoughEtcItems(requireEquipmentMat, requireEquipmentMatAmount) &&
            GI.Inst.ListenerManager.HasEnoughEtcItems(requireSharedMat, requireSharedMatAmount))
        {
            craftButton.enabled = true;
            craftButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);
        }
        else
        {
            craftButton.enabled = false;
            craftButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
        }
    }

    public void OnClickCraftButton()
    {
        ItemCraft.CreateRandomItem(ItemCategory);
        GI.Inst.UIManager.RefreshCraftLines();
    }

    public void Close()
    {
        Destroy(ItemCraft);
        ItemCraft = null;
    }
    
}
