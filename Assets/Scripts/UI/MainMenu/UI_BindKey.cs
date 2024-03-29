using TMPro;
using UnityEngine;

public class UI_BindKey : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private EBindKeyType bindKeyType;

    public void Init()
    {
        keyText.text = GI.Inst.Player.PlayerController.GetBindingKeyString(bindKeyType);
    }

    public void VisibleKeyText()
    {
        keyText.color = Color.white;
    }

    public void InvisibleKeyText()
    {
        keyText.color = Color.clear;
    }
}
