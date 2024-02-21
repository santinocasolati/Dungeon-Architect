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
    private ObjectsDatabaseSO database;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData objectsData, ObjectPlacer objectPlacer, ObjectsDatabaseSO database)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.objectsData = objectsData;
        this.objectPlacer = objectPlacer;
        this.database = database;

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

        int objectId = selectedData.GetPositionId(gridPos);

        if (!selectedData.GetIfPosHasBoss(gridPos))
        {
            int sellPrice = database.objects[objectId].Price;
            sellPrice = Mathf.FloorToInt(sellPrice * 0.9f);
            sellPrice = sellPrice == 0 ? 1 : sellPrice;
            CoinsManager.instance.AddCoins(sellPrice);
        }

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
