using UnityEngine;

public interface IPlayerPhysicsState
{
    public void Enter();
    public void Exit();
    public void Jump(Vector3 force);
    public void Move(Vector3 input);
    public void AddForce(Vector3 force);
    public Vector3 FixedUpdate(Vector3 currentPosition, float time);
}
