using UnityEngine;

public class PhysicsSystem
{
    public Vector3 Forces;

    private Vector3 _moveInput;
    private IGravity _gravity;
    private IPhysicsMove _mover;
    private IVelocityDamper _velocityDamper;
    private IVelocityConstrainer _velocityConstrainer;

    public PhysicsSystem(IGravity gravity, IPhysicsMove move, IVelocityConstrainer velocityConstrainer, IVelocityDamper velocityDamper)
    {
        _gravity = gravity;
        _mover = move;
        _velocityConstrainer = velocityConstrainer;
        _velocityDamper = velocityDamper;
    }

    public PhysicsSystem()
    { }

    public PhysicsSystem(PhysicsSystemPreset physicsSystemMode)
    {
        _gravity = physicsSystemMode.Gravity;
        _mover = physicsSystemMode.Mover;
        _velocityConstrainer = physicsSystemMode.Constrainer;
        _velocityDamper = physicsSystemMode.Damper;
    }

    public void ResumeFromForce(Vector3 force)
    {
        Forces = force;
    }

    public Vector3 Compute(float time)
    {
        Forces += _gravity.CalculateGravity(time);
        _velocityDamper.DampVelocity(ref Forces, time);
        _velocityConstrainer.ConstrainVelocity(ref Forces, time);
        return Forces;
    }

    public void Move(Vector3 input) => _moveInput = _mover.Move(input, ref _moveInput);

    public void AddForce(Vector3 force) => Forces += force;

    public void ChangePreset(PhysicsSystemPreset preset)
    {
        _gravity = preset.Gravity;
        _mover = preset.Mover;
        _velocityConstrainer = preset.Constrainer;
        _velocityDamper = preset.Damper;
    }

    public void ChangeGravity(IGravity newGravity) => _gravity = newGravity;

    public void ChangePhysicsMove(IPhysicsMove newPhysicsMove) => _mover = newPhysicsMove;

    public void ChangeVelocityDamper(IVelocityDamper newVelocityDamper) => _velocityDamper = newVelocityDamper;

    public void ChangeVelocityConstrainer(IVelocityConstrainer newVelocityConstrainer) => _velocityConstrainer = newVelocityConstrainer;

}
