using UnityEngine;

public class UI_MainMenuHolderButton : MonoBehaviour
{
    [SerializeField] private Define.EMainMenuType type;


    public void OnClickButton()
    {
        GI.Inst.UIManager.VisibleMainMenuSetting(type);
        GI.Inst.SoundManager.PlayEffectSound("ButtonClick");
    }
}
