using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerPhysicsStateMachine))]
public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private Vector3 _normalJump;
    [SerializeField] private Vector3 _chargedJump;
    private PlayerPhysicsStateMachine _playerPhysicsStateMachine;
    private PlayerInput _playerInput;
    private Vector3 _movementInput;
    private bool _modifierPressed;
    private bool _swingPressed;
    private float _currentSpeed;


    private void Awake()
    {
        _currentSpeed = _walkSpeed;
        _playerPhysicsStateMachine = GetComponent<PlayerPhysicsStateMachine>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        _playerPhysicsStateMachine.Move(_movementInput * _currentSpeed);
    }

    private void OnEnable()
    {
        _playerInput.ModifierPressed += OnModifierPressed;
        _playerInput.SwingPressed += OnSwingPressed;
        _playerInput.MovementPressed += OnMovementPressed;
        _playerInput.JumpPressed += OnJumpPressed;
    }

    private void OnSwingPressed()
    {
        _swingPressed = _swingPressed == false ? true : false;
        if (_swingPressed == true)
        {
            _playerPhysicsStateMachine.Attach(transform.forward * 50 + transform.up * 50 + transform.position);
            return;
        }
        _playerPhysicsStateMachine.Detach();
    }

    private void OnDisable()
    {
        _playerInput.MovementPressed -= OnMovementPressed;
        _playerInput.JumpPressed -= OnJumpPressed;
    }

    private void OnModifierPressed(bool obj)
    {
        _modifierPressed = obj;
        if (_modifierPressed == true)
        {
            _currentSpeed = _runSpeed;
            return;
        }
        _currentSpeed = _walkSpeed;
    }
    private void OnMovementPressed(Vector2 obj)
    {
        _movementInput.x = obj.x;
        _movementInput.z = obj.y;
    }

    private void OnJumpPressed()
    {
        _playerPhysicsStateMachine.Jump(_normalJump);
    }
}
