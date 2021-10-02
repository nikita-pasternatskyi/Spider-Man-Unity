using UnityEngine;

public interface IInputListener
{
    public void OnMouseMoved(Vector2 input);
    public void OnMoved(Vector3 moved);
    public void OnJumpPressed();
    public void OnSwingPressed(bool obj);
    public void OnModifierPressed(bool obj);
}
