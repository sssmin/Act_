using UnityEngine;

public class UI_OptionHolderButton : MonoBehaviour
{
    [SerializeField] private Define.EOptionType type;


    public void OnClickButton()
    {
        GI.Inst.UIManager.VisibleOptionCuzPressedHolderButton(type);
        GI.Inst.SoundManager.SFXPlay("ButtonClick");
    }
}
