using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenuHolderButton : MonoBehaviour
{
    [SerializeField] private Define.EMainMenuType type;


    public void OnClickButton()
    {
        GI.Inst.UIManager.VisibleMenuSetting(type);
    }
}
