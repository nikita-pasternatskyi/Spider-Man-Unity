using UnityEngine;

[RequireComponent(typeof(GrapplePointFinder))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(TransformRelativeConvertor))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("StateMachine")]
    [SerializeField] private PlayerStateMachine _playerStateMachine;
    [SerializeField] private bool _lockMouseCursor;

    [Header("Physics")]
    [SerializeField] private LineRendererPointsView _webLine;
    [SerializeField] private PlayerGround _ground;
    [SerializeField] private PlayerPhysics _playerPhysics;

    private GrapplePointFinder _grapplePointer;
    private TransformRelativeConvertor _relativeConvertor;
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        if (_lockMouseCursor == true)
            Cursor.lockState = CursorLockMode.Locked;

        _grapplePointer = GetComponent<GrapplePointFinder>();
        _relativeConvertor = GetComponent<TransformRelativeConvertor>();
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();

        _playerPhysics.PhysicsInit(_ground);
        _playerStateMachine.Initialize(_grapplePointer, _playerPhysics, _relativeConvertor);

        _playerInput.AddListener(_playerStateMachine);
    }

    private void OnEnable()
    {
        _playerPhysics.Attached += _webLine.AddPoint;
        _playerPhysics.Detached += _webLine.Reset;
        _ground.GroundedChanged += _playerPhysics.OnGroundedChanged;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _playerPhysics.PhysicsUpdate(transform.position);
    }

    private void OnDisable()
    {
        _playerPhysics.Attached -= _webLine.AddPoint;
        _playerPhysics.Detached -= _webLine.Reset;
        _ground.GroundedChanged -= _playerPhysics.OnGroundedChanged;
    }
}
