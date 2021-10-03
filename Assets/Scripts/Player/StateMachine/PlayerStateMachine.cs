using UnityEngine;

[System.Serializable]
public class PlayerStateMachine : DependencyInjector, IInputListener
{
    [Header("States")]
    [SerializeField] private NormalState _normalState;
    [SerializeField] private SwingState _swingState;

    [DependencyResolver] private TransformRelativeConvertor _convertor;
    [DependencyResolver] private PlayerPhysics _playerPhysics;

    private GrapplePointer _grapplePointer;
    private PlayerState _currentState;

    private Vector3 _currentInput;

    public void Initialize(GrapplePointer grapplePointer, PlayerPhysics playerPhysics, TransformRelativeConvertor convertor)
    {
        if (grapplePointer == null)
            throw new MissingReferenceException(nameof(grapplePointer));
        if (convertor == null)
            throw new MissingReferenceException(nameof(convertor));
        if (playerPhysics == null)
            throw new MissingReferenceException(nameof(playerPhysics));

        _grapplePointer = grapplePointer;
        _convertor = convertor;
        _playerPhysics = playerPhysics;

        InjectDependencies();
        ChangeState(_normalState);
    }

    public void OnJumpPressed()
    {
        _currentState.OnJumpPressed();
        ChangeState(_normalState);
        _playerPhysics.Detach();
    }

    public void OnModifierPressed(bool obj) => _currentState.OnModifierPressed(obj);

    public void OnMouseMoved(Vector2 input) => _currentState.OnMouseMoved(input);

    public void OnMoved(Vector3 input)
    {
        _currentInput = input;
        _currentState.OnMoved(input);
    }

    public void OnSwingPressed(bool obj)
    {
        _currentState.OnSwingPressed(obj);
        if (obj == true)
        {
            if (_grapplePointer.CheckGrapplePoint(_currentInput, out Vector3 point) == true)
            {
                ChangeState(_swingState);
                _playerPhysics.Attach(point);
            }
            return;
        }

        //_playerPhysics.Detach();
        //ChangeState(_normalState);

    }

    private void ChangeState(PlayerState newState)
    {
        if (newState == _currentState)
            return;
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
