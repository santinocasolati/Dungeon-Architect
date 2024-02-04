using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private Grid grid;
    private PreviewSystem previewSystem;
    private GridData floorData;
    private GridData objectsData;
    private ObjectPlacer objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData floorData, GridData objectsData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.objectsData = objectsData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos, Vector3 objectPos)
    {
        GridData selectedData = null;

        if (!objectsData.CanPlaceObjectAt(gridPos, Vector2Int.one))
        {
            selectedData = objectsData;
        } else if (!floorData.CanPlaceObjectAt(gridPos, Vector2Int.one))
        {
            selectedData = floorData;
        }

        if (selectedData == null) return;

        gameObjectIndex = selectedData.GetRepresentationIndex(gridPos);

        if (gameObjectIndex == -1) return;

        selectedData.RemoveObjectAt(gridPos);
        objectPlacer.RemoveObjectAt(gameObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), CheckIfSelectionIsValid(gridPos));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPos)
    {
        return !(objectsData.CanPlaceObjectAt(gridPos, Vector2Int.one) && floorData.CanPlaceObjectAt(gridPos, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPos)
    {
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), CheckIfSelectionIsValid(gridPos));
    }
}
