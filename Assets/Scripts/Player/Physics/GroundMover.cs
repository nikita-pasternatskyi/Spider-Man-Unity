using UnityEngine;

public class GroundMover : IPhysicsMove
{
    private PlayerGround _ground;

    public GroundMover(PlayerGround ground)
    {
        _ground = ground;
    }

    public Vector3 Move(Vector3 input, ref Vector3 totalInputForce)
    {
        if (input == Vector3.zero)
        {
            return Vector3.zero;
        }

        totalInputForce += input;
        var difference = input - totalInputForce;

        if (totalInputForce != input)
        {
            totalInputForce += difference;
        }
        return _ground.Project(totalInputForce);
    }
}
