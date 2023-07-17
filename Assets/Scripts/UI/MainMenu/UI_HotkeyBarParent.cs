using System.Collections.Generic;
using UnityEngine;

public class UI_HotkeyBarParent : MonoBehaviour
{
    [SerializeField] private List<UI_BindKey> bindKeys = new List<UI_BindKey>();
    
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
}
