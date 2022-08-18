using UnityEngine;

public class EnemyStateAttack : IState
{
    private Enemy _enemy;

    public EnemyStateAttack(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.Stop();
        _enemy.GoToPoint(_enemy.TargetArcher.transform.position);
    }

    public void Exit()
    {
        _enemy.Stop();
    }

    public void Update()
    {
        float _distanceToArcher = Vector3.Distance(_enemy.transform.position, _enemy.TargetArcher.transform.position);
        if(_distanceToArcher <= _enemy.AttackDistance)
        {
            _enemy.Stop();
            _enemy.TryAttackArcher();
            _enemy.StatesController.SetStateIdle();
        }
    }
}
