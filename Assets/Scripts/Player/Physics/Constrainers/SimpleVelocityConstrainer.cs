using UnityEngine;

[System.Serializable]
public class SimpleVelocityConstrainer : IVelocityConstrainer
{
    [SerializeField] private Vector3 _maximum;
    [SerializeField] private Vector3 _minimum;
    public void ConstrainVelocity(ref Vector3 velocity, float time)
    {
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
