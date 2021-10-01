using UnityEngine;

public interface IPhysicsMove
{
    public Vector3 Move(Vector3 input, ref Vector3 totalInputForce);
}
