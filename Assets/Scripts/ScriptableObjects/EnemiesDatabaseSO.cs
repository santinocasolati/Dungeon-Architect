using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesDatabase", menuName = "Database/Enemies Database")]
public class EnemiesDatabaseSO : ScriptableObject
{
    public List<EnemyData> enemies;
}

[System.Serializable]
public class EnemyData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public int CoinsReward { get; private set; }

    [field: SerializeField]
    public int XpReward { get; private set; }
}
