using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(MeshCollider))]
public class Grappable : MonoBehaviour
{
    public (Vector3 point, float distance) GetClosestPoint(Vector3 point)
    {
        var meshCollider = GetComponent<MeshCollider>();
        return (meshCollider.ClosestPoint(point), (meshCollider.ClosestPoint(point) - point).sqrMagnitude);
    }
    public (Vector3 point, float distance) GetClosestCorner(Vector3 point, bool cancelLowPoints = false, float minY = 0)
    {
        var meshCollider = GetComponent<MeshCollider>();
        (Vector3 position, float distance) targetPosition;
        targetPosition.position = Vector3.zero;
        targetPosition.distance = float.MaxValue;
        var convertedPointPosition = transform.InverseTransformPoint(point);
        foreach (var vertex in meshCollider.sharedMesh.vertices)
        {
            if (Vector3.Distance(convertedPointPosition, vertex) < targetPosition.distance)
            {
                if (cancelLowPoints == true)
                {
                    if (transform.TransformPoint(vertex).y > minY)
                    {
                        targetPosition.position = transform.TransformPoint(vertex);
                        targetPosition.distance = Vector3.Distance(convertedPointPosition, vertex);
                    }
                }
                
            }
        }

        return targetPosition;
    }
}
