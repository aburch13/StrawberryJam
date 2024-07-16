using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridMoveEventArgs : EventArgs
{
    public Vector2Int lastGridPosition;
    public Vector2Int currGridPosition;

    public GridMoveEventArgs(Vector2Int lastGridPosition, Vector2Int currGridPosition)
    {
        this.lastGridPosition = lastGridPosition;
        this.currGridPosition = currGridPosition;
    }
}

public class BuildingGrid : MonoBehaviour
{
    // Grid fields
    private Grid<BuildGridObject> grid;
    public int gridWidth;
    public int gridHeight;
    public float cellSize;
    public Transform cellVisualPrefab;
    private Vector2Int currCellGridPos = Vector2Int.zero;

    // Building selection fields
    [SerializeField] private List<BuildingData> buildingTypes;
    private int selectionIndex = 0;
    private bool isMoving = false;
    private Transform moveGhost;

    // Event fields
    public event EventHandler<EventArgs> OnSelectionChange;
    public event EventHandler<GridMoveEventArgs> OnCellMove;

    // Properties
    public static BuildingGrid Instance // Singleton property for global access
    {
        get;
        private set;
    }

    public Vector2Int CurrentCellGridPos // Mouse position on grid in grid coordinates
    {
        get { return currCellGridPos; }
    }

    public Vector3 CurrentCellWorldPos // Mouse position on grid in world coordinates
    {
        get { return grid.GetWorldPosition(currCellGridPos.x, currCellGridPos.y); }
    }

    public BuildingData CurrentSelection // Currently selected building type
    {
        get { return buildingTypes[selectionIndex]; }
    }

    private void Awake()
    {
        // Singleton configuration for global access to an instance
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        // Instantiate new grid
        grid = new Grid<BuildGridObject>(
            gridWidth, 
            gridHeight, 
            cellSize, 
            new Vector3(gridWidth, gridHeight) * -cellSize / 2f, 
            (Grid<BuildGridObject> g, int x, int y) =>
            {
                Transform cell = Instantiate(cellVisualPrefab, g.GetWorldPosition(x, y), Quaternion.identity, transform);
                cell.transform.localScale *= cellSize;
                return new BuildGridObject(g, x, y, cell.GetComponent<BuildGridCell>());
            });
    }

    // Attempts to create a new building at given grid position
    // Returns true if new building was successfully placed
    public bool PlaceBuilding(int gridX, int gridY)
    {
        // SANITIZE INPUT
        if (gridX < 0 || gridY < 0 || gridX >= gridWidth || gridY >= gridHeight)
        {
            Debug.Log("Cannot build: Outside of grid!");
            return false;
        }

        // CALCULATE GRID COVERAGE
        // Find mouse position on grid and determine all cells covered by selected building
        List<Vector2Int> gridCoverage = buildingTypes[selectionIndex].GetGridCoverage(new Vector2Int(gridX, gridY));

        // VAlIDATE PLACEMENT
        // Check if all cells in the building's coverage are open
        foreach (Vector2Int gridPos in gridCoverage)
        {
            if (grid.GetItem(gridPos.x, gridPos.y) == null)
            {
                Debug.Log("Cannot build: Outside of grid!");
                return false;
            }

            if (!grid.GetItem(gridPos.x, gridPos.y).CanBuild)
            {
                Debug.Log("Cannot build: Invalid placement!");
                return false;
            }
        }
        
        // VERIFY BUILD COST
        // Remove resources from inventory, or cancel build if there are insufficient resources
        if (!Inventory.Instance.RemoveMultiple(buildingTypes[selectionIndex].buildCost))
        {
            Debug.Log("Cannot build: Insufficient resources!");
            return false;
        }

        // CREATE BUILDING
        // If all cells in coverage are open and cost is met, instantiate building and store reference in covered cells
        Building building = Building.Create(grid.GetWorldPosition(gridX, gridY), new Vector2Int(gridX, gridY), buildingTypes[selectionIndex]);
        building.transform.parent = transform.parent;

        foreach (Vector2Int gridPos in gridCoverage)
            grid.GetItem(gridPos.x, gridPos.y).SetBuilding(building);

        Debug.Log(buildingTypes[selectionIndex].name + " has been built!");

        // IF MOVING
        // Destroy ghost and unlock selectionIndex
        if (isMoving)
        {
            isMoving = false;
            Destroy(moveGhost.gameObject);
            moveGhost = null;
        }

        // Build success
        return true;
    }

