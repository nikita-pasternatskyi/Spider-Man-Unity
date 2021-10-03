﻿using UnityEngine;

[System.Serializable]
public class NormalState : PlayerState
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _jump;
    [SerializeField] private float _runSpeed;
    [SerializeField] private Vector3 _runJump;
    private Vector3 _currentInput;
    private bool _modified;

    [Inject] private TransformRelativeConvertor _relativeConvertor;
    [Inject] private PlayerPhysics _physicsSystem;

    public override void OnMoved(Vector3 input)
    {
        _currentInput = input;
        MovePlayer();
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
            _physicsSystem.Move(_relativeConvertor.ConvertToRelative(_currentInput * _runSpeed));
            return;
        }
        _physicsSystem.Move(_relativeConvertor.ConvertToRelative(_currentInput * _speed));
    }

    private void Jump()
    {
        if (_modified == true)
        {
            _physicsSystem.AddForce(_relativeConvertor.ConvertToRelative(_runJump));
            return;
        }
        _physicsSystem.AddForce(_relativeConvertor.ConvertToRelative(_jump));
    }
}


