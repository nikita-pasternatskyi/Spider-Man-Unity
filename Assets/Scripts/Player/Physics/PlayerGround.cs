using System;
using UnityEngine;

[Serializable]
public class PlayerGround : SurfaceSlider
{
    [SerializeField] private Vector3 _groundCheckDirection;
    [SerializeField] private float _groundCheckDistance;
    public bool Grounded { get; private set; }
    public event Action<bool> GroundedChanged;
    public void FixedUpdate()
    {
        var grounded = Check(transform.position, _groundCheckDirection, _groundCheckDistance);
        if (grounded != Grounded)
        {
            Grounded = grounded;
            GroundedChanged?.Invoke(Grounded);
        }
    }
}
