using UnityEngine;

[System.Serializable]
public class PendulumGravity : IGravity
{
    [SerializeField] private Vector3 _force;
    [SerializeField] private float _multiplier;
    private Vector3 _direction;
    
    public void ChangeDirection(Vector3 direction) => _direction = direction;
    public Vector3 CalculateGravity(float time) => _force * _multiplier * time;
}
