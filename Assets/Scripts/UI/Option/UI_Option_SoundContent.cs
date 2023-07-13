using System;
using System.Collections.Generic;
using UnityEngine;



public class UI_Option_SoundContent : MonoBehaviour
{
    [SerializeField] private List<UI_Option_SoundLine> soundLines = new List<UI_Option_SoundLine>();

    public void InitOnce()
    {
        foreach (UI_Option_SoundLine soundLine in soundLines)
        {
            soundLine.InitOnce();
        }
    }

    private void OnDisable()
    {
        GI.Inst.SaveSoundData();
    }
}
