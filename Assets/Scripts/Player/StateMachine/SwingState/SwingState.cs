using UnityEngine;

[System.Serializable]
public class SwingState : PlayerState
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _boost;
    [SerializeField] private Vector3 _jump;

    [Inject] private PlayerStateMachine _playerStateMachine;
    [Inject] private TransformRelativeConvertor _relativeConvertor;
    [Inject] private PlayerPhysics _physics;

    private Vector3 _currentInput;
    private bool _modified;

    public override void Enter()
    {
        _currentInput = _playerStateMachine.CurrentInput;
        MovePlayer();
    }

    public override void Exit()
    {
        _physics.Detach();
    }

    public override void OnMoved(Vector3 input)
    {
        _currentInput = input;
        MovePlayer();
    }

    public override void OnModifierPressed(bool obj)
    {
        _modified = obj;
        if (_modified == true)
            _physics.AddForce(_relativeConvertor.ConvertToRelative(_boost));
    }

    public override void OnSwingPressed(bool obj)
    {
        if(obj == false)
            _playerStateMachine.ChangeState(PlayerStateMachine.PlayerStates.NormalState);
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
        _physics.Move(_relativeConvertor.ConvertToRelative(_currentInput * _speed));
    }

    private void Jump()
    {
        _physics.AddForce(_relativeConvertor.ConvertToRelative(_jump));
        _playerStateMachine.ChangeState(PlayerStateMachine.PlayerStates.NormalState);
    }
}
