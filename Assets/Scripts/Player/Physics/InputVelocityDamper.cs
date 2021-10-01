using UnityEngine;

[System.Serializable]
public class InputVelocityDamper : IVelocityDamper
{
    [SerializeField] private float _force;
    private Vector3 _input;

    public InputVelocityDamper(float force) => _force = force;

    private float DampForceAxis(float axis, float damperValue, float time)
    {
        if (axis != 0)
        {
            if (DifferentSigns(axis, damperValue))
            {
                return damperValue * time * _force;
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

    public void UpdateInput(Vector3 input)
    {
        _input = input;
    }

    public void DampVelocity(ref Vector3 velocity, float time)
    {
        velocity.x += DampForceAxis(velocity.x, _input.x, time);
        velocity.z += DampForceAxis(velocity.z, _input.z, time);
    }
}
