using UnityEngine;

[System.Serializable]
public class CustomPlayerVelocityConstrainer : IVelocityConstrainer
{
    [SerializeField] private Vector3 _maximum;
    [SerializeField] private Vector3 _minimum;
    [SerializeField] private float _minimumYValueOnGround;
    [SerializeField] private float _slopeFallSpeed;
    private PlayerGround _playerGround;

    public void Init(PlayerGround playerGround)
    {
        _playerGround = playerGround;
    }

    public void ConstrainVelocity(ref Vector3 velocity, float time)
    {
        if (_playerGround.Grounded == true)
        {
            velocity.y = velocity.y < _minimumYValueOnGround == true ? _minimumYValueOnGround : velocity.y;
        }

        if (_playerGround.CurrentNormal.y != 1)
        {
            velocity.y -= _slopeFallSpeed;
        }

        velocity.x = ConstrainValue(velocity.x, _maximum.x, _minimum.x);
        velocity.y = ConstrainValue(velocity.y, _maximum.y, _minimum.y);
        velocity.z = ConstrainValue(velocity.z, _maximum.z, _minimum.z);
    }

    private float ConstrainValue(float value, float maximum, float minimum)
    {
        if (value > maximum)
            value = maximum;
        if (value < minimum)
            value = minimum;
        if (value == minimum || value == maximum)
            return value;
        return value;
    }
}
