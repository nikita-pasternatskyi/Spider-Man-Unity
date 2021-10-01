using UnityEngine;

[CreateAssetMenu(fileName = "PhysicsConfig", menuName = "Create/PhysicsConfig")]
public class PhysicsMovementConfig : ScriptableObject
{
    public Vector3 Gravity;
    public float GravityMultiplier = 1;
    public float SlopeFallSpeed = 15;
    public float MinimumYValueOnGround = 0;
    public float MinimumYValueFall = -100;
    public float DampingForce = 4;
}
