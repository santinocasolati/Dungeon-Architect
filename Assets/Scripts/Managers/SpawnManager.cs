using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public Transform enemiesContainer;
    public EnemiesDatabaseSO enemiesDatabase;

    [SerializeField] private float yPos;
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemiesLayer;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;

        UtilitiesFunctions.instance.ManagerSingleton(gameObject);
    }

    public void ResetManager()
    {
        aliveEnemies.Clear();
    }

    public void SpawnEnemies(int amount, List<GameObject> floors)
    {
        aliveEnemies.Clear();

        for (int i = 0; i < amount; i++)
        {
            GameObject randomFloor = floors[Random.Range(0, floors.Count)];
            Collider collider = randomFloor.GetComponent<Collider>();

            EnemyData selectedEnemy = enemiesDatabase.enemies[Random.Range(0, enemiesDatabase.enemies.Count)];

            bool positionValid = false;
            Vector3 enemyPos = Vector3.zero;

            while (!positionValid)
            {
                Vector3 randomPoint = GetRandomPoint(collider);
                Vector3Int gridPos = grid.WorldToCell(randomPoint);
                enemyPos = grid.CellToWorld(gridPos);
                enemyPos.x += .5f;
                enemyPos.z += .5f;

                positionValid = IsPositionValid(enemyPos, selectedEnemy.Size);
            }

            InstantiateEnemy(enemyPos, selectedEnemy);
        }
    }

    private bool IsPositionValid(Vector3 position, Vector2Int enemySize)
    {
        for (int x = 0; x < enemySize.x; x++)
        {
            for (int y = 0; y < enemySize.y; y++)
            {
                Vector3 checkPosition = new Vector3(position.x + x, 0, position.z + y);
                Collider[] enemies = Physics.OverlapSphere(checkPosition, 0.5f, enemiesLayer);

                if (enemies.Length > 0) return false;

                Collider[] ground = Physics.OverlapSphere(checkPosition, 0.5f, groundLayer);

                if (ground.Length == 0) return false;
            }
        }

        return true;
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

    private void InstantiateEnemy(Vector3 enemyPos, EnemyData selectedEnemy)
    {
        GameObject spawnedEnemy = Instantiate(selectedEnemy.Prefab, enemyPos, selectedEnemy.Prefab.transform.rotation, enemiesContainer);
        aliveEnemies.Add(spawnedEnemy);

        HealthHandler enemyHealth = spawnedEnemy.GetComponent<HealthHandler>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath.AddListener(() => OnEnemyDeath(spawnedEnemy, selectedEnemy.Reward));
        }
    }

    private void OnEnemyDeath(GameObject enemy, int reward)
    {
        aliveEnemies.Remove(enemy);
        Destroy(enemy);
        CoinsManager.instance.AddCoins(reward);

        if (aliveEnemies.Count == 0)
        {
            RoundCleared();
        }
    }

    private void RoundCleared()
    {
        RoundManager.instance.EndRound(true);
    }

    public void DespawnEnemies()
    {
        aliveEnemies.ForEach(enemy =>
        {
            Destroy(enemy);
        });

        aliveEnemies.Clear();
    }
}
