using TMPro;
using UnityEngine;


public class UI_Option_Resolution : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.RemoveListener(DropdownChanged);
        dropdown.onValueChanged.AddListener(DropdownChanged);
        int width = GI.Inst.ScreenWidth;
        int height = GI.Inst.ScreenHeight;
        
        foreach (var option in dropdown.options)
        {
            if (option.text == $"{width}x{height}")
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
        string[] values = (dropdown.options[value].text).Split('x');

        int width = int.Parse(values[0]);
        int height = int.Parse(values[1]);
        GI.Inst.ApplyResolution(width, height);
    }
}
