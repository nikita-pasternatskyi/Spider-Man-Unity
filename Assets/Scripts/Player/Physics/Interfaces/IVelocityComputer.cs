using UnityEngine;

public interface IVelocityComputer
{
    public Vector3 Compute(ref Vector3 forces, in Vector3 movementInput, in float time, IGravity gravity, IPhysicsMove mover, IVelocityDamper velocityDamper, IVelocityConstrainer velocityConstrainer);
}
