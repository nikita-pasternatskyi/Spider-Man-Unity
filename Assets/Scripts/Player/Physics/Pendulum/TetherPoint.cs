using UnityEngine;

public class TetherPoint
{
    public readonly Vector3 StartPosition;

    public Vector3 Position;
    public float Length;

    public TetherPoint(Vector3 startPosition, float length)
    {
        StartPosition = startPosition;
        Position = startPosition;
        Length = length;
    }
}
