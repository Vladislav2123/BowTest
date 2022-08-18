using UnityEngine;
using UnityEngine.AI;

public class CustomNavMeshAgent : MonoBehaviour
{
    [SerializeField] private float _stopDistance;
    [SerializeField] private float _viewAngle;
    [SerializeField] private LayerMask _obstacleLayers;

    public float StopDistance => _stopDistance;
    public float ViewAngle => _viewAngle;
    public NavMeshPath Path { get; private set; }
    public int CurrentCornerIndex { get; private set; }
    public Vector3 CurrentPathCorner => IsPathLaid ? Path.corners[CurrentCornerIndex] : Vector3.zero;
    public Vector3 Destination => IsPathLaid ? Path.corners[Path.corners.Length - 1] : Vector3.zero;
    public bool IsPathLaid => Path != null;
    public bool IsPathAchievable => IsPathLaid ? Path.status == NavMeshPathStatus.PathComplete : false;
    public LayerMask ObstacleLayers => _obstacleLayers;

    public void PavePathToRandomPoint(Vector3 sourcePosition, float radius)
    {
        Vector3 randomPoint = FindRandomPoint(sourcePosition, radius);
        PavePathToPoint(randomPoint);
    }

    public void PavePathToRandomPointWithoutObstacle(Vector3 sourcePosition, float radius)
    {
        Vector3 randomPointWithoutObstacle = FindRandomPointWithoutObstacle(sourcePosition, radius);
        PavePathToPoint(randomPointWithoutObstacle);
    }

    public Vector3 FindRandomPoint(Vector3 sourcePosition, float radius)
    {
        Vector3 randomDirection = sourcePosition + (Random.insideUnitSphere * radius);
        NavMesh.SamplePosition(randomDirection, out NavMeshHit navMeshHit, radius, NavMesh.AllAreas);
        return navMeshHit.position;
    }

    public Vector3 FindRandomPointWithoutObstacle(Vector3 sourcePosition, float radius)
    {
        Vector3 randomPosition = Vector3.zero;
        bool isValidPositionFinded = false;
        while (isValidPositionFinded == false)
        {
            randomPosition = FindRandomPoint(sourcePosition, radius);

            if (randomPosition.y < -10000 || randomPosition.y > 10000) continue;

            if (Physics.Linecast(sourcePosition + Vector3.up, randomPosition, out RaycastHit navMeshHit, _obstacleLayers) == false)
            {
                isValidPositionFinded = true;
                Debug.DrawLine(randomPosition, randomPosition + Vector3.up, Color.green, 10f);
                Debug.DrawLine(sourcePosition + Vector3.up * 0.2f, randomPosition, Color.yellow, 10f);
            }
        }

        return randomPosition;
    }

    public void PavePathToPoint(Vector3 targetPoint)
    {
        Path = new NavMeshPath();
        NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10, NavMesh.AllAreas);
        NavMesh.CalculatePath(hit.position, targetPoint, NavMesh.AllAreas, Path);
        CurrentCornerIndex = 0;
    }

    public void ResetPath()
    {
        Path = null;
        CurrentCornerIndex = 0;
    }

    public bool TryIncreaseCurrentCornerIndex()
    {
        if (IsPathLaid == false) return false;

        if (CurrentCornerIndex < Path.corners.Length - 1)
        {
            CurrentCornerIndex++;
            return true;
        }

        return false;
    }

    public bool IsPointAchievable(Vector3 point)
    {
        NavMeshPath testPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, testPath);
        return testPath.status == NavMeshPathStatus.PathComplete;
    }

    private void OnDrawGizmosSelected()
    {
        if (Path == null) return;

        for (int i = 0; i < Path.corners.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Path.corners[i], 0.15f);

            if (i < Path.corners.Length - 1)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(Path.corners[i], Path.corners[i + 1]);
            }
        }
    }
}
