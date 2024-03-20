using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledTroopsManager : MonoBehaviour
{
    public static KilledTroopsManager _instance;

    [SerializeField] private ObjectPlacer objectPlacer;

    private GridData objectsData;

    private List<Vector3> troopsToRemove = new();

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;
    }

    public static void AssignGridData(GridData data)
    {
        _instance.objectsData = data;
    }

    public static void AddKilled(Vector3 killedPos)
    {
        _instance.troopsToRemove.Add(killedPos);
    }

    public static void StartCleaning()
    {
        _instance.troopsToRemove.ForEach(t =>
        {
            _instance.KillTroop(t);
        });

        _instance.troopsToRemove.Clear();
    }

    private void KillTroop(Vector3 gridPos)
    {
        Vector3Int gridPosInt = new Vector3Int(
            Mathf.FloorToInt(gridPos.x) + 1,
            0,
            Mathf.FloorToInt(gridPos.z) + 1
        );

        int gameObjectIndex = objectsData.GetRepresentationIndex(gridPosInt);

        objectPlacer.RemoveObjectAt(gameObjectIndex, false);
        objectsData.RemoveObjectAt(gridPosInt);
    }
}
