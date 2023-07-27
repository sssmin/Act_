using System.Collections.Generic;
using UnityEngine;

public class UI_DungeonButtonParent : MonoBehaviour
{
    private Dictionary<EDungeonType, UI_DungeonButton> dungeonButtonUIs = new Dictionary<EDungeonType, UI_DungeonButton>();

    public void InitOnce()
    {
        for (int i = 0; i < (int)EDungeonType.Max; i++)
        {
            GameObject go = GI.Inst.ResourceManager.Instantiate("UI_DungeonButton", transform);
            UI_DungeonButton dungeonButtonUI = go.GetComponent<UI_DungeonButton>();
            dungeonButtonUI.InitOnce((EDungeonType)i);
            if (!dungeonButtonUIs.ContainsKey((EDungeonType)i))
            {
                dungeonButtonUIs.Add((EDungeonType)i, dungeonButtonUI);
            }

            if (i == 0)
            {
                dungeonButtonUI.DisableButton();
            }
        }
    }
    
    public void EnableAllButton()
    {
        foreach (KeyValuePair<EDungeonType,UI_DungeonButton> pair in dungeonButtonUIs)
        {
            pair.Value.EnableButton();
        }
    }
    
}
