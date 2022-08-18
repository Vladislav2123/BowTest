using UnityEngine;
using System.Collections;
using Zenject;

public class Arrow : MonoBehaviour
{
    [Header("=== SPEED ===")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeedDistance;

    [Header("=== TRAJECTORY ===")]
    [SerializeField] private AnimationCurve _yFlyTrajectory;
    [SerializeField] private float _yFlyWeigth = 1;

    [Header("=== TARGETS KILLING ===")]
    [SerializeField] private float _targetsDetectionDistance;
    [SerializeField] private Vector3 _targetDetectionOffset;
    [SerializeField] private Vector3 _stuckOffset;
    [SerializeField] private LayerMask _targetsLayers;

    [SerializeField] private float _destroyDelay;
    [SerializeField] private Rigidbody _rigidbody;

    [Inject] private CameraShake _cameraShake;
    private Vector3 _launchPoint;
    private Vector3 _targetPoint;
    private float _flyTime;
    private float _flySpeed;
    private float _damage;
    private bool _isFlying;
    private bool _isStucked;

    public void Launch(Vector3 targetPosition, float damage)
    {
        _launchPoint = transform.position;
        _targetPoint = targetPosition;
        float distanceToTargetPoint = Vector3.Distance(transform.position, _targetPoint);
        float distanceT = distanceToTargetPoint / _maxSpeedDistance;
        _flySpeed = Mathf.Lerp(_minSpeed, _maxSpeed, distanceT);
        _flyTime = distanceToTargetPoint / _flySpeed;
        _damage = damage;

        StartCoroutine(FlyRoutine());
    }

    private IEnumerator FlyRoutine()
    {
        _isFlying = true;
        float t = 0;

        while(t <= 1)
        {
            transform.position = GetPositionOnTrajectory(t);
            Vector3 trajectoryDirection = (GetPositionOnTrajectory(t + 0.05f) - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(trajectoryDirection);

            t += Time.deltaTime / _flyTime;
            yield return null;
        }

        _isFlying = false;

        if (_isStucked == false)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(transform.forward * _flySpeed, ForceMode.Impulse);
            Destroy(gameObject, _destroyDelay);
        }
    }

    private Vector3 GetPositionOnTrajectory(float time)
    {
        Vector3 targetPosition = Vector3.Lerp(_launchPoint, _targetPoint, time);
        targetPosition.y += _yFlyTrajectory.Evaluate(time) * _yFlyWeigth;
        return targetPosition;
    }

    private void Update()
    {
        TryKillTarget();
    }

    private void TryKillTarget()
    {
        if (_isStucked || _isFlying == false) return;

        Vector3 originRayPoint = transform.position + _targetDetectionOffset;
        if (Physics.Raycast(originRayPoint, transform.forward, out RaycastHit hit, 
            _targetsDetectionDistance, _targetsLayers) == false) return;
        if (hit.transform.TryGetComponent<Target>(out Target hitedTarget) == false) return;

        hitedTarget.TryTakeDamage(_damage, hit.point, _flySpeed, transform.forward);
        if (hitedTarget.IsStruck) _cameraShake.Shake();
        StuckInTransform(hitedTarget.transform, hit.point);
        _isStucked = true;
    }

    private void StuckInTransform(Transform stuckingTransform, Vector3 stuckPoint)
    {
        transform.position = stuckPoint + _stuckOffset;
        transform.SetParent(stuckingTransform);
        _rigidbody.isKinematic = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 originLinePoint = transform.position + _targetDetectionOffset;
        Gizmos.DrawLine(originLinePoint, originLinePoint + transform.forward * _targetsDetectionDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + _stuckOffset, 0.15f);
    }
}
