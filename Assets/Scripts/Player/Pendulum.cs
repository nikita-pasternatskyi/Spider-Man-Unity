using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pendulum : IPlayerPhysicsState
{
    [SerializeField] private Vector3 _gravity;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private uint _accuracy = 50;
    [SerializeField] private float _spring;
    [SerializeField] private float _dampingForce;
    private List<TetherPoint> _tetherPoints;
    private Vector3 _velocity;
    private Vector3 _direction;

    public Vector3 Velocity => _velocity;

    public event Action<Vector3> Attached;
    public event Action<Vector3> VelocityCalculated;

    private TetherPoint _currentTetherPoint => _tetherPoints[0];

    private class TetherPoint
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

    public void Start()
    {
        _tetherPoints = new List<TetherPoint>();
    }

    public void Attach(Vector3 currentPosition, RaycastHit hit)
    {
        var length = Vector3.Distance(currentPosition, hit.point);
        _tetherPoints.Add(new TetherPoint(hit.point, hit.normal, length));
        Attached?.Invoke(hit.point);
    }

    public void Attach(Vector3 currentPosition, Vector3 tetherPosition)
    {
        var length = Vector3.Distance(currentPosition, tetherPosition);
        _tetherPoints.Add(new TetherPoint(tetherPosition, Vector3.zero, length));
        Attached?.Invoke(tetherPosition);
    }

    public void Detach()
    {
        _tetherPoints.Clear();
        _direction = Vector3.zero;
    }

    public void Move(Vector3 input)
    {
        _velocity += Vector3.ProjectOnPlane(input, _direction);
        VelocityCalculated?.Invoke(_velocity);
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
        VelocityCalculated?.Invoke(_velocity);
    }

    public void Jump(Vector3 force) => throw new NotImplementedException();

    public void FixedUpdate(Vector3 currentPosition, float time)
    {
        _direction = _currentTetherPoint.Position - currentPosition;
        ResetTetherPosition();
        Movement(currentPosition, time);
        for (int i = 0; i < _accuracy; i++)
        {
            CorrectVelocity(currentPosition);
        }
        VelocityCalculated?.Invoke(_velocity);
    }

    private void ApplyGravity(float time) => _velocity += _gravity * time;

    private void DampVelocity() => _velocity -= _velocity * _dampingForce;

    private void Movement(Vector3 currentPosition, float time)
    {
        ApplyGravity(time);
        _velocity = Vector3.ProjectOnPlane(_velocity, _direction);
        DampVelocity();
    }

    private void ResetTetherPosition()
    {
        _tetherPoints[0].Position = _tetherPoints[0].StartPosition;
    }

    private void CorrectVelocity(Vector3 currentPosition)
    {
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
            _tetherPoints[0].Position -= changeDirection.normalized * distanceError * 1/_spring;
        }
    }

}
