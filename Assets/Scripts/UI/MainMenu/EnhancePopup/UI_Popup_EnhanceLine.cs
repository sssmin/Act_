using UnityEngine;

public class UI_Popup_EnhanceLine : MonoBehaviour
{
    [SerializeField] private UI_EnhanceItemSlot originalEquipmentSlot;
    [SerializeField] private UI_EnhanceItemSlot sameEquipmentSlot;

    public void InitOnce(BaseWeapon originalWeapon)
    {
        originalEquipmentSlot.InitOnce(originalWeapon);
    }

    public void SetSameEquipment(BaseWeapon sameWeapon)
    {
        sameEquipmentSlot.InitOnce(sameWeapon);
    }
}
