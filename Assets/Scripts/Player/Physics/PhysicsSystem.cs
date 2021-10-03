using UnityEngine;

public class PhysicsSystem
{
    public Vector3 Forces;

    private Vector3 _moveInput;
    private IVelocityComputer _velocityComputer;
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
    {
        _velocityComputer = new StandardVelocityComputer();
    }

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
        return _velocityComputer.Compute(ref Forces, _moveInput, time, _gravity, _mover, _velocityDamper, _velocityConstrainer);
    }

    public void Move(Vector3 input) => _moveInput = _mover.Move(input, ref _moveInput);

    public void AddForce(Vector3 force) => Forces += force;

    public void ChangePreset(PhysicsSystemPreset preset)
    {
        _gravity = preset.Gravity;
        _mover = preset.Mover;
        _velocityConstrainer = preset.Constrainer;
        _velocityDamper = preset.Damper;
        _velocityComputer = preset.Computer;
    }

    public void ChangeComputer(IVelocityComputer newComputer) => _velocityComputer = newComputer;

    public void ChangeGravity(IGravity newGravity) => _gravity = newGravity;

    public void ChangePhysicsMove(IPhysicsMove newPhysicsMove) => _mover = newPhysicsMove;

    public void ChangeVelocityDamper(IVelocityDamper newVelocityDamper) => _velocityDamper = newVelocityDamper;

    public void ChangeVelocityConstrainer(IVelocityConstrainer newVelocityConstrainer) => _velocityConstrainer = newVelocityConstrainer;

}
