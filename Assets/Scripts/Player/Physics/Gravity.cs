using UnityEngine;

[System.Serializable]
public class Gravity : IGravity
{
    [SerializeField] private Vector3 _force;
    [SerializeField] private float _multiplier;

    public Vector3 CalculateGravity(float time) => _force * _multiplier * time;
}
