using UnityEngine;
using UnityEngine.UI;

public enum EMerchantType
{
    Buy,
    Craft
}

public class UI_Merchant_Menu : UI_Popup
{
    [SerializeField] private Button buyButton; 
    [SerializeField] private Button craftButton;
    [SerializeField] private Transform contentParentTransform;
    [SerializeField] private Transform inventoryParentTransform;

    private GameObject inventoryWrapper;
    private UI_Merchant_ItemLineParent itemLineParentUI;
    private UI_Merchant_CraftLineParent craftLineParentUI;

    private EMerchantType currentType;
    private Merchant Merchant { get; set; }
    private ItemCraft TempItemCraft { get; set; }
    public void InitOnce()
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_ItemLineParent", transform);
        itemLineParentUI = go.GetComponent<UI_Merchant_ItemLineParent>();
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_CraftLineParent", transform);
        craftLineParentUI = go.GetComponent<UI_Merchant_CraftLineParent>();
        craftLineParentUI.InitOnce();
        
        GI.Inst.UIManager.SetNormalButtonColorPreset(buyButton);
        GI.Inst.UIManager.SetNormalButtonColorPreset(craftButton);
    }
    
    public void Open(EMerchantType type, Merchant merchant)
    {
        currentType = type;
        Merchant = merchant;
        
        craftLineParentUI.gameObject.SetActive(false);
        itemLineParentUI.gameObject.SetActive(false);
        
        buyButton.interactable = true;
        craftButton.interactable = true;
        
        GI.Inst.ResourceManager.CreateItemCraft(Define.ELabel.ItemCraft, craft =>
        {
            TempItemCraft = craft;
            
            craftLineParentUI.transform.SetParent(null);
            itemLineParentUI.transform.SetParent(null);
            
            switch (type)
            {
                case EMerchantType.Buy:
                {
                    itemLineParentUI.gameObject.SetActive(true);
                    itemLineParentUI.transform.SetParent(contentParentTransform);
                    itemLineParentUI.rectTransform.localPosition = Vector3.zero;
                    
                    itemLineParentUI.Init(Merchant.GetItemIds());

                    inventoryWrapper = GI.Inst.UIManager.GetInventoryWrapper().gameObject;
                    inventoryWrapper.transform.SetParent(inventoryParentTransform);
                    buyButton.interactable = false;
                }
                    break;
                case EMerchantType.Craft:
                {
                    craftLineParentUI.gameObject.SetActive(true);
                    craftLineParentUI.transform.SetParent(contentParentTransform);
                    craftLineParentUI.transform.localPosition = Vector3.zero;
                    
                    craftLineParentUI.Init(TempItemCraft);
                    
                    inventoryWrapper = GI.Inst.UIManager.GetInventoryWrapper().gameObject;
                    inventoryWrapper.transform.SetParent(inventoryParentTransform);
                    craftButton.interactable = false;
                }
                    break;
            }
        });
    }

    private void CloseMerchant()
    {
        inventoryWrapper.transform.SetParent(null);
        itemLineParentUI.transform.SetParent(null);
        craftLineParentUI.transform.SetParent(null);
        
        craftLineParentUI.Close();
        Destroy(TempItemCraft);
        TempItemCraft = null;
    }
    
    public void OnClickBuyCategoryButton()
    {
        if (currentType == EMerchantType.Buy)
            return;
        Open(EMerchantType.Buy, Merchant);
    }
    
    public void OnClickCraftCategoryButton()
    {
        if (currentType == EMerchantType.Craft)
            return;
        if (!GI.Inst.TutorialManager.ExecuteTutorialIfNotCompleted(ETutorial.ItemCraft))
        {
           //튜토 완료         
        }
        GI.Inst.UIManager.ClearCraftResult();
        Open(EMerchantType.Craft, Merchant);
    }

    public override void Close()
    {
        CloseMerchant();
        gameObject.SetActive(false);
    }
    
    //튜토리얼에서 사용됨
    public void EnableButton(EMerchantType type)
    {
        switch (type)
        {
            case EMerchantType.Buy:
                buyButton.interactable = true;
                break;
            case EMerchantType.Craft:
                craftButton.interactable = true;
                break;
        }
    }

    public void DisableButton(EMerchantType type)
    {
        switch (type)
        {
            case EMerchantType.Buy:
                buyButton.interactable = false;
                break;
            case EMerchantType.Craft:
                craftButton.interactable = false;
                break;
        }
    }

    public Button GetWeaponCraftButton()
    {
        return craftLineParentUI.GetWeaponCraftButton();
    }
}
