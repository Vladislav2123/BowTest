using UnityEngine;

public class Bone : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;

    public Rigidbody Rigidbody
    {
        get
        {
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            return _rigidbody;
        }
    }

    public Collider Collider
    {
        get
        {
            if (_collider == null) _collider = GetComponent<Collider>();
            return _collider;
        }
    }
}
