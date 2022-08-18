using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterMovement))]
public class NPC : MonoBehaviour
{
    private Coroutine _runningFollowingRoutine;

    public GameObject GameObject => gameObject;
    public Transform Transform => transform;
    public CharacterMovement Movement { get; private set; }
    public CustomNavMeshAgent Agent { get; private set; }

    protected virtual void Awake()
    {
        Movement = GetComponent<CharacterMovement>();
        Agent = GetComponent<CustomNavMeshAgent>();
    }

    public void GoToPoint(Vector3 point)
    {
        if (Agent.IsPathLaid) Stop();
        Agent.PavePathToPoint(point);
        StartCoroutine(FollowingPathRoutine());
    }

    private IEnumerator FollowingPathRoutine()
    {
        while (Agent.IsPathLaid)
        {
            if (TryFollowPath() == false) Stop();

            yield return null;
        }
    }

    public bool TryFollowPath()
    {
        if (Agent.IsPathLaid == false) return false;

        float distanceToCurrentCorner = Vector3.Distance(transform.position, Agent.CurrentPathCorner);

        if (distanceToCurrentCorner > Agent.StopDistance)
        {
            MoveToPosition(Agent.CurrentPathCorner);
            return true;
        }
        else return Agent.TryIncreaseCurrentCornerIndex();

    }

    public void Stop()
    {
        if (_runningFollowingRoutine != null) StopCoroutine(_runningFollowingRoutine);
        _runningFollowingRoutine = null;
        Agent.ResetPath();
    }

    public void MoveToPosition(Vector3 position)
    {
        RotateToPosition(position);

        Vector3 directionToPosition = GetDirectionToPosition(position);
        directionToPosition.y = 0;

        Movement.Move(new Vector2(directionToPosition.x, directionToPosition.z), false);
    }

    public void RotateToPosition(Vector3 position)
    {
        Vector3 directionToPosition = GetDirectionToPosition(position);
        directionToPosition.y = 0;

        Movement.Rotate(directionToPosition);
    }

    public Vector3 GetDirectionToPosition(Vector3 targetPosition, float offset = 0)
    {
        return (targetPosition + Vector3.up * offset - transform.position + Vector3.up * offset).normalized;
    }

    public float GetAngleToDirection(Vector3 direction)
    {
        Quaternion rotationInDirection = Quaternion.LookRotation(direction);
        return Quaternion.Angle(transform.rotation, rotationInDirection);
    }
}
