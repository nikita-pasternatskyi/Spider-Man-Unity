using UnityEngine;

[System.Serializable]
public class SwingState : PlayerState
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _boost;
    [SerializeField] private Vector3 _jump;

    [Inject] private TransformRelativeConvertor _relativeConvertor;
    [Inject] private PlayerPhysics _physicsSystem;

    private Vector3 _currentInput;
    private bool _modified;

    public override void Enter()
    {
        Debug.Log("Swing");
    }

    public override void Exit()
    {
         Debug.Log("Bye");
    }

    public override void OnMoved(Vector3 input)
    {
        _currentInput = input;
        MovePlayer();
    }

    public override void OnModifierPressed(bool obj)
    {
        _modified = obj;
        if (_modified == true)
            _physicsSystem.AddForce(_relativeConvertor.ConvertToRelative(_boost));
    }

    public override void OnJumpPressed()
    {
        Jump();
    }

    public override void OnMouseMoved(Vector2 input)
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        _physicsSystem.Move(_relativeConvertor.ConvertToRelative(_currentInput * _speed));
    }

    private void Jump()
    {
        _physicsSystem.AddForce(_relativeConvertor.ConvertToRelative(_jump));
    }
}
