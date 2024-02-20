using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public Transform enemiesContainer;
    public GameObject[] enemiesPrefab;

    [SerializeField] private float yPos;
    [SerializeField] private Grid grid;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    public void SpawnEnemies(int amount, List<GameObject> floors)
    {
        aliveEnemies.Clear();

        for (int i = 0; i < amount; i++)
        {
            GameObject randomFloor = floors[Random.Range(0, floors.Count)];
            Collider collider = randomFloor.GetComponent<Collider>();

            Vector3 randomPoint = GetRandomPoint(collider);
            Vector3Int gridPos = grid.WorldToCell(randomPoint);
            Vector3 enemyPos = grid.CellToWorld(gridPos);
            enemyPos.x -= 1;
            enemyPos.z -= 1;

            InstantiateEnemy(enemyPos);
        }
    }

    private Vector3 GetRandomPoint(Collider collider)
    {
        Vector3 randomPoint = Vector3.zero;
        Bounds bounds = collider.bounds;
        randomPoint.x = Mathf.Ceil(Random.Range(bounds.min.x, bounds.max.x));
        randomPoint.y = yPos;
        randomPoint.z = Mathf.Ceil(Random.Range(bounds.min.z, bounds.max.z));
        return randomPoint;
    }

    private void InstantiateEnemy(Vector3 enemyPos)
    {
        GameObject selectedEnemy = enemiesPrefab[Random.Range(0, enemiesPrefab.Length)];
        GameObject spawnedEnemy = Instantiate(selectedEnemy, enemyPos, selectedEnemy.transform.rotation, enemiesContainer);
        aliveEnemies.Add(spawnedEnemy);

        HealthHandler enemyHealth = spawnedEnemy.GetComponent<HealthHandler>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath.AddListener(() => OnEnemyDeath(spawnedEnemy));
        }
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
        Destroy(enemy);

        if (aliveEnemies.Count == 0)
        {
            RoundCleared();
        }
    }

    private void RoundCleared()
    {
        RoundManager.instance.EndRound(true);
    }
}
