using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStateMachine : DependencyInjector, IInputListener
{
    [Header("States")]
    [SerializeField] private NormalState _normalState;
    [SerializeField] private SwingState _swingState;

    [DependencyResolver] private PlayerStateMachine _playerStateMachine;
    [DependencyResolver] private TransformRelativeConvertor _convertor;
    [DependencyResolver] private PlayerPhysics _playerPhysics;
    [DependencyResolver] private GrapplePointFinder _grapplePointer;

    public Vector3 CurrentInput { get; private set; }
    private PlayerState _currentState;
    private Dictionary<PlayerStates, PlayerState> _states;

    public enum PlayerStates
    {
        NormalState, SwingState
    }

    public void Initialize(GrapplePointFinder grapplePointer, PlayerPhysics playerPhysics, TransformRelativeConvertor convertor)
    {
        _playerStateMachine = this;
        if (grapplePointer == null)
            throw new MissingReferenceException(nameof(grapplePointer));
        if (convertor == null)
            throw new MissingReferenceException(nameof(convertor));
        if (playerPhysics == null)
            throw new MissingReferenceException(nameof(playerPhysics));

        _grapplePointer = grapplePointer;
        _convertor = convertor;
        _playerPhysics = playerPhysics;
        _states = new Dictionary<PlayerStates, PlayerState>();
        _states.Add(PlayerStates.NormalState, _normalState);
        _states.Add(PlayerStates.SwingState, _swingState);
        InjectDependencies();
        ChangeState(PlayerStates.NormalState);
    }

    public void OnJumpPressed()
    {
        _currentState.OnJumpPressed();
    }

    public void OnModifierPressed(bool obj) => _currentState.OnModifierPressed(obj);

    public void OnMouseMoved(Vector2 input) => _currentState.OnMouseMoved(input);

    public void OnMoved(Vector3 input)
    {
        CurrentInput = input;
        _currentState.OnMoved(input);
    }

    public void OnSwingPressed(bool obj)
    {
        _currentState.OnSwingPressed(obj);
    }

    public void ChangeState(PlayerStates playerState)
    {
        _states.TryGetValue(playerState, out PlayerState newState);
        if (newState == _currentState)
            return;
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}


