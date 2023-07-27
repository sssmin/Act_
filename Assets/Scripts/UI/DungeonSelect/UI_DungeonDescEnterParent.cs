using TMPro;
using UnityEngine;

public class UI_DungeonDescEnterParent : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI descText;
    [SerializeField] public UI_DungeonEnterParent dungeonEnterParentUI;

    public void InitOnce()
    {
        SO_DungeonInfo info = GI.Inst.ResourceManager.GetDungeonInfo(0);
        if (info)
        {
            descText.text = info.desc;
            dungeonEnterParentUI.InitOnce(info, 0);
        }
    }
    
    public void Refresh(EDungeonType type)
    {
        SO_DungeonInfo info = GI.Inst.ResourceManager.GetDungeonInfo(type);
        if (info)
        {
            descText.text = info.desc;
            dungeonEnterParentUI.Init(info, type);
        }
        else
        {
            descText.text = "준비 중";
            dungeonEnterParentUI.Init(info, EDungeonType.None);
        }
    }
}
