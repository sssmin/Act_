using UnityEngine;

public class UI_DungeonSelect : MonoBehaviour
{
    [SerializeField] private Transform attachTransform;
    [SerializeField] public UI_DungeonButtonParent dungeonButtonParentUI;
    public UI_DungeonDescEnterParent dungeonDescEnterParentUI;
    private UI_DungeonTooltip dungeonTooltipUI;
    public void InitOnce()
    {
        GI.Inst.UIManager.getDungeonTooltipUI -= GetDungeonTooltipUI;
        GI.Inst.UIManager.getDungeonTooltipUI += GetDungeonTooltipUI;
        
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_DungeonTooltip", transform);
        dungeonTooltipUI = go.GetComponent<UI_DungeonTooltip>();
        dungeonTooltipUI.gameObject.SetActive(false);

        dungeonButtonParentUI.InitOnce();
        
        go = GI.Inst.ResourceManager.Instantiate("UI_DungeonDescEnter", attachTransform);
        dungeonDescEnterParentUI = go.GetComponent<UI_DungeonDescEnterParent>();
        dungeonDescEnterParentUI.InitOnce();

        GI.Inst.UIManager.onClickDungeonButton -= OnClickDungeonButton;
        GI.Inst.UIManager.onClickDungeonButton += OnClickDungeonButton;
    }

    private void OnDestroy()
    {
        GI.Inst.UIManager.onClickDungeonButton -= OnClickDungeonButton;
        GI.Inst.UIManager.getDungeonTooltipUI -= GetDungeonTooltipUI;
    }

    private UI_DungeonTooltip GetDungeonTooltipUI() => dungeonTooltipUI;
    private void OnClickDungeonButton(EDungeonType type) 
    {
        dungeonButtonParentUI.EnableAllButton();
        dungeonDescEnterParentUI.Refresh(type);
    }
}
