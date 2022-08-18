using UnityEngine;

public class CharacterBoneTarget : Target
{
    [SerializeField] private ParticleSystem _bloodFX;

    private IAlive _health;
    private Rigidbody _rigidbody;

    public override bool IsStruck => _health.IsDead;

    private void Awake()
    {
        _health = GetComponentInParent<IAlive>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void TryTakeDamage(float damage, Vector3 damagePosition, float damageForce, Vector3 damageDirection)
    {
        if (IsStruck) return;

        _health.Health -= damage;
        if(_bloodFX != null) _bloodFX.Play();
        if (_health.IsDead)
        {
            _rigidbody.AddForceAtPosition(damageDirection * damageForce, damagePosition);
            _targetsHolder.TryRemoveTarget(this);
        }
    }
}
