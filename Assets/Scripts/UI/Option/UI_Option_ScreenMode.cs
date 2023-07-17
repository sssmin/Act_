using TMPro;
using UnityEngine;

public class UI_Option_ScreenMode : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.RemoveListener(DropdownChanged);
        dropdown.onValueChanged.AddListener(DropdownChanged);

        string value = GI.Inst.IsFullscreen ? "전체 화면" : "창모드";
        foreach (var option in dropdown.options)
        {
            if (option.text == value)
            {
                dropdown.value = dropdown.options.IndexOf(option);
                break;
            }
        }
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(DropdownChanged);
    }

    private void DropdownChanged(int value)
    {
        if (dropdown.options[value].text == "전체 화면")
        {
            Debug.Log("전체화면 세팅");
            GI.Inst.ApplyScreenMode(true);
        }
        else
        {Debug.Log("창모드 세팅");
            GI.Inst.ApplyScreenMode(false);
        }
    }
}
