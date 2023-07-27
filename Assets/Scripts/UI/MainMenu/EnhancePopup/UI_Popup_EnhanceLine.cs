using UnityEngine;

public class UI_Popup_EnhanceLine : MonoBehaviour
{
    [SerializeField] private UI_EnhanceItemSlot originalEquipmentSlot;
    [SerializeField] private UI_EnhanceItemSlot sameEquipmentSlot;

    public void InitOnce(SO_BaseWeapon originalWeapon)
    {
        originalEquipmentSlot.InitOnce(originalWeapon);
    }

    public void SetSameEquipment(SO_BaseWeapon sameWeapon)
    {
        sameEquipmentSlot.InitOnce(sameWeapon);
    }
}
