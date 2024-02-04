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
    private GridData floorData;
    private GridData objectsData;
    private ObjectPlacer objectPlacer;

    public PlacementState(int id, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData floorData, GridData objectsData, ObjectPlacer objectPlacer)
    {
        this.id = id;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
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
        bool placementValidity = CheckPlacementValidity(gridPos, selectedObjectIndex);
        if (!placementValidity) return;

        int index = objectPlacer.PlaceObject(database.objects[selectedObjectIndex].Prefab, objectPos);

        GridData selectedData = database.objects[selectedObjectIndex].ObjectType == ObjectType.Floor ? floorData : objectsData;
        selectedData.AddObjectAt(gridPos, database.objects[selectedObjectIndex].Size, database.objects[selectedObjectIndex].Id, index);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objects[selectedObjectIndex].ObjectType == ObjectType.Floor ? floorData : objectsData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objects[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool placementValidity = CheckPlacementValidity(gridPos, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), placementValidity);
    }
}
