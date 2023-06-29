using UnityEngine;
using UnityEngine.UI;

public class UI_Skill_MoveSkillIcon : MonoBehaviour//, IDropHandler
{
    private Image icon; 
    private void Start()
    {
        icon = GetComponent<Image>();
        icon.raycastTarget = false;
    }

    // public void OnDrop(PointerEventData eventData)
    // {
    //     Debug.Log("Move OnDrop");
    //     GI.Inst.ResourceManager.Destroy(gameObject);
    // }
}
