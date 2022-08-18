using System.Collections;
using UnityEngine;

public class EnemyWalkAroundState : IState
{
    private Enemy _enemy;
    private Coroutine _playingRoutine;
    private bool _isWaiting;

    public EnemyWalkAroundState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.Stop();
    }

    public void Exit()
    {
        _enemy.StopCoroutine(_playingRoutine);
        _enemy.Stop();
    }

    public void Update()
    {
        if (_enemy.Agent.IsPathLaid) return;

        if (_isWaiting == false) _playingRoutine = _enemy.StartCoroutine(GoToNewPointRoutine());
    }

    private IEnumerator GoToNewPointRoutine()
    {
        _isWaiting = true;

        float delay = Random.Range(_enemy.WalkAroundInterval.MinValue, _enemy.WalkAroundInterval.MaxValue);
        yield return new WaitForSeconds(delay);

        Vector3 newPoint = Vector3.zero;
        bool isAchievablePointFound = false;
        while (isAchievablePointFound == false)
        {
            newPoint = _enemy.Agent.FindRandomPoint(_enemy.StartPoint, _enemy.WalkAroundRadius);
            if (_enemy.Agent.IsPointAchievable(newPoint)) isAchievablePointFound = true;
        }

        _enemy.GoToPoint(newPoint);
        _isWaiting = false;
    }
}
