using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class FloatingJoystick : MonoBehaviour
{
    [HideInInspector] public RectTransform rectTransform;
    public RectTransform knob;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
