using System;
using UnityEngine;

public interface IMovementInputProvider
{
    public event Action<Vector2> MovementPressed;
}
