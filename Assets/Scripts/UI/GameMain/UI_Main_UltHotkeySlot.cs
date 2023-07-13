using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_UltHotkeySlot : UI_Main_SkillHotkeySlotBase
{
    [SerializeField] private TextMeshProUGUI chargeAmountText;
    [SerializeField] private Image chargeImage;
    private float Amount { get; set; }
 
    public override void InitOnce()
    {
        float amount = GI.Inst.ListenerManager.GetUltSkillChargeAmount();
        UpdateFillAmount(amount);
    }
    
    public override void SetSkillIcon(Sprite icon)
    {
        base.SetSkillIcon(icon);
         
        chargeImage.color = new Color(36f / 255f, 36 / 255f, 36 / 255f, 163f / 255f);
        chargeAmountText.text = Mathf.FloorToInt(Amount).ToString();
    }

    public override void Clear()
    {
        base.Clear();
        
        chargeImage.color = Color.clear;
        chargeAmountText.text = "";
    }

    public void UpdateFillAmount(float chargeAmount)
    {
        chargeImage.fillAmount =  1 - chargeAmount * 0.01f;
        chargeAmountText.text = Mathf.FloorToInt(chargeAmount).ToString();
        Amount = chargeAmount;
    }

    

    

   
}