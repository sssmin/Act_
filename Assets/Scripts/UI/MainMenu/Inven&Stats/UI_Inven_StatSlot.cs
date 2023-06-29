using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Inven_StatSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;
    

    public void Init(Stats stats, Define.EStatType statType)
    {
        string name = "";
        string value = "";
        Util.ConvertStatString(stats, statType, out name, out value);

        statName.text = name;
        statValue.text = value;
        
    }
}
