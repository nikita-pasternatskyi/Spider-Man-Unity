using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpiderManSwingPointFinder))]
public class SpiderMan : SimpleStateMachine<SpiderManState>
{
    [SerializeField] private TransformRelativeConvertor _transformRelativeConvertor;
    [State] [SerializeField] private SpiderManMovementState _spiderManMovementState;
    [State] [SerializeField] private SpiderManSwingState _spiderManSwingState;

    [StateDependencyResolver] private SpiderMan _spiderManSM;
    [StateDependencyResolver] private PlayerInput _playerInput;

    private SpiderManSwingPointFinder _SMSPF;
    public Vector3 TransformIntoRelativeToCamera(Vector3 input) => _transformRelativeConvertor.ConvertToRelative(input);

    private void Awake()
    {
        _spiderManSM = this;
        _SMSPF = GetComponent<SpiderManSwingPointFinder>();
        _playerInput = GetComponent<PlayerInput>();
        InitStates();
        InitDependencies();
        ChangeState(_spiderManMovementState);
    }

    private void OnEnable()
    {
        _playerInput.SwingPressed += OnSwingPressed;
    }

    private void OnDisable()
    {
        _playerInput.SwingPressed -= OnSwingPressed;
    }

    private void OnSwingPressed()
    {
        //if (_physics.IsAttached == false)
        //{
        //    _physics.Attach(new Vector3(transform.position.x, transform.position.y + 20,transform.position.z + 40));
        //    ChangeState(_spiderManSwingState);
        //    //if (_SMSPF.GetAttachPoint(TransformIntoRelativeToCamera(_playerInput.MovementInput), out Vector3 point))
        //    //{
        //    //    _physics.Attach(point);
        //    //    ChangeState(_spiderManSwingState);
        //    //}
        //    return;
        //}
        //_physics.Detach();
        ChangeState(_spiderManMovementState);
    }
}
