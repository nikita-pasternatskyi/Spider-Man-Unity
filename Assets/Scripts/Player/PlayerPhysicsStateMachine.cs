using UnityEngine;

[RequireComponent(typeof(RigidbodyVelocityChanges))]
public class PlayerPhysicsStateMachine : MonoBehaviour
{
    [SerializeField] private LineRendererPointsView _lineRendererPoints;
    [SerializeField] private Movement _moveState;
    [SerializeField] private Pendulum _pendulumState;
    [SerializeField] private RigidbodyVelocityChanges _view;
    private IPlayerPhysicsState currentState;

    private void Awake()
    {
        ChangeState(_moveState);
    }

    private void Start()
    {
        _pendulumState.Start();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate(transform.position, Time.deltaTime);
    }

    private void OnEnable()
    {
        _moveState.VelocityCalculated += _view.VelocityChanged;
        _pendulumState.VelocityCalculated += _view.VelocityChanged;
        _pendulumState.Attached += _lineRendererPoints.AddPoint;
    }

    private void OnDisable()
    {
        _moveState.VelocityCalculated -= _view.VelocityChanged;
        _pendulumState.VelocityCalculated -= _view.VelocityChanged;
        _pendulumState.Attached -= _lineRendererPoints.AddPoint;
    }

    public void Jump(Vector3 force)
    {
        currentState.Jump(force);
    }

    public void Move(Vector3 inputForce)
    {
        currentState.Move(inputForce);
    }

    public void Attach(Vector3 point)
    {
        ChangeState(_pendulumState);
        _pendulumState.Attach(transform.position, point);
        _pendulumState.SetVelocity(_moveState.Velocity);
    }

    public void Detach()
    {
        _pendulumState.Detach();
        _lineRendererPoints.Reset();
        ChangeState(_moveState);
        _moveState.SetVelocity(_pendulumState.Velocity);
    }

    private void ChangeState(IPlayerPhysicsState newState)
    {
        currentState = newState;
        currentState.Start();
    }

}
