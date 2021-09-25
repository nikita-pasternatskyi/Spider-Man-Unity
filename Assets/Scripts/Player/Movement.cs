using System;
using UnityEngine;

[Serializable]
public class Movement : IPlayerPhysicsState
{
    [SerializeField] private Vector3 _gravity;
    [SerializeField] private float _slopeFallSpeed;
    [SerializeField] private float _minimumYValue;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _dampingForce;
    [SerializeField] private PlayerGround _playerGround;
    public event Action<Vector3> VelocityCalculated;
    private bool _wasGrounded;

    private Vector3 _velocityToAdd;
    private Vector3 _velocity;
    private Vector3 _gravityVelocity;

    public Vector3 Velocity => _velocity + _gravityVelocity;

    public void FixedUpdate(Vector3 currentPosition, float time)
    {
        _playerGround.FixedUpdate(currentPosition);
        DampVelocity(time);
        CalculateGravity(time);
        _velocity += _gravityVelocity;
        VelocityCalculated?.Invoke(_velocity);

        _wasGrounded = _playerGround.Grounded;
        _velocityToAdd = Vector3.zero;
    }

    private void CalculateGravity(float time)
    {
        _gravityVelocity += _gravity * time * _gravityMultiplier;
        if (_playerGround.Grounded == true)
        {
            _velocity.y = _velocity.y < _minimumYValue == true ? _minimumYValue : _velocity.y;
            _gravityVelocity.y = _gravityVelocity.y < _minimumYValue == true ? _minimumYValue : _gravityVelocity.y;
        }
    }

    public void Move(Vector3 input)
    {
        var projectedInput = _playerGround.Project(input);
        if (_playerGround.CurrentNormal.y == 1)
        {
            _velocity += projectedInput;
            return;
        }
        if (projectedInput.z <= 0)
            projectedInput.y -= _slopeFallSpeed;
        _velocity += projectedInput;
    }

    private void DampVelocity(float time)
    {
        var velocity = _velocity;
        velocity.y = 0;
        _velocity -= velocity * _dampingForce * time;
    }

    public void Jump(Vector3 force)
    {
        if (_playerGround.Grounded == true)
        {
            _velocity += force;
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
        _gravityVelocity.y = velocity.y;
    }

    public void Start()
    { }
}
