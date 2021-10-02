using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererPointsView : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);
    }

    public void AddPoint(Vector3 position)
    {
        _lineRenderer.positionCount = _lineRenderer.positionCount + 1;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position);
    }

    public void Reset()
    {
        _lineRenderer.positionCount = 1;
    }
}
