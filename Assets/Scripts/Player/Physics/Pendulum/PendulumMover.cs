using UnityEngine;

public class PendulumMover : IPhysicsMove
{
    private Vector3 _direction;
    public void UpdateDirection(Vector3 newDirection) => _direction = newDirection;

    public Vector3 Move(Vector3 input, ref Vector3 totalInputforce)
    {
        return Vector3.Project(input, _direction);
    }
}
