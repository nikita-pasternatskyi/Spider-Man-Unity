using UnityEngine;

public interface IVelocityConstrainer
{
    public void ConstrainVelocity(ref Vector3 velocity, float time);
}
