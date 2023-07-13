using UnityEngine;
using UnityEngine.UI;

public enum EMerchantType
{
    Buy,
    Craft
}

public class UI_Merchant_Menu : UI_Popup
{
    [SerializeField] private Button closeButton; 
    [SerializeField] private Button buyButton; 
    [SerializeField] private Button craftButton;
    [SerializeField] private Transform contentParentTransform;
    [SerializeField] private Transform inventoryParentTransform;

    private GameObject inventoryWrapper;
    private UI_Merchant_ItemLineParent itemLineParentUI;
    private UI_Merchant_CraftLineParent craftLineParent;

    private EMerchantType currentType;
    private Merchant Merchant { get; set; }
    private ItemCraft TempItemCraft { get; set; }
    public void InitOnce()
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_ItemLineParent", transform);
        itemLineParentUI = go.GetComponent<UI_Merchant_ItemLineParent>();
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_CraftLineParent", transform);
        craftLineParent = go.GetComponent<UI_Merchant_CraftLineParent>();
        craftLineParent.InitOnce();
    }
    
    public void Open(EMerchantType type, Merchant merchant)
    {
        currentType = type;
        Merchant = merchant;
        
        GI.Inst.ResourceManager.CreateItemCraft(Define.ELabel.ItemCraft, craft =>
        {
            TempItemCraft = craft;
            
            switch (type)
            {
                case EMerchantType.Buy:
                {
                    craftLineParent.transform.SetParent(null);
                    itemLineParentUI.transform.SetParent(contentParentTransform);
                    
                    itemLineParentUI.rectTransform.localPosition = Vector3.zero;
                    
                    itemLineParentUI.Init(Merchant.GetItemIds());

                    inventoryWrapper = GI.Inst.UIManager.GetInventoryWrapper().gameObject;
                    inventoryWrapper.transform.SetParent(inventoryParentTransform);
                    buyButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                    craftButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);
                }
                    break;
                case EMerchantType.Craft:
                {
                    itemLineParentUI.transform.SetParent(null);
                    craftLineParent.transform.SetParent(contentParentTransform);
                    craftLineParent.transform.localPosition = Vector3.zero;
                    
                    craftLineParent.Init(TempItemCraft);
                    
                    inventoryWrapper = GI.Inst.UIManager.GetInventoryWrapper().gameObject;
                    inventoryWrapper.transform.SetParent(inventoryParentTransform);
                    craftButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                    buyButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);
                }
                    break;
            }
            
        });
        
       
    }

    public void Init(EMerchantType type, Merchant merchant)
    {
        currentType = type;
        Merchant = merchant;
        
        switch (type)
        {
            case EMerchantType.Buy:
                {
                    craftLineParent.transform.SetParent(null);
                    itemLineParentUI.transform.SetParent(contentParentTransform);
                    
                    itemLineParentUI.rectTransform.localPosition = Vector3.zero;
                    
                    itemLineParentUI.Init(Merchant.GetItemIds());

                    inventoryWrapper = GI.Inst.UIManager.GetInventoryWrapper().gameObject;
                    inventoryWrapper.transform.SetParent(inventoryParentTransform);
                    buyButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                    craftButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);
                }
                break;
            case EMerchantType.Craft:
                {
                    itemLineParentUI.transform.SetParent(null);
                    craftLineParent.transform.SetParent(contentParentTransform);
                    craftLineParent.transform.localPosition = Vector3.zero;
                    
                    craftLineParent.Init(TempItemCraft);
                    
                    
                    inventoryWrapper = GI.Inst.UIManager.GetInventoryWrapper().gameObject;
                    inventoryWrapper.transform.SetParent(inventoryParentTransform);
                    craftButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(176f);
                    buyButton.colors = GI.Inst.UIManager.GetPressedButtonPreset(255f);
                }
                break;
        }
    }

    public void OnClickCloseButton()
    {
        CloseMerchant();
    }

    public void CloseMerchant()
    {
        inventoryWrapper.transform.SetParent(null);
        itemLineParentUI.transform.SetParent(null);
        craftLineParent.transform.SetParent(null);
        
        GI.Inst.UIManager.InvisibleMerchantUI();
        craftLineParent.Close();
        Destroy(TempItemCraft);
        TempItemCraft = null;
    }
    
    public void OnClickBuyCategoryButton()
    {
        if (currentType == EMerchantType.Buy)
            return;
        Init(EMerchantType.Buy, Merchant);
    }
    
    public void OnClickCraftCategoryButton()
    {
        if (currentType == EMerchantType.Craft)
            return;
        GI.Inst.UIManager.ClearCraftResult();
        Init(EMerchantType.Craft, Merchant);
    }
     
}
