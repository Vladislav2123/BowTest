using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class EnemyAnimationHandler : MonoBehaviour
{
    private Animator _animator;
    private CharacterMovement _movement;

    private const string ANIMATOR_MOVING_BOOL = "IsMoving";
    private const string ANIMATOR_HIT_TRIGGER = "Attack";

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _movement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        HandleMoveAnimation();
    }

    private void HandleMoveAnimation()
    {
        _animator.SetBool(ANIMATOR_MOVING_BOOL, _movement.IsMoving);
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(ANIMATOR_HIT_TRIGGER);
    }
}
