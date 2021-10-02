using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplePointer : MonoBehaviour
{
    [SerializeField] private float _maxDistance;
    [SerializeField] private Vector3 _left;
    [SerializeField] private Vector3 _center;
    [SerializeField] private Vector3 _right;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _grappable;

    [Header("Debug")]
    [SerializeField] private Color32 _selectedColor;
    [SerializeField] private PlayerInput _playerInput;

    private RaycastHit[] _results;
    private RaycastHit[] _previousResults;
    public List<GameObject> _objects;

    private void Awake()
    {
        _objects = new List<GameObject>();
    }

    public bool CheckGrapplePoint(Vector3 input, out Vector3 point)
    {
        point = Vector3.zero;
        var rHit = 0f;
        var lHit = 0f;
        if (Physics.SphereCast(transform.position, _radius, _center.normalized, out RaycastHit hit, _maxDistance, _grappable))
        {
            point = hit.point;
            return true;
        }

        if (Physics.SphereCast(transform.position, _radius, _right.normalized, out RaycastHit hitR, _maxDistance, _grappable))
        {
            rHit = hitR.distance;
        }

        if (Physics.SphereCast(transform.position, _radius, _left.normalized, out RaycastHit hitL, _maxDistance, _grappable))
        {
            lHit = hitL.distance;
        }

        if (rHit < lHit)
        {
            point = hitR.point;
            return true;
        }
        if (lHit < rHit)
        {
            point = hitL.point;
            return true;
        }
        return false;
    }

    private void CollectCloseObjects()
    {
        _results = Physics.SphereCastAll(transform.position, _radius, transform.forward, 0f, _grappable);
        foreach (var item in _results)
        {
            if (_objects.Contains(item.collider.gameObject) == false)
            {
                _objects.Add(item.collider.gameObject);
                Added(item.collider.gameObject);
            }
        }

        if (_previousResults != null)
        {
            List<RaycastHit> toRemove = _previousResults.Except(_results).ToList();
            foreach (var item in toRemove)
            {
                _objects.Remove(item.collider.gameObject);
                Removed(item.collider.gameObject);
            }
        }
        _previousResults = _results;
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
        print("r");
        if (obj.TryGetComponent<MeshRenderer>(out MeshRenderer mr))
        {
            mr.material.color = Color.white;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(3, 160, 98, 125);
        Gizmos.DrawSphere(transform.position + _center.normalized * _maxDistance, _radius);
        Gizmos.color = new Color32(255, 255, 255, 125);
        Gizmos.DrawSphere(transform.position + _right.normalized * _maxDistance, _radius);
        Gizmos.color = new Color32(0, 0, 0, 125);
        Gizmos.DrawSphere(transform.position + _left.normalized * _maxDistance, _radius);
    }
}
