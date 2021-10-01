using UnityEngine;

public class SpiderManSwingPointFinder : MonoBehaviour
{
    [SerializeField] private LayerMask _grappableMask;
    [SerializeField] private Vector3 _center;
    [SerializeField] private Vector3 _left;
    [SerializeField] private Vector3 _right;
    [SerializeField] private Vector3 _perfectAnchor;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.position + _right * 10);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.position + _center * 10);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.position + _left * 10);
    }

    public bool GetAttachPoint(Vector3 input, out Vector3 point)
    {
        point = Vector3.zero;
        UnityEngine.Physics.Raycast(transform.position, transform.position + _right, out RaycastHit hitR, _grappableMask);
        UnityEngine.Physics.Raycast(transform.position, transform.position + _left, out RaycastHit hitL, _grappableMask);
        UnityEngine.Physics.Raycast(transform.position, transform.position + _center, out RaycastHit hitC, _grappableMask);
        if (input.x > 0 && hitR.collider != null)
        {
            point = hitR.point;
            return true;
        }

        if (input.x < 0 && hitL.collider != null)
        {
            point = hitL.point;
            return true;
        }
        return false;
    }
}
