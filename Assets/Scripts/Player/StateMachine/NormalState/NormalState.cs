using UnityEngine;

[System.Serializable]
public class NormalState : PlayerState
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _jump;
    [SerializeField] private float _runSpeed;
    [SerializeField] private Vector3 _runJump;
    private Vector3 _currentInput;
    private bool _modified;

    [Inject] private PlayerStateMachine _playerStateMachine;
    [Inject] private GrapplePointFinder _grapplePointer;
    [Inject] private TransformRelativeConvertor _relativeConvertor;
    [Inject] private PlayerPhysics _physics;

    public override void OnMoved(Vector3 input)
    {
        _currentInput = input;
        MovePlayer();
    }

    public override void OnSwingPressed(bool obj)
    {
        if (obj == true)
        {
            Swing();
        }
    }

    public override void OnModifierPressed(bool obj)
    {
        _modified = obj;
        MovePlayer();
    }

    public override void OnJumpPressed()
    {
        Jump();
    }

    public override void OnMouseMoved(Vector2 input)
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (_modified == true)
        {
            _physics.Move(_relativeConvertor.ConvertToRelative(_currentInput * _runSpeed));
            return;
        }
        _physics.Move(_relativeConvertor.ConvertToRelative(_currentInput * _speed));
    }

    private void Swing()
    {
        if (_grapplePointer.FindGrapplePoint(_relativeConvertor.ConvertToRelative(_currentInput), _relativeConvertor.transform.forward, _relativeConvertor.transform.right, out Vector3 point) == true)
        {
            _physics.Attach(point);
            _playerStateMachine.ChangeState(PlayerStateMachine.PlayerStates.SwingState);
        }
    }

    private void Jump()
    {
        if (_modified == true)
        {
            _physics.AddForce(_relativeConvertor.ConvertToRelative(_runJump));
            return;
        }
        _physics.AddForce(_relativeConvertor.ConvertToRelative(_jump));
    }
}


