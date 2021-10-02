using UnityEngine;

[System.Serializable]
public class PlayerStateMachine : DependencyInjector, IInputListener
{
    [Header("States")]
    [SerializeField] private NormalState _normalState;

    private PlayerState _currentState;
    [DependencyResolver] private PhysicsSystem _physicsSystem;

    public void Initialize(PlayerInput playerInput, PhysicsSystem physicsSystem)
    {
        if (playerInput == null)
            throw new MissingReferenceException(nameof(playerInput));
        if (physicsSystem == null)
            throw new MissingReferenceException(nameof(physicsSystem));
        _physicsSystem = physicsSystem;
        playerInput.AddListener(this);
        InjectDependencies();
    }

    public void OnJumpPressed() => _currentState.OnJumpPressed();
    public void OnModifierPressed(bool obj) => _currentState.OnModifierPressed(obj);
    public void OnMouseMoved(Vector2 input) => _currentState.OnMouseMoved(input);
    public void OnMoved(Vector3 moved) => _currentState.OnMoved(moved);
    public void OnSwingPressed(bool obj) => _currentState.OnSwingPressed(obj);
}


