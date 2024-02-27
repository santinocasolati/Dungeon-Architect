using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesDatabase", menuName = "Enemies/Enemies Database")]
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
    public int Reward { get; private set; }
}
