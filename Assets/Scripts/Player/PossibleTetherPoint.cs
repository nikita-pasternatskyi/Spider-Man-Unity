using UnityEngine;

public class PossibleTetherPoint
{
    public Vector3 Position;
    public float Score;

    public PossibleTetherPoint(float score) => Score = score;

    public PossibleTetherPoint() { }
}
