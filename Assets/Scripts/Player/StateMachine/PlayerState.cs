using UnityEngine;

public class PlayerState : IInputListener
{
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void OnJumpPressed() { }
    public virtual void OnModifierPressed(bool obj) { }
    public virtual void OnMouseMoved(Vector2 input) { }
    public virtual void OnMoved(Vector3 moved) { }
    public virtual void OnSwingPressed(bool obj) { }
}


