using TMPro;
using UnityEngine;

public class UI_Merchant_Result : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    public void InitOnce()
    {
        Clear();

        GI.Inst.UIManager.setCraftResult -= SetCraftResult;
        GI.Inst.UIManager.setCraftResult += SetCraftResult;
        GI.Inst.UIManager.clearCraftResult -= Clear;
        GI.Inst.UIManager.clearCraftResult += Clear;
    }

    private void SetCraftResult(string itemName)
    {
        resultText.text = $"{itemName} 제작 성공";
    }

    private void Clear()
    {
        resultText.text = "";
    }
}
