using UnityEngine;

[System.Serializable]
public class PendulumVelocityConstraints : IVelocityConstrainer
{
    [SerializeField] private float _spring;
    private TetherPoint _currentTetherPoint;
    private Vector3 _currentBobPosition;
    private Vector3 _direction;

    public Vector3 GetDirection() => _direction;

    public void UpdatePosition(Vector3 position)
    {
        _currentBobPosition = position;
    }

    public void ChangeTether(Vector3 point)
    {
        _currentTetherPoint = new TetherPoint(point, Vector3.zero, Vector3.Distance(_currentBobPosition, point));
    }

    public void ConstrainVelocity(ref Vector3 velocity, float time)
    {
        _direction = _currentTetherPoint.Position - _currentBobPosition;
        velocity = Vector3.ProjectOnPlane(velocity, _direction);
        _currentTetherPoint.Position = _currentTetherPoint.StartPosition;
        float distance = Vector3.Distance(_currentBobPosition, _currentTetherPoint.Position);
        float distanceError = Mathf.Abs(distance - _currentTetherPoint.Length);

        Vector3 changeDirection = Vector3.zero;
        if (distance > _currentTetherPoint.Length)
        {
            changeDirection = _currentTetherPoint.Position - _currentBobPosition;
        }
        else if (distance < _currentTetherPoint.Length)
        {
            changeDirection = _currentBobPosition - _currentTetherPoint.Position;
        }

        if (changeDirection != Vector3.zero)
        {
            velocity += changeDirection.normalized * distanceError;
            _currentTetherPoint.Position -= changeDirection.normalized * distanceError * 1 / _spring;
        }
    }
}
