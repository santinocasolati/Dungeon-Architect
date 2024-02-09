using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GridData
{
    public Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionsToOccupy, id, placedObjectIndex);

        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos)) {
                throw new Exception($"Dictionary already contains this cell position: {pos}");
            }

            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }

        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize, Grid grid)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos) || !CheckForObjectAtPosition(grid.CellToWorld(pos)))
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckForObjectAtPosition(UnityEngine.Vector3 position)
    {
        UnityEngine.Vector3 rayOrigin = new UnityEngine.Vector3(position.x, position.y + 1f, position.z);
        UnityEngine.Vector3 rayDirection = position - rayOrigin;

        int placeableLayer = 1 << LayerMask.NameToLayer("Placeable");

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, placeableLayer))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Placeable"))
            {
                return true;
            }
        }

        return false;
    }

    public int GetRepresentationIndex(Vector3Int gridPos)
    {
        if (!placedObjects.ContainsKey(gridPos))
        {
            return -1;
        }

        return placedObjects[gridPos].PlacedObjectIndex;
    }

    public void RemoveObjectAt(Vector3Int gridPos)
    {
        foreach (var pos in placedObjects[gridPos].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
