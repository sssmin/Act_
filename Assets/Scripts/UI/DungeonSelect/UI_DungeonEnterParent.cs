using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DungeonEnterParent : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button enterButton;
    private EDungeonType dungeonType;
    private DungeonInfoDetail detail;

    public void InitOnce(SO_DungeonInfo info, EDungeonType type)
    {
        Init(info, type);
        
        dropdown.onValueChanged.RemoveListener(DropdownChanged);
        dropdown.onValueChanged.AddListener(DropdownChanged);
        
        enterButton.onClick.RemoveListener(OnClickEnterButton);
        enterButton.onClick.AddListener(OnClickEnterButton);
    }
    
    public void Init(SO_DungeonInfo info, EDungeonType type)
    {
        dungeonType = type;
        enterButton.interactable = true;

        dropdown.options.Clear();

        if (type == EDungeonType.None)
        {
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData("...");
            dropdown.options.Add(newOption);
            enterButton.interactable = false;
        }
        else
        {
            foreach (DungeonInfoDetail infoDetail in info.dungeonInfoDetails)
            {
                if (infoDetail.dungeonCategory == EDungeonCategory.Normal)
                {
                    TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData("Lv." + infoDetail.dungeonLevel);
                    dropdown.options.Add(newOption);
                }
                else if (infoDetail.dungeonCategory == EDungeonCategory.Boss)
                {
                    TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData("보스");
                    dropdown.options.Add(newOption);
                }
            }
        }
        dropdown.RefreshShownValue();
        dropdown.value = 0;
        detail = info.dungeonInfoDetails[0];
        GI.Inst.DungeonManager.SelectDungeon(detail, dungeonType);
    }
    
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(DropdownChanged);
        enterButton.onClick.RemoveListener(OnClickEnterButton);
    }

    private void DropdownChanged(int value)
    {
        enterButton.interactable = true;
        
        SO_DungeonInfo info = GI.Inst.ResourceManager.GetDungeonInfo(dungeonType);
        
        if (value != 0 && !info.dungeonInfoDetails[value - 1].isLevelCompleted)
        {
            enterButton.interactable = false;
        }
        detail = info.dungeonInfoDetails[value];
        GI.Inst.DungeonManager.SelectDungeon(detail, dungeonType);
    }

    private void OnClickEnterButton()
    { 
        GI.Inst.UIManager.DestroyDungeonSelectUI();
        GI.Inst.UIManager.FadeOut(() =>
        {
            if (detail.dungeonCategory == EDungeonCategory.Normal)
                GI.Inst.SceneLoadManager.RequestLoadSceneAsync("Dungeon", 0f);
            else if (detail.dungeonCategory == EDungeonCategory.Boss)
                GI.Inst.SceneLoadManager.RequestLoadSceneAsync("Boss", 0f);
        });

    }
    
    
}
