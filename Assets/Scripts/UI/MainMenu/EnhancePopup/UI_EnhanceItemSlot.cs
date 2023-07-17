using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnhanceItemSlot : MonoBehaviour
{
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private Image elementIcon;
    [SerializeField] private TextMeshProUGUI enhanceLevelText;

    public void InitOnce(BaseWeapon weapon)
    {
        Clear();
        if (weapon == null) return;
        equipmentIcon.color = Color.white;
        equipmentIcon.sprite = weapon.itemIcon;
        if (weapon.Element != EWeaponElement.None)
        {
            string elementName = Enum.GetName(typeof(EWeaponElement), weapon.Element);
            elementIcon.color = Color.white;
            elementIcon.sprite = GI.Inst.ResourceManager.GetStatusSprite(elementName);
        }

        enhanceLevelText.text = $"+{weapon.EnhanceLevel}";
    }

    private void Clear()
    {
        equipmentIcon.color = Color.clear;
        equipmentIcon.sprite = null;
        elementIcon.color = Color.clear;
        elementIcon.sprite = null;
        enhanceLevelText.text = "";
    }
}
