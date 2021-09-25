using UnityEngine;

public interface IPlayerPhysicsState
{
    public void Start();
    public void Jump(Vector3 force);
    public void Move(Vector3 input);
    public void FixedUpdate(Vector3 currentPosition, float time);
}
