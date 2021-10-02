using UnityEngine;

[System.Serializable]
public class NormalState : PlayerState
{
    [SerializeField] private float _s;
    [Inject] private PhysicsSystem _physicsSystem;
}


