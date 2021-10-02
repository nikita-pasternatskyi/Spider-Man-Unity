using UnityEngine;

public class TransformRelativeConvertor : MonoBehaviour
{
    [SerializeField] private Transform _relativeTransform;

    public Vector3 ConvertToRelative(Vector3 vector)
    {
        var forward = _relativeTransform.forward;
        var right = _relativeTransform.right;
        var result = forward.normalized * vector.z + right.normalized * vector.x;
        result.y = vector.y;
        return result;
    }

    public Vector3 ConvertToRelative(Vector3 vectorToConvert, Transform relative)
    {
        var forward = relative.forward;
        var right = relative.right;
        var result = forward.normalized * vectorToConvert.z + right.normalized * vectorToConvert.x;
        result.y = vectorToConvert.y;
        return result;
    }
}
