using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Normal")]
    [SerializeField] private PlayerGround _ground;
    [SerializeField] private NormalPhysics _normalPhysics;
    private PhysicsSystemPreset _normalPreset;

    [Header("Pendulum")]
    [SerializeField] private PendulumPhysics _pendulumPhysics;
    private PhysicsSystemPreset _pendulumPreset;

    public bool Swinning { get; private set; }
    private PhysicsSystem _physicsSystem;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _physicsSystem = new PhysicsSystem();
        PhysicsInit(_physicsSystem);
    }

    private void OnEnable()
    {
        _ground.GroundedChanged += OnGroundedChanged;
    }

    private void OnDisable()
    {
        _ground.GroundedChanged -= OnGroundedChanged;
    }

    private void FixedUpdate()
    {
        PhysicsUpdate(transform.position);
    }

    private void OnGroundedChanged(bool obj)
    {
        if (obj == true)
        {
            _physicsSystem.Forces.x = 0;
            _physicsSystem.Forces.z = 0;
        }
    }

    private void PhysicsInit(PhysicsSystem physicsSystem)
    {
        _normalPhysics.Initialize(_ground);
        _pendulumPreset = _pendulumPhysics.CreatePreset();
        _normalPreset = _normalPhysics.CreatePreset();
        _physicsSystem = physicsSystem;

        _physicsSystem.ChangePreset(_normalPreset);
    }

    private void PhysicsUpdate(Vector3 currentPosition)
    {
        if (Swinning == true)
        {
            _pendulumPhysics.FixedUpdate(currentPosition);
        }
        _rigidbody.velocity = _physicsSystem.Compute(Time.deltaTime);
    }

    private void HandlePhysicsSwingChange()
    {
        Swinning = Swinning == true ? false : true;
        if (Swinning == true)
        {
            _physicsSystem.ChangePreset(_pendulumPreset);
            return;
        }
        _physicsSystem.ChangePreset(_normalPreset);
        return;
    }
}
