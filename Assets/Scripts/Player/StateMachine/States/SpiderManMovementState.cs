using UnityEngine;

[System.Serializable]
public class SpiderManMovementState : SpiderManState
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private Vector3 _normalJump;
    [SerializeField] private Vector3 _chargedJump;

    [StateInject] private SpiderMan _spiderManSM;
    [StateInject] private PlayerInput _playerInput;

    private Vector3 _moveInput;
    private Vector3 _movement;

    private bool _modifier;

    public override void Enter()
    {
        //_playerInput.MovementPressed += OnMoved;
        //_playerInput.ModifierPressed += OnModifierPressed;
        //_playerInput.JumpPressed += OnJumpPressed;
        //_playerInput.MouseMoved += OnMouseMoved;
    }


    public override void Exit()
    {
        //_playerInput.MovementPressed -= OnMoved;
        //_playerInput.ModifierPressed -= OnModifierPressed;
        //_playerInput.JumpPressed -= OnJumpPressed;
        //_playerInput.MouseMoved -= OnMouseMoved;
    }

    //private void OnJumpPressed()
    //{
    //    if (_modifier == true)
    //    {
    //        var translatedChargedJump = _spiderManSM.TransformIntoRelativeToCamera(_chargedJump);
    //        _physics.Jump(translatedChargedJump);
    //        return;
    //    }

    //    var translatedNormalJump = _spiderManSM.TransformIntoRelativeToCamera(_normalJump);
    //    _physics.Jump(translatedNormalJump);
    //}

    //private void OnMoved(Vector3 input)
    //{
    //    _moveInput = input;
    //    _movement = _spiderManSM.TransformIntoRelativeToCamera(input);
    //    if (_modifier == true)
    //    {
    //        _physics.Move(_movement * _runSpeed);
    //        return;
    //    }
    //    _physics.Move(_movement * _walkSpeed);
    //}

    //private void OnModifierPressed(bool obj)
    //{
    //    if (_modifier == false && obj == true)
    //    {
    //        _physics.Move(_movement * _runSpeed);
    //    }
    //    if (_modifier == true && obj == false)
    //    {
    //        _physics.Move(_movement * _walkSpeed);
    //    }
    //    _modifier = obj;
    //}

    //private void OnMouseMoved(Vector2 mousePos)
    //{
    //    _movement = _spiderManSM.TransformIntoRelativeToCamera(_moveInput);
    //    if (_modifier == true)
    //    {
    //        _physics.Move(_movement * _runSpeed);
    //        return;
    //    }
    //    _physics.Move(_movement * _walkSpeed);
    //}

}
