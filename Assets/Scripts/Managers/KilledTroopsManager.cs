using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledTroopsManager : MonoBehaviour
{
    public static KilledTroopsManager instance;

    [SerializeField] private ObjectPlacer objectPlacer;

    private GridData objectsData;

    private List<Vector3> troopsToRemove = new();

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    public void AssignGridData(GridData data)
    {
        objectsData = data;
    }

    public void AddKilled(Vector3 killedPos)
    {
        troopsToRemove.Add(killedPos);
    }

    public void StartCleaning()
    {
        troopsToRemove.ForEach(t =>
        {
            KillTroop(t);
        });

        troopsToRemove.Clear();
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
