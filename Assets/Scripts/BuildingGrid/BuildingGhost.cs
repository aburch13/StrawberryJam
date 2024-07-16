using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    // Fields
    private Transform visual;
    private BuildingData data;

    // Start is called before the first frame update
    void Start()
    {
        RefreshVisual();

        BuildingGrid.Instance.OnSelectionChange += OnSelectionChange;
        BuildingGrid.Instance.OnCellMove += OnCellMove;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = BuildingGrid.Instance.CurrentCellWorldPos;
    }

    private void OnSelectionChange(object sender, EventArgs e)
    {
        ClearValidity(BuildingGrid.Instance.CurrentCellGridPos);
        RefreshVisual();
        if (data != null) 
            VisualizeValidity(BuildingGrid.Instance.CurrentCellGridPos);
    }

    private void OnCellMove(object sender, GridMoveEventArgs e)
    {
        ClearValidity(e.lastGridPosition);
        VisualizeValidity(e.currGridPosition);
    }

    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        data = BuildingGrid.Instance.CurrentSelection;

        if (data != null)
        {
            visual = Instantiate(data.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
        }
    }

    private void ClearValidity(Vector2Int oldCellPosition)
    {
        List<Vector2Int> oldCells = data.GetGridCoverage(oldCellPosition);

        foreach (Vector2Int cellPos in oldCells)
        {
            // Reset validity color of any existing grid cells
            BuildingGrid.Instance.GetCellData(cellPos.x, cellPos.y)?.ResetValidity();
        }
    }

    private void VisualizeValidity(Vector2Int currCellPosition)
    {
        List<Vector2Int> currCoverage = data.GetGridCoverage(currCellPosition);
        bool isValidPlacement = true;

        // Check placement validity (all covered cells exist AND can be built on)
        foreach (Vector2Int cellPos in currCoverage)
        {
            BuildGridObject gridObj = BuildingGrid.Instance.GetCellData(cellPos.x, cellPos.y);

            if (gridObj == null || !gridObj.CanBuild)
            {
                isValidPlacement = false;
                break;
            }
        }

        // Color grid cells with color indicating whether placement is valid
        foreach (Vector2Int cellPos in currCoverage)
        {
            BuildingGrid.Instance.GetCellData(cellPos.x, cellPos.y)?.SetValidity(isValidPlacement);
        }
    }
}
