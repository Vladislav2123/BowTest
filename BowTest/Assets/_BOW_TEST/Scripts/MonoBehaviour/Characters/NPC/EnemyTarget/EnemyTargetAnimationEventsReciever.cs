using UnityEngine;

public class EnemyTargetAnimationEventsReciever : MonoBehaviour
{
    private Enemy _enemyTarget;

    private void Awake()
    {
        _enemyTarget = GetComponentInParent<Enemy>();
    }

    public void Hit()
    {
        _enemyTarget.TryHitArcher();
    }
}
