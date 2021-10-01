using UnityEngine;

public class SurfaceSlider : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsSurface;
    [SerializeField] private Vector3 _defaultNormal;
    [SerializeField] private float _minimumSurfaceY;
    public Vector3 CurrentNormal { get; private set; }

    public Vector3 ProjectDirection(Vector3 project)
    {
        return Vector3.ProjectOnPlane(project.normalized, CurrentNormal);
    }
    public Vector3 Project(Vector3 project)
    {
        return Vector3.ProjectOnPlane(project, CurrentNormal);
    }
    public bool Check(Vector3 position, Vector3 checkDirection, float distance)
    {
        if (Physics.Raycast(position, checkDirection, out RaycastHit hit, distance, _whatIsSurface))
        {
            Vector3 currentNormal = hit.normal;
            if (currentNormal.y > _minimumSurfaceY)
            {
                CurrentNormal = currentNormal;
                return true;
            }
        }
        CurrentNormal = _defaultNormal;
        return false;
    }
    public bool Check(Vector3 position, Vector3 checkDirection, float distance, float minimumSurfaceY)
    {
        if (Physics.Raycast(position, checkDirection, out RaycastHit hit, distance, _whatIsSurface))
        {
            Vector3 currentNormal = hit.normal;
            if (currentNormal.y > minimumSurfaceY)
            {
                CurrentNormal = currentNormal;
                return true;
            }
        }
        return false;
    }

}
