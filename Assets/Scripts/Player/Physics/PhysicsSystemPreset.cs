using UnityEngine;
public class PhysicsSystemPreset
{
    public IVelocityComputer Computer;
    public IGravity Gravity;
    public IPhysicsMove Mover;
    public IVelocityDamper Damper;
    public IVelocityConstrainer Constrainer;

    public PhysicsSystemPreset(IGravity gravity,IPhysicsMove mover, IVelocityDamper damper, IVelocityConstrainer constrainer, IVelocityComputer velocityComputer = null)
    {
        Computer = velocityComputer == null ? new StandardVelocityComputer() : velocityComputer;
        Gravity = gravity;
        Mover = mover;
        Damper = damper;
        Constrainer = constrainer;
    }
}
