using UnityEngine;
public class PhysicsSystemPreset
{
    public IGravity Gravity;
    public IPhysicsMove Mover;
    public IVelocityDamper Damper;
    public IVelocityConstrainer Constrainer;

    public PhysicsSystemPreset(IGravity gravity,IPhysicsMove mover, IVelocityDamper damper, IVelocityConstrainer constrainer)
    {
        Gravity = gravity;
        Mover = mover;
        Damper = damper;
        Constrainer = constrainer;
    }
}
