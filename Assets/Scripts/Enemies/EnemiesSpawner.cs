using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public Transform enemiesContainer;
    public GameObject[] enemiesPrefab;

    [SerializeField] private float yPos;
    [SerializeField] private Grid grid;

    public void SpawnEnemies(int amount, GameObject[] floors)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject randomFloor = floors[Random.Range(0, floors.Length)];
            Collider collider = randomFloor.GetComponent<Collider>();

            Vector3 randomPoint = GetRandomPoint(collider);
            Vector3Int gridPos = grid.WorldToCell(randomPoint);
            Vector3 enemyPos = grid.CellToWorld(gridPos);
            enemyPos.x += .5f;
            enemyPos.z += .5f;

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
        // TODO: Assign the boss reference to the AI
    }
}
