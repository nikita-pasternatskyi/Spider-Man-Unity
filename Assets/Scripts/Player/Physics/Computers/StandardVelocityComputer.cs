using UnityEngine;

public class StandardVelocityComputer : IVelocityComputer
{
    public Vector3 Compute(ref Vector3 forces, in Vector3 movementInput, in float time, IGravity gravity, IPhysicsMove mover, IVelocityDamper velocityDamper, IVelocityConstrainer velocityConstrainer)
    {
        forces += gravity.CalculateGravity(time);
        velocityDamper.DampVelocity(ref forces, time);
        velocityConstrainer.ConstrainVelocity(ref forces, time);
        return forces + movementInput;
    }
}
