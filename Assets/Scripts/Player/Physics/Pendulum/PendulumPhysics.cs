using UnityEngine;

[System.Serializable]
public class PendulumPhysics
{
    [SerializeField] private PendulumGravity _gravity;
    [SerializeField] private PendulumVelocityConstraints _constrainer;
    [SerializeField] private PendulumVelocityDamper _damper;
    private PendulumMover _pendulumMover;

    public PhysicsSystemPreset CreatePreset()
    {
        return new PhysicsSystemPreset(_gravity, _pendulumMover, _damper, _constrainer);
    }

    public void FixedUpdate(Vector3 currentPosition)
    {
        _constrainer.UpdatePosition(currentPosition);
        _gravity.ChangeDirection(_constrainer.GetDirection());
        _pendulumMover.UpdateDirection(_constrainer.GetDirection());
    }

    public void ChangeTether(Vector3 point, Vector3 currentPosition)
    {
        _constrainer.UpdatePosition(currentPosition);
        if (_pendulumMover == null)
            _pendulumMover = new PendulumMover();
        _constrainer.ChangeTether(point);
    }
}
