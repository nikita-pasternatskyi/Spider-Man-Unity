using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IJumpInputProvider, IMovementInputProvider
{
    public Controls Controls { get; private set; }
    public Vector2 MousePosition { get; private set; }

    public event Action<Vector2> MouseMoved;
    public event Action<Vector2> MovementPressed;
    public event Action JumpPressed;
    public event Action SwingPressed;
    public event Action<bool> ModifierPressed;

    private void Awake()
    {
        Controls = new Controls();
    }

    private void OnEnable()
    {
        Controls.Enable();
        Controls.Gameplay.Movement.performed += _ctx => MovementPressed?.Invoke(_ctx.ReadValue<Vector2>());
        Controls.Gameplay.Movement.canceled += _ctx => MovementPressed?.Invoke(_ctx.ReadValue<Vector2>());

        Controls.Gameplay.MouseLook.performed += _ctx => OnMouseMoved(_ctx.ReadValue<Vector2>());
        Controls.Gameplay.MouseLook.canceled += _ctx => OnMouseMoved(_ctx.ReadValue<Vector2>());

        Controls.Gameplay.Jump.performed += _ctx => JumpPressed?.Invoke();

        Controls.Gameplay.ThrowWeb.performed += _ctx => SwingPressed?.Invoke();

        Controls.Gameplay.Modifier.performed += _ctx => ModifierPressed?.Invoke(_ctx.performed);
        Controls.Gameplay.Modifier.canceled += _ctx => ModifierPressed?.Invoke(_ctx.performed);
    }

    private void OnMouseMoved(Vector2 obj)
    {
        MousePosition = obj;
        MouseMoved?.Invoke(obj);
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
