// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplePointFinder : MonoBehaviour
{
    [SerializeField] private float _cornerBonus;
    [SerializeField] private float _maximumDistanceToCorner;
    [SerializeField] private float _YThreshold;
    [SerializeField] private float _perfectDistance;
    [SerializeField] private float _perfectRopeZ;
    [SerializeField] private float _perfectRopeHeight;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _grappable;

    [Header("Debug")]
    [SerializeField] private Color32 _selectedColor;

    private Collider[] _previousSphereCastResults;
    private Dictionary<Grappable, PossibleTetherPoint> _grappablePoints;

    private void Awake()
    {
        _grappablePoints = new Dictionary<Grappable, PossibleTetherPoint>();
    }

    public bool FindGrapplePoint(Vector3 input, Vector3 forward, Vector3 right, out Vector3 point)
    {
        point = Vector3.zero;
        CollectCloseObjects(input);
        if (_grappablePoints.Count == 0)
        {
            return false;
        }

        point = PickPoint(input, forward, right);

        if (point == Vector3.zero)
            Debug.LogError($"ERROR: POINT EQUALS TO ZERO: {input} {forward} {right}");
        _grappablePoints.Clear();
        _previousSphereCastResults = null;
        return true;
    }

    private Collider[] CollectCloseObjects(Vector3 input)
    {
        Collider[] currentColliders = Physics.OverlapSphere(transform.position, _radius, _grappable);
        foreach (var collider in currentColliders)
        {
            if (collider.TryGetComponent(out Grappable grappable) == true)
            {
                if (_grappablePoints.ContainsKey(grappable) == false)
                {
                    var heading = grappable.transform.position - transform.position;
                    var positionedInFront = Vector3.Dot(heading, transform.forward) > 0;
                    var inputInFront = input.z >= 0;
                    var cross = Vector3.Cross(transform.forward, heading);
                    if (positionedInFront == inputInFront)
                    {
                        AddGrapplePointFromInput(input, grappable, cross);
                    }
                }
            }
        }

        return currentColliders;
    }

    private void AddGrapplePointFromInput(Vector3 input, Grappable grappable, Vector3 crossProduct)
    {
        if (input.x == 0)
        {
            AddGrapplePoint(grappable);
        }

        else if (crossProduct.y.SameSign(input.x))
        {
            AddGrapplePoint(grappable);
        }
    }

    private void AddGrapplePoint(Grappable grappable)
    {
        _grappablePoints.Add(grappable, new PossibleTetherPoint());
        Added(grappable.gameObject);
    }


    private Vector3 PickPoint(Vector3 input, Vector3 forward, Vector3 right)
    {
        var direction = new Vector3(input.x, _perfectRopeHeight, _perfectRopeZ);
        var endDirection = forward.normalized * direction.z + right.normalized * direction.x;

        ScorePoints(endDirection);

        (Vector3 point, float distance) target;
        target.point = Vector3.zero;
        target.distance = float.MaxValue;
        foreach (var grappable in _grappablePoints)
        {
            if (grappable.Value.Score < target.distance)
            {
                target.point = grappable.Value.Position;
                target.distance = grappable.Value.Score;
            }
        }
        return target.point;
    }

    private void ScorePoints(Vector3 direction)
    {

        foreach (var grappablePoint in _grappablePoints)
        {
            var point = transform.position + direction;
            var closestPoint = grappablePoint.Key.GetClosestPoint(point);
            var cornerPoint = grappablePoint.Key.GetClosestCorner(point, true, transform.localPosition.y - _YThreshold);
            
            if (cornerPoint.distance < _maximumDistanceToCorner)
            {
                grappablePoint.Value.Score = Mathf.Abs(_perfectDistance - cornerPoint.distance) - _cornerBonus;
                grappablePoint.Value.Position = cornerPoint.point;
                Debug.Log($"{grappablePoint.Value.Score} : corner", grappablePoint.Key.gameObject);
                Debug.Log($"{grappablePoint.Value.Position} : corner", grappablePoint.Key.gameObject);
                continue;
            }
            grappablePoint.Value.Score = Mathf.Abs(_perfectDistance - closestPoint.distance);
            grappablePoint.Value.Position = closestPoint.point;
            Debug.Log($"{grappablePoint.Value.Score} : surface", grappablePoint.Key.gameObject);
            Debug.Log($"{grappablePoint.Value.Position} : surface", grappablePoint.Key.gameObject);
        }

        //if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _perfectDistance))
        //{
        //    if (hit.collider.TryGetComponent(out Grappable grappable) == true)
        //    {
        //        _grappablePoints.Add(new Grappable(), new PossibleTetherPoint(Mathf.Abs(_perfectDistance - hit.distance)));
        //        Debug.Log($"got a hit - {hit.point}");
        //    }
        //}
    }

    private void Added(GameObject obj)
    {
        if (obj.TryGetComponent<MeshRenderer>(out MeshRenderer mr))
        {
            mr.material.color = _selectedColor;
        }
    }

    private void Removed(GameObject obj)
    {
        if (obj.TryGetComponent<MeshRenderer>(out MeshRenderer mr))
        {
            mr.material.color = Color.white;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(3, 160, 98, 125);
        Gizmos.DrawSphere(transform.position, _radius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(0, _perfectRopeHeight, _perfectRopeZ).normalized * _perfectDistance);
    }
}
