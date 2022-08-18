using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _lookOffset;
    [SerializeField] private bool _localOffset;

    public Transform Target { get => _target; set => _target = value; }

    private void Start()
    {
        transform.rotation = GetRotationToTarget();
    }

    private void FixedUpdate()
    {
        Quaternion lookRotation = GetRotationToTarget();
        if (transform.rotation != lookRotation) RotateToRotation(lookRotation);
    }

    private void RotateToRotation(Quaternion targetRotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _speed);
    }

    private Quaternion GetRotationToTarget()
    {
        Vector3 lookPosition = _localOffset ? Target.TransformPoint(_lookOffset) : Target.position + _lookOffset;
        Vector3 directionToTarget = (lookPosition - transform.position).normalized;
        return Quaternion.LookRotation(directionToTarget);
    }
}
