using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Action beginOverlapCallback;
    private Action endOverlapCallback;
    
    public void InitOnce(Action beginCallback, Action endCallback)
    {
        beginOverlapCallback = beginCallback;
        endOverlapCallback = endCallback;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        beginOverlapCallback?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        endOverlapCallback?.Invoke();
    }
}
