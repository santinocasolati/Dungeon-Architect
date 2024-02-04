using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputDetector inputDetector;
    [SerializeField] private Grid grid;
    [SerializeField] private PreviewSystem previewSystem;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private ObjectPlacer objectPlacer;

    private GridData floorData, objectsData;
    private IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        objectsData = new GridData();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(id, grid, previewSystem, database, floorData, objectsData, objectPlacer);
        inputDetector.OnMousePressed += PlaceStructure;
        inputDetector.OnCancel += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, previewSystem, floorData, objectsData, objectPlacer);
        inputDetector.OnMousePressed += PlaceStructure;
        inputDetector.OnCancel += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputDetector.IsPointerOverUI()) return;

        Vector3 mousePos = inputDetector.GetSelectedMapPosition();
        Vector3 roundedVector = RoundToLowestInt(mousePos);
        Vector3Int gridPos = grid.WorldToCell(roundedVector);

        Vector3 objectPos = grid.CellToWorld(gridPos);
        objectPos.x -= 1;
        objectPos.z -= 1;
        objectPos.y = 0;

        buildingState.OnAction(gridPos, objectPos);
    }

    private void StopPlacement()
    {
        gridVisualization.SetActive(false);
        
        if (buildingState != null)
        {
            buildingState.EndState();
        }

        inputDetector.OnMousePressed -= PlaceStructure;
        inputDetector.OnCancel -= StopPlacement;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null) return;

        Vector3 mousePos = inputDetector.GetSelectedMapPosition();

        Vector3 roundedVector = RoundToLowestInt(mousePos);

        Vector3Int gridPos = grid.WorldToCell(roundedVector);

        buildingState.UpdateState(gridPos);
    }

    private Vector3 RoundToLowestInt(Vector3 inputVector)
    {
        float roundedX = Mathf.Ceil(inputVector.x);
        float roundedY = Mathf.Ceil(inputVector.y);
        float roundedZ = Mathf.Ceil(inputVector.z);

        return new Vector3(roundedX, roundedY, roundedZ);
    }
}
