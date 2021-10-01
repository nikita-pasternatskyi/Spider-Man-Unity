using UnityEngine;

public class PhysicsMovement : IPlayerPhysicsState
{
    private PhysicsMovementConfig _physicsConfig;
    private PlayerGround _playerGround;
    private Vector3 _force;
    private Vector3 _inputForce;

    public PhysicsMovement(PhysicsMovementConfig physicsConfig, PlayerGround playerGround)
    {
        _physicsConfig = physicsConfig;
        _playerGround = playerGround;
    }

    public Vector3 FixedUpdate(Vector3 currentPosition, float time)
    {
        CalculateGravity(time);
        return _force + _inputForce;
    }


    public void Jump(Vector3 force)
    {
        if (_playerGround.Grounded == true)
        {
            AddForce(force);
        }
    }

    public void OnGroundedChanged(bool obj)
    {
        if (obj == true)
        {
            _force.x = 0;
            _force.z = 0;
        }
    }
    public void Move(Vector3 input)
    {
        if (input == Vector3.zero)
        {
            _inputForce = Vector3.zero;
            return;
        }
        _inputForce += input;
        var difference = input - _inputForce;
        if (_inputForce != input)
        {
            _inputForce += difference;
        }
        _inputForce = _playerGround.Project(_inputForce);
    }
    private void CalculateGravity(float time)
    {
        _force += _physicsConfig.Gravity * time * _physicsConfig.GravityMultiplier;

        if (_playerGround.Grounded == true)
        {
            _force.y = _force.y < _physicsConfig.MinimumYValueOnGround == true ? _physicsConfig.MinimumYValueOnGround : _force.y;
        }

        _force.y = _force.y < _physicsConfig.MinimumYValueFall == true ? _physicsConfig.MinimumYValueFall : _force.y;

        if (_playerGround.CurrentNormal.y != 1)
        {
            _force.y -= _physicsConfig.SlopeFallSpeed;
        }

        _force.z += DampForceAxis(_force.z, _inputForce.z, time);
        _force.x += DampForceAxis(_force.x, _inputForce.x, time);
    }
    public void AddForce(Vector3 force)
    {
        _force += force;
    }

    private float DampForceAxis(float axis, float damperValue, float time)
    {
        if (axis != 0)
        {
            if (DifferentSigns(axis, damperValue))
            {
                return damperValue * _physicsConfig.DampingForce * time;
            }
        }
        return 0;
    }
    private bool DifferentSigns(float a, float b)
    {
        if (a < 0 && b > 0)
            return true;

        if (a > 0 && b < 0)
            return true;

        return false;
    }

    public void Enter() => throw new System.NotImplementedException();
    public void Exit() => throw new System.NotImplementedException();
}


//public void OnGroundedChanged(bool obj)
//{
//    if (obj == true)
//    {
//        _force.x = 0;
//        _force.z = 0;
//    }
//}

//public void Move(Vector3 input)
//{
//    if (input == Vector3.zero)
//    {
//        _inputForce = Vector3.zero;
//        return;
//    }
//    _inputForce += input;
//    var difference = input - _inputForce;
//    if (_inputForce != input)
//    {
//        _inputForce += difference;
//    }
//    _inputForce = _playerGround.Project(_inputForce);
//}

//private void CalculateGravity(float time)
//{
//    _force += _physicsConfig.Gravity * time * _physicsConfig.GravityMultiplier;

//    if (_playerGround.Grounded == true)
//    {
//        _force.y = _force.y < _physicsConfig.MinimumYValueOnGround == true ? _physicsConfig.MinimumYValueOnGround : _force.y;
//    }

//    _force.y = _force.y < _physicsConfig.MinimumYValueFall == true ? _physicsConfig.MinimumYValueFall : _force.y;

//    if (_playerGround.CurrentNormal.y != 1)
//    {
//        _force.y -= _physicsConfig.SlopeFallSpeed;
//    }

//    _force.z += DampForceAxis(_force.z, _inputForce.z, time);
//    _force.x += DampForceAxis(_force.x, _inputForce.x, time);
//}
