using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option_BindKey : MonoBehaviour
{
    [SerializeField] private EBindKeyType bindKeyType;
    private Button button; 
    private TextMeshProUGUI bindkeyText;

    private void Awake()
    {
        bindkeyText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);

    }

    public void Init(EBindKeyType type)
    {
        bindKeyType = type;
        RefreshUI();
    }
    
    public void OnClickButton()
    {
        GI.Inst.UIManager.SetVisibleBindKeyPopup(true);
        GI.Inst.Player.PlayerController.RebindKey(bindKeyType); 
    }

    public void RefreshUI()
    {
        GI.Inst.UIManager.SetVisibleBindKeyPopup(false);
        bindkeyText.text = GI.Inst.Player.PlayerController.GetBindingKeyString(bindKeyType);
    }
}
