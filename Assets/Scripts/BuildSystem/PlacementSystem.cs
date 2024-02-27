using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private InputDetector inputDetector;
    [SerializeField] private Grid grid;
    [SerializeField] private PreviewSystem previewSystem;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private string gridVisualizationTag;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private float yPos;

    private GridData objectsData;
    private IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        objectsData = new GridData();
        KilledTroopsManager.instance.AssignGridData(objectsData);
    }

    public void StartPlacement(int id)
    {
        StopPlacement();

        VisualizatorStateSetter(true);

        buildingState = new PlacementState(id, grid, previewSystem, database, objectsData, objectPlacer);
        inputDetector.OnMousePressed += PlaceStructure;
        inputDetector.OnCancel += StopPlacement;
    }

    private void VisualizatorStateSetter(bool state)
    {
        GameObject[] visualizators = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.CompareTag(gridVisualizationTag) && obj.scene.isLoaded).ToArray();

        foreach (GameObject visualizator in visualizators)
        {
            visualizator.SetActive(state);
        }
    }

    public void StartRemoving()
    {
        StopPlacement();

        VisualizatorStateSetter(true);

        buildingState = new RemovingState(grid, previewSystem, objectsData, objectPlacer, database);
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
        objectPos.y = yPos;

        buildingState.OnAction(gridPos, objectPos);
    }

    public void StopPlacement()
    {
        VisualizatorStateSetter(false);
        
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
