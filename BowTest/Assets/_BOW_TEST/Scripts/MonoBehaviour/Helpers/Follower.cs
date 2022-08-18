using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private bool _localFollowing;
    [SerializeField] private float _distance = 1;

    public Transform Target { get => _target; set => _target = value; }

    private void Start()
    {
        transform.position = GetMovePosition();
    }

    private void FixedUpdate()
    { 
        Vector3 followPosition = GetMovePosition();
        if (transform.position != followPosition) MoveToPosition(followPosition);
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _speed);
    }

    private Vector3 GetMovePosition()
    {
        Vector3 movePosition = Vector3.zero;
        if(_localFollowing)
        {
            return Target.TransformPoint(_offset * _distance);
        }

        return Target.position + _offset * _distance;
    }
}
