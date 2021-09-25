using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyVelocityChanges : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void VelocityChanged(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }
}
