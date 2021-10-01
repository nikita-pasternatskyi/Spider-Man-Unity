using UnityEngine;

[System.Serializable]
public class PendulumVelocityDamper : IVelocityDamper
{
    [SerializeField] private float _force;

    public void DampVelocity(ref Vector3 velocity, float time)
    {
        velocity -= velocity * _force;
    }
}
