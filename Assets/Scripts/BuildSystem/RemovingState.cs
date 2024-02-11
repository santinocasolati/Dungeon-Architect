using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private Grid grid;
    private PreviewSystem previewSystem;
    private GridData objectsData;
    private ObjectPlacer objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData objectsData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
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

        if (!objectsData.CanPlaceObjectAt(gridPos, Vector2Int.one, grid))
        {
            selectedData = objectsData;
        }

        if (selectedData == null) return;

        gameObjectIndex = selectedData.GetRepresentationIndex(gridPos);

        if (gameObjectIndex == -1) return;

        objectPlacer.RemoveObjectAt(gameObjectIndex, selectedData.GetIfPosHasBoss(gridPos));
        selectedData.RemoveObjectAt(gridPos);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), CheckIfSelectionIsValid(gridPos));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPos)
    {
        return !(objectsData.CanPlaceObjectAt(gridPos, Vector2Int.one, grid));
    }

    public void UpdateState(Vector3Int gridPos)
    {
        previewSystem.UpdatePosition(grid.CellToWorld(gridPos), CheckIfSelectionIsValid(gridPos));
    }
}
