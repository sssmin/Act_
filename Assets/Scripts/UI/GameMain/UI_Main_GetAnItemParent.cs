using UnityEngine;

public class UI_Main_GetAnItemParent : MonoBehaviour
{
    public void InitOnce()
    {
        GI.Inst.UIManager.createGetAnItemSlot -= CreateGetAnItemSlot;
        GI.Inst.UIManager.createGetAnItemSlot += CreateGetAnItemSlot;
        GI.Inst.UIManager.createGetAnGoldSlot -= CreateGetAnGoldSlot;
        GI.Inst.UIManager.createGetAnGoldSlot += CreateGetAnGoldSlot;
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.createGetAnItemSlot -= CreateGetAnItemSlot;
        GI.Inst.UIManager.createGetAnGoldSlot -= CreateGetAnGoldSlot;
    }

    private void CreateGetAnItemSlot(Sprite icon, string itemName, string amount)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_GetAnItemSlot", transform);
        UI_Main_GetAnItemSlot getAnItemSlotUI = go.GetComponent<UI_Main_GetAnItemSlot>();
        getAnItemSlotUI.SetInfo(icon, itemName, amount);
    }

    private void CreateGetAnGoldSlot(Sprite icon, string gold)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_GetAnItemSlot", transform);
        UI_Main_GetAnItemSlot getAnItemSlotUI = go.GetComponent<UI_Main_GetAnItemSlot>();
        getAnItemSlotUI.SetGoldInfo(icon, gold);
    }

}
