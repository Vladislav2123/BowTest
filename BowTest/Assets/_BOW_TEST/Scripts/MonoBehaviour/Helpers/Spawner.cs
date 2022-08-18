using System.Collections;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnPrefabs;
    [SerializeField] private int _spawnCount;
    [SerializeField] private Interval _spawnInterval;
    [SerializeField] private Transform[] _spawnPoints;

    [Inject] private DiContainer _diContainer;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        for(int i = 0; i < _spawnCount; i++)
        {
            float delay = Random.Range(_spawnInterval.MinValue, _spawnInterval.MaxValue);
            yield return new WaitForSeconds(delay);

            var randomPrefab = _spawnPrefabs[Random.Range(0, _spawnPrefabs.Length)];
            var randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            _diContainer.InstantiatePrefab(randomPrefab, randomPoint.position, randomPoint.rotation, transform);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if(_spawnPoints != null && _spawnPoints.Length > 0)
        {
            Gizmos.color = Color.blue;
            foreach(var point in _spawnPoints) Gizmos.DrawSphere(point.position, 0.3f);
        }
    }
}
