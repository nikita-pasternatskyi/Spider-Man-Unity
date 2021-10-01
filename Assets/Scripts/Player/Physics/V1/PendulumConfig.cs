using UnityEngine;

[CreateAssetMenu(fileName = "PendulumPhysicsConfig", menuName = "Create/PendulumPhysicsConfig")]
public class PendulumConfig : ScriptableObject
{
    public Vector3 Gravity;
    public float GravityMultiplier = 1;
    public uint Accuracy = 10;
    public float Spring = 2;
    public float DampingForce = 4;
}
