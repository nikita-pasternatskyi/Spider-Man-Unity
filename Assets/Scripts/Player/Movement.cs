using System;
using UnityEngine;

[Serializable]
public class Movement : IPlayerPhysicsState
{
    [SerializeField] private Vector3 _gravity;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _slopeFallSpeed;
    [SerializeField] private float _minimumYValue;
    [SerializeField] private float _dampingForce;
    [SerializeField] private PlayerGround _playerGround;
    public event Action<Vector3> VelocityCalculated;
    private bool _wasGrounded;

    private Vector3 _inputMovement;
    private Vector3 _forces;
    private Vector3 _movement;

    public void Start()
    {
    }
    private void CheckGround()
    {
        if (_playerGround.Grounded == true && _wasGrounded == false)
        {
            _forces.x = 0;
            _forces.z = 0;
        }
        _wasGrounded = _playerGround.Grounded;
    }
    private void CalculateGravity(float time)
    {
        _forces += _gravity * time * _gravityMultiplier;
        if (_playerGround.Grounded == true)
        {
            _forces.y = _forces.y < _minimumYValue == true ? _minimumYValue : _forces.y;
        }

        if (_playerGround.CurrentNormal.y != 1)
        {
            _forces.y -= _slopeFallSpeed;
        }

        if (_forces.z != 0)
        {
            _forces.z += _inputMovement.z * _dampingForce * time;
        }

        if (_forces.x != 0)
        {
            _forces.x += _inputMovement.x * _dampingForce * time;
        }
    }
    public void Jump(Vector3 force)
    {
        if (_playerGround.Grounded == true)
        {
            AddInstantForce(force);
        }
    }
    public void FixedUpdate(Vector3 currentPosition, float time)
    {
        _playerGround.FixedUpdate(currentPosition);

        CalculateGravity(time);
        _movement = _forces + _inputMovement;
        VelocityCalculated?.Invoke(_movement);
        CheckGround();
    }

    public void Move(Vector3 input)
    {
        if (input == Vector3.zero)
        {
            _inputMovement = Vector3.zero;
            return;
        }
        _inputMovement += input;
        var difference = input - _inputMovement;
        if (_inputMovement != input)
        {
            _inputMovement += difference;
        }
        _inputMovement = _playerGround.Project(_inputMovement);
    }

    private void AddInstantForce(Vector3 force) => _forces += force;
}
