using UnityEngine;

[System.Serializable]
public class PlayerGround : SurfaceSlider
{
    [SerializeField] private Vector3 _groundDirection;
    [SerializeField] private float _groundCheckDistance;
    public bool Grounded { get; private set; }
    public void FixedUpdate(Vector3 currentPosition)
    {
        Grounded = Check(currentPosition, _groundDirection, _groundCheckDistance);
    }
}
