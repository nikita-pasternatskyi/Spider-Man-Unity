using UnityEngine;

public interface IVelocityDamper
{
    public void DampVelocity(ref Vector3 velocity, float time);
}
