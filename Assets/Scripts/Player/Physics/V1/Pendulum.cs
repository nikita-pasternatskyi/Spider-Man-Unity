using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pendulum
{
    public Vector3 UnTouched;
    [SerializeField] private PendulumConfig _pendulumPhysicsConfig;
    private Vector3 _inputForce;
    private List<TetherPoint> _tetherPoints;
    private Vector3 _direction;
    private Vector3 _velocity;

    public event Action Detached;

    private TetherPoint _currentTetherPoint => _tetherPoints[0];

    public void Init()
    {
        _tetherPoints = new List<TetherPoint>();
    }

    public class TetherPoint
    {
        public Vector3 Position;
        public readonly Vector3 StartPosition;
        public readonly Vector3 Normal;
        public readonly float Length;

        public TetherPoint(Vector3 startPosition, Vector3 normal, float length)
        {
            StartPosition = startPosition;
            Normal = normal;
            Length = length;
            Position = StartPosition;
        }
    }

    public void Attach(Vector3 currentPosition, Vector3 tetherPosition)
    {
        var length = Vector3.Distance(currentPosition, tetherPosition);
        _tetherPoints.Add(new TetherPoint(tetherPosition, Vector3.zero, length));
    }
    public void Detach()
    {
        _tetherPoints.Clear();
        _direction = Vector3.zero;
        Detached?.Invoke();
    }
    public void Move(Vector3 input)
    {
        _inputForce = input;
    }
    public void AddForce(Vector3 force)
    {
        _velocity += force;
    }
    public void Jump(Vector3 force)
    {
        AddForce(force);
        Detach();
    }

    public Vector3 FixedUpdate(Vector3 currentPosition, float time)
    {
        _velocity += _pendulumPhysicsConfig.Gravity * time;
        _velocity -= _velocity * _pendulumPhysicsConfig.DampingForce;
        CorrectVelocity(currentPosition, time);
        return _velocity;
    }

    private void ApplyGravity(float time) => _velocity += _pendulumPhysicsConfig.Gravity * time;
    private void DampVelocity()
    {
        _velocity -= _velocity * _pendulumPhysicsConfig.DampingForce;
    }
    private void Movement(Vector3 currentPosition, float time)
    {
        
    }

    private void ResetTetherPosition()
    {
        _tetherPoints[0].Position = _tetherPoints[0].StartPosition;
    }
    private void CorrectVelocity(Vector3 currentPosition, float time)
    {
        _velocity = Vector3.ProjectOnPlane(_velocity, _direction);
        _direction = _currentTetherPoint.Position - currentPosition;
        ResetTetherPosition();

        float distance = Vector3.Distance(currentPosition, _currentTetherPoint.Position);
        float distanceError = Mathf.Abs(distance - _currentTetherPoint.Length);
        Vector3 changeDirection = Vector3.zero;
        if (distance > _currentTetherPoint.Length)
        {
            changeDirection = _currentTetherPoint.Position - currentPosition;
        }
        else if (distance < _currentTetherPoint.Length)
        {
            changeDirection = currentPosition - _currentTetherPoint.Position;
        }

        if (changeDirection != Vector3.zero)
        {
            _velocity += changeDirection.normalized * distanceError;
            _tetherPoints[0].Position -= changeDirection.normalized * distanceError * 1/ _pendulumPhysicsConfig.Spring;
        }
    }
}
