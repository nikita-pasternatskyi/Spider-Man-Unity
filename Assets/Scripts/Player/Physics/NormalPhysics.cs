using UnityEngine;

[System.Serializable]
public class NormalPhysics
{
    [SerializeField] private Gravity _gravity;
    [SerializeField] private CustomPlayerVelocityConstrainer _constrainer;
    [SerializeField] private InputVelocityDamper _damper;
    private GroundMover _mover;

    public void Initialize(PlayerGround ground)
    {
        _constrainer.Init(ground);
        _mover = new GroundMover(ground);
    }

    public PhysicsSystemPreset CreatePreset()
    {
        return new PhysicsSystemPreset(_gravity, _mover, _damper, _constrainer, new StandardVelocityComputer());
    }

    public void UpdateInput(Vector3 input)
    {
        _damper.UpdateInput(input);
    }
}
