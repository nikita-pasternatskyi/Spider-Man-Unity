using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("StateMachine")]
    [SerializeField] private PlayerStateMachine _playerStateMachine;

    [Header("Physics")]
    [SerializeField] private PlayerGround _ground;
    [SerializeField] private NormalPhysics _normalPhysics;
    private PhysicsSystemPreset _normalPreset;

    [SerializeField] private PendulumPhysics _pendulumPhysics;
    private PhysicsSystemPreset _pendulumPreset;

    private bool _swinning;
    private Rigidbody _rigidbody;
    private PlayerInput _playerInput;
    private GrapplePointer _grapplePointer;
    private PhysicsSystem _physicsSystem;

    public event Action Attached;
    public event Action Detached;

    private void Awake()
    {
        PhysicsInit();
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _playerStateMachine.Initialize(_playerInput, _physicsSystem);
    }

    private void OnEnable() => _ground.GroundedChanged += OnGroundedChanged;

    private void FixedUpdate() => PhysicsUpdate(transform.position);

    private void OnDisable() => _ground.GroundedChanged -= OnGroundedChanged;

    private void PhysicsInit()
    {
        _physicsSystem = new PhysicsSystem();
        _pendulumPhysics.Initialize();
        _normalPhysics.Initialize(_ground);
        _pendulumPreset = _pendulumPhysics.CreatePreset();
        _normalPreset = _normalPhysics.CreatePreset();

        _physicsSystem.ChangePreset(_normalPreset);
    }

    private void HandlePresetChanging()
    {
        _swinning = _swinning == true ? false : true;
        if (_swinning == true)
        {
            _physicsSystem.ChangePreset(_pendulumPreset);
            Attached?.Invoke();
            return;
        }
        _physicsSystem.ChangePreset(_normalPreset);
        Detached?.Invoke();
        return;
    }

    private void Swing(Vector3 input)
    {
        if (_grapplePointer.CheckGrapplePoint(input, out Vector3 point) == true && _swinning == false)
        {
            _pendulumPhysics.ChangeTether(point, transform.position);
            HandlePresetChanging();
            return;
        }
        HandlePresetChanging();
    }

    private void OnGroundedChanged(bool obj)
    {
        if (obj == true)
        {
            _physicsSystem.Forces.x = 0;
            _physicsSystem.Forces.z = 0;
        }
    }

    private void PhysicsUpdate(Vector3 currentPosition)
    {
        if (_swinning == true)
        {
            _pendulumPhysics.FixedUpdate(currentPosition);
        }
        _rigidbody.velocity = _physicsSystem.Compute(Time.deltaTime);
    }

}


