using UnityEngine;
using UnityEngine.UI;

public class UI_Skill_MoveSkillIcon : MonoBehaviour
{
    private Image icon; 
    private void Start()
    {
        icon = GetComponent<Image>();
        icon.raycastTarget = false;
    }
}
