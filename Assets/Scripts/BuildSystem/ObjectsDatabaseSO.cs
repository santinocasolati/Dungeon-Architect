using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlacableObjectsDatabase", menuName = "BuildingSystem/Placeable Objects Database")]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objects;
}

[System.Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int Id { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public ObjectType ObjectType { get; private set; }
}

public enum ObjectType
{
    Floor,
    Structure
}
