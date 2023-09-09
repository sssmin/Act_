using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] RectTransform contentWrapper; //배치는 얘로
    [SerializeField] RectTransform contentParent;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] TextMeshProUGUI rarityText;
    private List<UI_ItemTooltip_ItemStats> itemStatsList = new List<UI_ItemTooltip_ItemStats>();
    private List<UI_ItemTooltip_DescText> descTexts = new List<UI_ItemTooltip_DescText>();
    private GameObject EffectDescObject { get; set; }

    private void Awake()
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
    }

    public void Init(SO_Item item, Vector3 slotPos, int pivot)
    {
        AllClear();
        InitSize(slotPos, pivot);

        typeText.text = Util.ConvertItemCategoryString(item.ItemCategory);
        itemNameText.text = item.itemName;
        
        SO_Equipment equipment = item as SO_Equipment;
        if (equipment)
        {
            rarityText.text = Util.ConvertRarityString(equipment.rarity);
            rarityText.color = Util.GetRarityColor(equipment.rarity);
            if (equipment.itemStats.Count > 0)
                InitItemStats(equipment);
            
            if (equipment.effectDescs.Count > 0)
            {
                EffectDescObject = GI.Inst.ResourceManager.Instantiate("UI_ItemTooltip_EffectDescObject", contentParent.transform);
                RectTransform effectDescObjRect = EffectDescObject.GetComponent<RectTransform>();
                if (effectDescObjRect)
                    effectDescObjRect.localScale = Vector3.one;
                IncreaseWrapperSize();
                InitEffectDescs(equipment);
            }
        }
        else
        {
            rarityText.text = "";

            InitDescText(item.itemDesc);
        }
    }

    private void InitDescText(string desc)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_ItemTooltip_DescText", contentParent.transform);
        UI_ItemTooltip_DescText itemTooltipDescText = go.GetComponent<UI_ItemTooltip_DescText>();
        itemTooltipDescText.Init(desc);
        descTexts.Add(itemTooltipDescText);
        IncreaseWrapperSize();
    }

    private void InitEffectDescs(SO_Equipment equipment)
    {
        foreach (string effectDesc in equipment.effectDescs)
        {
            InitDescText(effectDesc);
        }
    }

    private void InitItemStats(SO_Equipment equipment)
    {
        foreach (Stat itemStat in equipment.itemStats)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_ItemTooltip_ItemStats", contentParent.transform);
            UI_ItemTooltip_ItemStats itemTooltipItemStats = go.GetComponent<UI_ItemTooltip_ItemStats>();
            string name = "";
            string value = "";
            Util.ConvertStatString(itemStat, out name, out value);
            itemTooltipItemStats.Init(name, value);
            itemStatsList.Add(itemTooltipItemStats);
            
            IncreaseWrapperSize();
        }
    }

    private void InitSize(Vector3 slotPos, int pivot)
    {
        contentWrapper.pivot = new Vector2(pivot, 0.5f);
        contentWrapper.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 250f);
        contentParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 250f);
        contentWrapper.transform.position = slotPos;
    }

    private void IncreaseWrapperSize()
    {
        contentWrapper.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentWrapper.rect.height + 50f);
        contentParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentParent.rect.height + 50f);
    }

    private void AllClear()
    {
        foreach (UI_ItemTooltip_ItemStats itemStats in itemStatsList)
        {
            GI.Inst.ResourceManager.Destroy(itemStats.gameObject);
        }
        itemStatsList.Clear();
        if (EffectDescObject)
        {
            GI.Inst.ResourceManager.Destroy(EffectDescObject);
            EffectDescObject = null;
        }

        foreach (UI_ItemTooltip_DescText descText in descTexts)
        {
            GI.Inst.ResourceManager.Destroy(descText.gameObject);
        }
        descTexts.Clear();
    }
}
