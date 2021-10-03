using System;
using UnityEngine;

[RequireComponent(typeof(GrapplePointer))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(TransformRelativeConvertor))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private LineRendererPointsView _line;
    [Header("StateMachine")]
    [SerializeField] private PlayerStateMachine _playerStateMachine;

    [Header("Physics")]
    [SerializeField] private PlayerGround _ground;
    [SerializeField] private PlayerPhysics _playerPhysics;

    private GrapplePointer _grapplePointer;
    private TransformRelativeConvertor _relativeConvertor;
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _grapplePointer = GetComponent<GrapplePointer>();
        _relativeConvertor = GetComponent<TransformRelativeConvertor>();
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();

        _playerPhysics.PhysicsInit(_ground);
        _playerStateMachine.Initialize(_grapplePointer, _playerPhysics, _relativeConvertor);

        _playerInput.AddListener(_playerStateMachine);
    }

    private void OnEnable()
    {
        _playerPhysics.Attached += _line.AddPoint;
        _playerPhysics.Detached += _line.Reset;
        _ground.GroundedChanged += _playerPhysics.OnGroundedChanged;
    }

    private void FixedUpdate() => _rigidbody.velocity = _playerPhysics.PhysicsUpdate(transform.position);

    private void OnDisable()
    {
        _playerPhysics.Detached -= _line.Reset;
        _playerPhysics.Attached -= _line.AddPoint;
        _ground.GroundedChanged -= _playerPhysics.OnGroundedChanged;
    }
}
