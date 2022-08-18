using UnityEngine;
using Zenject;

public class Target : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _gizmosRadius;

    [Inject] protected TargetsHolder _targetsHolder;

    public Vector3 Center => transform.position + _offset;
    public virtual bool IsStruck { get; protected set; }

    public virtual void TryTakeDamage(float damage, Vector3 damagePoint, float damageForce, Vector3 damageDirection) { }

    protected virtual void OnEnable()
    {
        _targetsHolder.TryAddTarget(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Center, _gizmosRadius);
    }
}
