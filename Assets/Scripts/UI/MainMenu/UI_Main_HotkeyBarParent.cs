using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_HotkeyBarParent : MonoBehaviour
{
    [SerializeField] private List<UI_BindKey> bindKeys = new List<UI_BindKey>();
    [SerializeField] private Image bgImage;
  
    public void InitOnce()
    {
        //바인딩
        GI.Inst.UIManager.refreshHotKeyMainUI -= Init;
        GI.Inst.UIManager.refreshHotKeyMainUI += Init;
        
        Init();
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.refreshHotKeyMainUI -= Init;
    }

    private void Init()
    {
        foreach (UI_BindKey bindKey in bindKeys)
        {
            bindKey.Init();
        }
    }

    public void VisibleUI()
    {
        foreach (UI_BindKey bindKey in bindKeys)
        {
            bindKey.VisibleKeyText();
        }

        bgImage.color = new Color(1f, 1f, 1f, 60f / 255f);
    }
    
    public void InvisibleUI()
    {
        foreach (UI_BindKey bindKey in bindKeys)
        {
            bindKey.InvisibleKeyText();
        }
        bgImage.color = new Color(1f, 1f, 1f, 0f);
    }
}