    // Places building at current position
    public bool PlaceBuilding()
    {
        return PlaceBuilding(currCellGridPos.x, currCellGridPos.y);
    }

    // Switches between starting/ending building move process
    public void ToggleMoveBuilding()
    {
        if (isMoving)
            CancelMoveBuilding();
        else
            StartMoveBuilding();
    }

    // Initiates the process of moving a building
    public void StartMoveBuilding()
    {
        BuildGridObject gridObj = grid.GetItem(currCellGridPos.x, currCellGridPos.y);
        Building building = gridObj.GetBuilding();

        if (building != null)
        {
            // Lock selectionIndex to selected building
            isMoving = true;
            selectionIndex = buildingTypes.IndexOf(building.Data);

            // Destroy building and create ghost visual
            moveGhost = Instantiate(building.Data.visual, grid.GetWorldPosition(gridObj.x, gridObj.y), Quaternion.identity);
            DestroyBuilding();
        }

        // Moving ends when a building is placed (see PlaceBuilding)
    }

    // Cancels the moving process if it is ongoing
    public void CancelMoveBuilding()
    {
        if (!isMoving) return;

        // Create building at old location
        grid.GetXY(moveGhost.transform.position, out int x, out int y);
        PlaceBuilding(x, y);

        // Moving fields reset by PlaceBuilding
    }

    // Changes the currently selected building type
    public void ChangeBuildSelection(bool isForward)
    {
        if (!isMoving)
        {
            if (isForward) selectionIndex++;
            else selectionIndex--;

            // Loop to start/end of list as needed
            if (selectionIndex < 0)
                selectionIndex = buildingTypes.Count - 1;
            if (selectionIndex >= buildingTypes.Count)
                selectionIndex = 0;

            OnSelectionChange?.Invoke(this, new EventArgs());
            Debug.Log("Now building: " + buildingTypes[selectionIndex].name);
        }
    }

    // Destroys any existing building at current position
    public void DestroyBuilding()
    {
        BuildGridObject gridObj = grid.GetItem(currCellGridPos.x, currCellGridPos.y);
        Building building = gridObj.GetBuilding();

        // If building exists at selected cell, remove all cell references and destroy it
        if (building != null)
        {
            // Refund build cost
            Inventory.Instance.AddMultiple(building.Data.buildCost);

            // Remove grid cell references
            List<Vector2Int> gridCoverage = building.GetGridCoverage();
            foreach (Vector2Int gridPos in gridCoverage)
                grid.GetItem(gridPos.x, gridPos.y).ClearBuilding();

            // Destroy building
            building.DestroySelf();
        }
    }

    // Get grid object at given position
    public BuildGridObject GetCellData(int x, int y)
    {
        return grid.GetItem(x, y);
    }

    // Update current cell position to mouse position
    public void MoveToMouseCell()
    {
        grid.GetXY(GetMouseWorldPosition(), out int x, out int y);
        Vector2Int newGridPos = new(x, y);
        if (newGridPos.x < 0 || newGridPos.x >= gridWidth || newGridPos.y < 0 || newGridPos.y >= gridHeight) return;

        if (currCellGridPos != newGridPos)
        {
            OnCellMove?.Invoke(this, new GridMoveEventArgs(currCellGridPos, newGridPos));
            currCellGridPos = newGridPos;
        }
    }

    // Update current cell position by given offsets
    public void MoveCurrentCell(int dx, int dy)
    {
        if (dx == 0 && dy == 0) return;

        Vector2Int newGridPos = new(currCellGridPos.x + dx, currCellGridPos.y + dy);
        if (newGridPos.x < 0 || newGridPos.x >= gridWidth || newGridPos.y < 0 || newGridPos.y >= gridHeight) return;

        OnCellMove?.Invoke(this, new GridMoveEventArgs(currCellGridPos, newGridPos));
        currCellGridPos = newGridPos;
    }

    // Utility method: convert mouse screen position to world XY coordinates
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        return worldPos;
    }
}
