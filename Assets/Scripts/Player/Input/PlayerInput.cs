using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MousePosition { get; private set; }
    public Vector3 MovementInput { get; private set; }

    public event Action<Vector2> MouseMoved;
    public event Action<Vector3> MovementPressed;
    public event Action JumpPressed;
    public event Action<bool> SwingPressed;
    public event Action<bool> ModifierPressed;

    private List<IInputListener> _listeners;
    private Controls _actionMap;

    private void Awake()
    {
        _listeners = new List<IInputListener>();
        _actionMap = new Controls();
    }

    private void OnEnable()
    {
        _actionMap.Enable();
        _actionMap.Gameplay.Movement.performed += _ctx => OnMovementPressed(_ctx.ReadValue<Vector2>());
        _actionMap.Gameplay.Movement.canceled += _ctx => OnMovementPressed(_ctx.ReadValue<Vector2>());

        _actionMap.Gameplay.MouseLook.performed += _ctx => OnMouseMoved(_ctx.ReadValue<Vector2>());
        _actionMap.Gameplay.MouseLook.canceled += _ctx => OnMouseMoved(_ctx.ReadValue<Vector2>());

        _actionMap.Gameplay.Jump.performed += _ctx => JumpPressed?.Invoke();

        _actionMap.Gameplay.ThrowWeb.performed += _ctx => SwingPressed?.Invoke(_ctx.performed);
        _actionMap.Gameplay.ThrowWeb.canceled += _ctx => SwingPressed?.Invoke(_ctx.performed);

        _actionMap.Gameplay.Modifier.performed += _ctx => ModifierPressed?.Invoke(_ctx.performed);
        _actionMap.Gameplay.Modifier.canceled += _ctx => ModifierPressed?.Invoke(_ctx.performed);
    }

    private void OnDisable()
    {
        foreach (var item in _listeners)
        {
            MouseMoved -= item.OnMouseMoved;
            MovementPressed -= item.OnMoved;
            JumpPressed -= item.OnJumpPressed;
            SwingPressed -= item.OnSwingPressed;
            ModifierPressed -= item.OnModifierPressed;
        }
        _actionMap.Disable();
    }

    public void AddListener(IInputListener listener)
    {
        MouseMoved += listener.OnMouseMoved;
        MovementPressed += listener.OnMoved;
        JumpPressed += listener.OnJumpPressed;
        SwingPressed += listener.OnSwingPressed;
        ModifierPressed += listener.OnModifierPressed;
    }

    public void RemoveListener(IInputListener listener)
    {
        MouseMoved -= listener.OnMouseMoved;
        MovementPressed -= listener.OnMoved;
        JumpPressed -= listener.OnJumpPressed;
        SwingPressed -= listener.OnSwingPressed;
        ModifierPressed -= listener.OnModifierPressed;
    }

    private void OnMouseMoved(Vector2 obj)
    {
        MousePosition = obj;
        MouseMoved?.Invoke(obj);
    }

    private void OnMovementPressed(Vector2 obj)
    {
        MovementInput = new Vector3(obj.x, 0, obj.y);
        MovementPressed?.Invoke(MovementInput);
    }
}
