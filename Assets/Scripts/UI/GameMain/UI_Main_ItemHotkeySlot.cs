using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_ItemHotkeySlot : MonoBehaviour
{
    [SerializeField] private  Image itemIconImage;
    [SerializeField] private  TextMeshProUGUI bindKeyText;
    [SerializeField] private  Image cooltimeImage;
    [HideInInspector] public string itemId = "";//없는 상태는 빈칸으로, 쿨타임 표시 위함
    [SerializeField] protected TextMeshProUGUI itemAmount;
    [SerializeField] private Sprite defaultSlotSprite;

    
    
    private void Update()
    {
        if (itemId != "")
            cooltimeImage.fillAmount = GI.Inst.CooltimeManager.GetItemIconFillAmount(itemId);
    }

    public void Init(Item item)
    {
        if (item.amount <= 0)
        {
            Clear();
            return;
        }
        itemId = item.itemId;
        itemIconImage.sprite = item.itemIcon;
        itemAmount.text =  $"x{item.amount:#,0}";
    }

    public void Clear()
    {
        itemId = "";
        itemIconImage.sprite = defaultSlotSprite;
        itemAmount.text = "";
    }
}