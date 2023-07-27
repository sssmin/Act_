using System.Collections.Generic;
using UnityEngine;

public class UI_Merchant_ItemLineParent : MonoBehaviour
{
    private List<UI_Merchant_ItemLine> itemLines = new List<UI_Merchant_ItemLine>();
    private List<SO_Item> items = new List<SO_Item>();
    [SerializeField] public RectTransform rectTransform;

    public void Init(List<string> inItems)
    {
        Clear();
        foreach (string itemId in inItems)
        {
            SO_Item item = GI.Inst.ResourceManager.GetItemData(itemId);
            items.Add(item);
        }

        InitItemLines();
    }
    
    private void InitItemLines()
    {
        items.Sort();
        foreach (SO_Item item in items)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Merchant_ItemLine", transform);
            UI_Merchant_ItemLine itemLine = go.GetComponent<UI_Merchant_ItemLine>();
            itemLines.Add(itemLine);
            itemLine.Init(item);
        }
    }
    
    private void Clear()
    {
        foreach (UI_Merchant_ItemLine merchantItemLine in itemLines)
        {
            GI.Inst.ResourceManager.Destroy(merchantItemLine.gameObject);
        }
        items.Clear();
        itemLines.Clear();
    }
    
    
}
