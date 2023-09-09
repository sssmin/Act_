using System;
using UnityEngine;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField] private Vector2 joystickSize = new Vector2(200f, 200f);
    [SerializeField] public FloatingJoystick joystick;

    private Finger movementFinger;
    private Vector2 movementAmount;
    private PlayerController PlayerController { get; set; }

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += Touch_onFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= Touch_onFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void Touch_onFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.rectTransform.sizeDelta = joystickSize;
            joystick.rectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPos)
    {
        if (startPos.x < joystickSize.x / 2)
        {
            startPos.x = joystickSize.x / 2;
        }
        
        if (startPos.y < joystickSize.y / 2)
        {
            startPos.y = joystickSize.y / 2;
        }
        else if (startPos.y > Screen.height - joystickSize.y / 2)
        {
            startPos.y = Screen.height - joystickSize.y / 2;
        }

        return startPos;
    }
    
    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
            joystick.knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
            movementAmount = Vector2.zero;
        }
    }
    
    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            Vector2 knobPos;
            float maxMovement = joystickSize.x / 2f;
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, joystick.rectTransform.anchoredPosition) > maxMovement)
            {
                knobPos = (currentTouch.screenPosition - joystick.rectTransform.anchoredPosition).normalized *
                          maxMovement;
            }
            else
            {
                knobPos = currentTouch.screenPosition - joystick.rectTransform.anchoredPosition;
            }

            joystick.knob.anchoredPosition = knobPos;
            movementAmount = knobPos / maxMovement;
        }
    }

    private void Update()
    {
        PlayerController.MoveDir = movementAmount;
    }
}
