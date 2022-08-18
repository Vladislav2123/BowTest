using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStatesController : StatesController
{
    private Enemy _enemyTarget;

    protected override void Awake()
    {
        _enemyTarget = GetComponent<Enemy>();

        base.Awake();
    }

    protected override void InitializeStates()
    {
        _states[typeof(NPCIdleState)] = new NPCIdleState();
        _states[typeof(EnemyWalkAroundState)] = new EnemyWalkAroundState(_enemyTarget);
        _states[typeof(EnemyStateAttack)] = new EnemyStateAttack(_enemyTarget);
    }

    public void SetStateIdle()
    {
        IState state = GetState<NPCIdleState>();
        SetState(state);
    }

    public void SetStateAttack()
    {
        IState state = GetState<EnemyStateAttack>();
        SetState(state);
    }

    public void SetStateWalkAround()
    {
        IState state = GetState<EnemyWalkAroundState>();
        SetState(state);
    }
}
