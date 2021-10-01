using UnityEngine;

[System.Serializable]
public class SpiderManSwingState : SpiderManState
{
    [SerializeField] private float _swingSpeed;
    [SerializeField] private Vector3 _swingBoost;
    [SerializeField] private Vector3 _swingJump;

    [StateInject] private PlayerInput _playerInput;
    [StateInject] private SpiderMan _spiderManSM;

    private bool _boosted;

    public override void Enter()
    {
    //    _playerInput.MovementPressed += OnMoved;
    //    _playerInput.ModifierPressed += OnModifierPressed;
    //    _playerInput.JumpPressed += OnJumpPressed;
    }

    public override void Exit()
    {
        //_playerInput.MovementPressed -= OnMoved;
        //_playerInput.ModifierPressed -= OnModifierPressed;
        //_playerInput.JumpPressed -= OnJumpPressed;
    }

    //private void OnMoved(Vector3 input)
    //{
    //    _physics.Move(_spiderManSM.TransformIntoRelativeToCamera(input) * _swingSpeed);
    //}

    //private void OnModifierPressed(bool obj)
    //{
    //    if (obj == true && _boosted == false)
    //    {
    //        _physics.AddForce(_spiderManSM.TransformIntoRelativeToCamera(_swingBoost));
    //        _boosted = true;
    //    }
    //}

    //private void OnJumpPressed()
    //{
    //    _physics.Jump(_swingJump);
    //}

}
