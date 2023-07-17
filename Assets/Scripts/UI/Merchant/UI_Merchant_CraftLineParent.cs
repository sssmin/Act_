using UnityEngine;

public class UI_Merchant_CraftLineParent : MonoBehaviour
{
    private UI_Merchant_CraftLine WeaponCraftLine;
    private UI_Merchant_CraftLine ArmorCraftLine;
    private UI_Merchant_CraftLine AccCraftLine;
    private UI_Merchant_Result resultLine;
    
    
    public void InitOnce()
    {
        GI.Inst.UIManager.refreshCraftLines -= RefreshCraftLines;
        GI.Inst.UIManager.refreshCraftLines += RefreshCraftLines;
        
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_CraftLine", transform);
        WeaponCraftLine = go.GetComponent<UI_Merchant_CraftLine>();
        WeaponCraftLine.InitOnce(Item.EItemCategory.Weapon);
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_CraftLine", transform);
        ArmorCraftLine = go.GetComponent<UI_Merchant_CraftLine>();
        ArmorCraftLine.InitOnce(Item.EItemCategory.Armor);
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_CraftLine", transform);
        AccCraftLine = go.GetComponent<UI_Merchant_CraftLine>();
        AccCraftLine.InitOnce(Item.EItemCategory.Acc);
        
        go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_Result", transform);
        resultLine = go.GetComponent<UI_Merchant_Result>();
        resultLine.InitOnce();
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.refreshCraftLines -= RefreshCraftLines;
    }

    public void Init(ItemCraft itemCraft)
    {
        WeaponCraftLine.Init(itemCraft);
        ArmorCraftLine.Init(itemCraft);
        AccCraftLine.Init(itemCraft);
    }

    private void RefreshCraftLines()
    {
        WeaponCraftLine.RefreshCraftLine();
        ArmorCraftLine.RefreshCraftLine();
        AccCraftLine.RefreshCraftLine();
    }

    public void Close()
    {
        WeaponCraftLine.Close();
        ArmorCraftLine.Close();
        AccCraftLine.Close();
    }
    
    
    
    
    
    
}
