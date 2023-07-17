using TMPro;
using UnityEngine;

public class UI_ItemTooltip_DescText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descText;

    public void Init(string desc)
    {
        string[] sub = desc.Split("Y");

        if (sub.Length > 1)
        {
            descText.text = sub[1];
            descText.color = Color.yellow;
        }
        else
        {
            descText.text = desc;
            descText.color = Color.white;
        }

    }
}
