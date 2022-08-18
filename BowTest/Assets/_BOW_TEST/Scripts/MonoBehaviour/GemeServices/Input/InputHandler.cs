using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    public event Action OnPressedEvent;
    public event Action OnReleaseEvent;
    
    private bool _isPressed;
    private bool _isBlocked;

    public bool IsPressed
    {
        get => _isPressed;
        set
        {
            if (IsBlocked) return;

            _isPressed = value;
            if (value) OnPressedEvent?.Invoke();
            else OnReleaseEvent?.Invoke();
        }
    }

    public bool IsBlocked
    {
        get => _isBlocked;
        set
        {
            if(value == false && IsPressed) IsPressed = false;

            _isBlocked = value;
        }
    }
}
