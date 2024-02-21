using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    private int id;
    private Grid grid;
    private PreviewSystem previewSystem;
    private ObjectsDatabaseSO database;
    private GridData objectsData;
    private ObjectPlacer objectPlacer;

    public PlacementState(int id, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData objectsData, ObjectPlacer objectPlacer)
    {
        this.id = id;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.objectsData = objectsData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objects.FindIndex(data => data.Id == id);

        if (selectedObjectIndex > -1)
        {
            this.previewSystem.StartShowingPlacementPreview(this.database.objects[this.selectedObjectIndex].Prefab, this.database.objects[this.selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {id}");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos, Vector3 objectPos)
    {
        if (!CoinsManager.instance.RemoveCoins(database.objects[selectedObjectIndex].Price)) return;

        bool placementValidity = CheckPlacementValidity(gridPos, selectedObjectIndex);
        if (!placementValidity) return;

        int index = objectPlacer.PlaceObject(database.objects[selectedObjectIndex].Prefab, objectPos, database.objects[selectedObjectIndex].isBoss);

        objectsData.AddObjectAt(gridPos, database.objects[selectedObjectIndex].Size, database.objects[selectedObjectIndex].Id, index, database.objects[selectedObjectIndex].isBoss);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        if (database.objects[selectedObjectIndex].isBoss)
        {
            return (objectsData.CanPlaceObjectAt(gridPosition, database.objects[selectedObjectIndex].Size, grid) && !GameManager.instance.bossPlaced);
        }
        else
        {
            return objectsData.CanPlaceObjectAt(gridPosition, database.objects[selectedObjectIndex].Size, grid);
        }
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool placementValidity = CheckPlacementValidity(gridPos, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), placementValidity);
    }
}
