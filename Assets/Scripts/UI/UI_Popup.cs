using UnityEngine;

public enum EInventoryPopupType
{
    EquippedEquipment, //장착된 장비
    Equipment, //장비
    Consumable, //소모품
    Etc //기타
}

public enum EThrowawayBuyPopupType
{
    ThrowAway, //소모품, 기타 버리기
    ThrowAwayConfirm, //장비 버리기 컨펌
    Buy, //구매
}

public abstract class UI_Popup : MonoBehaviour
{
    public abstract void Close();
}
