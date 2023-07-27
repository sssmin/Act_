using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup_Enhance : UI_Popup
{
    [SerializeField] private Button enhanceButton;
    [SerializeField] private TextMeshProUGUI enhanceButtonText;
    [SerializeField] private UI_Popup_EnhanceLine enhanceLineUI;
    [SerializeField] private TextMeshProUGUI levelDesc; // +0 >> +1

    private SO_BaseWeapon Original { get; set; }
    private SO_Item WeaponMat { get; set; }
    

    public void InitOnce(SO_BaseWeapon originalWeapon)
    {
        Original = originalWeapon;
        
        GI.Inst.UIManager.SetNormalButtonColorPreset(enhanceButton);
        enhanceButtonText.color = new Color(170f / 255f, 170f / 255f, 170f / 255f, 1f);
        
        enhanceButton.interactable = false;
        
        levelDesc.text = $"+{originalWeapon.EnhanceLevel} >> +{originalWeapon.EnhanceLevel + 1}";
        enhanceLineUI.InitOnce(originalWeapon);
        enhanceButton.onClick.RemoveListener(OnClickEnhanceButton);
        enhanceButton.onClick.AddListener(OnClickEnhanceButton);
        GI.Inst.UIManager.setSameEquipment += SetSameEquipment;
        
        GI.Inst.UIManager.MainMenuUI.InventoryStatus = EInventoryStatus.Enhance;
        //무기 카테고리만 활성화
        GI.Inst.UIManager.DisableInvenCategoryBtnCantEnhance();
        //강화 가능한 무기 재료만 활성화
        GI.Inst.UIManager.EnableCanWeaponMat(originalWeapon);
    }
    
    private void OnDestroy()
    {
        enhanceButton.onClick.RemoveListener(OnClickEnhanceButton);
        GI.Inst.UIManager.setSameEquipment -= SetSameEquipment;
        
        GI.Inst.UIManager.MainMenuUI.InventoryStatus = EInventoryStatus.Default;
        //무기 카테고리만 비활성화
        GI.Inst.UIManager.EnableCategoryButton();
        //슬롯 리프레쉬
        GI.Inst.UIManager.RefreshInventoryUI();
    }
    
    private void SetSameEquipment(SO_Item sameWeapon)
    {
        WeaponMat = sameWeapon;
        
        enhanceButton.interactable = true;
        enhanceButtonText.color = Color.white;
        enhanceLineUI.SetSameEquipment(sameWeapon as SO_BaseWeapon);
    }
    
    private void OnClickEnhanceButton()
    {
        GI.Inst.ListenerManager.Enhance(Original, WeaponMat);
        GI.Inst.UIManager.ClosePopup();
    }
    
    public override void Close()
    {
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
