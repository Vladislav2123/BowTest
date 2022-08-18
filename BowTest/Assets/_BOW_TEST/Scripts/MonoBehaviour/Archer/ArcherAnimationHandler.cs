using UnityEngine;

public class ArcherAnimationHandler : MonoBehaviour
{
    private Animator _animator;

    private const string IS_AIMING_BOOL = "IsAiming";

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void PlayAimAnimation()
    {
        _animator.SetBool(IS_AIMING_BOOL, true);
    }

    public void StopAimAnimation()
    {
        _animator.SetBool(IS_AIMING_BOOL, false);
    }
}
