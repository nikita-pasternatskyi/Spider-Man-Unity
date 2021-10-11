using System;
using UnityEngine;

[Serializable]
public class PlayerPhysics
{
    [SerializeField] private NormalPhysics _normalPhysics;
    private PhysicsSystemPreset _normalPreset;

    [SerializeField] private PendulumPhysics _pendulumPhysics;
    private PhysicsSystemPreset _pendulumPreset;

    private bool _attached;
    private PhysicsSystem _physicsSystem;
    private Vector3 _currentPosition;

    public event Action<Vector3> Attached;
    public event Action Detached;

    public void PhysicsInit(PlayerGround ground)
    {
        _physicsSystem = new PhysicsSystem();

        _pendulumPhysics.Initialize();
        _normalPhysics.Initialize(ground);

        _pendulumPreset = _pendulumPhysics.CreatePreset();
        _normalPreset = _normalPhysics.CreatePreset();

        _physicsSystem.ChangePreset(_normalPreset);
    }

    public Vector3 PhysicsUpdate(Vector3 currentPosition)
    {
        _currentPosition = currentPosition;
        if (_attached == true)
        {
            _pendulumPhysics.FixedUpdate(_currentPosition);
        }
        return _physicsSystem.Compute(Time.deltaTime);
    }

    public void AddForce(Vector3 force)
    {
        _physicsSystem.AddForce(force);
    }

    public void Attach(Vector3 point)
    {
        _pendulumPhysics.ChangeTether(point, _currentPosition);
        _physicsSystem.ChangePreset(_pendulumPreset);
        _attached = true;
        Attached?.Invoke(point);
    }

    public void Detach()
    {
        _physicsSystem.ChangePreset(_normalPreset);
        _attached = false;
        Detached?.Invoke();
    }

    public void Move(Vector3 input)
    {
        _normalPhysics.UpdateInput(input);
        _physicsSystem.Move(input);
    }

    public void OnGroundedChanged(bool obj)
    {
        if (obj == true)
        {
            _physicsSystem.Forces.x = 0;
            _physicsSystem.Forces.z = 0;
        }
    }
}
