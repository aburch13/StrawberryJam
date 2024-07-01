using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    // Grid fields
    private Grid<BuildGridObject> grid;
    public int gridWidth;
    public int gridHeight;
    public float cellSize;

    // Building selection fields
    [SerializeField] private List<BuildingData> buildingTypes;
    private int selectionIndex = 0;
    private bool isMoving = false;
    private Transform moveGhost;

    // Event fields
    public event EventHandler<EventArgs> OnSelectionChange;

    // Properties
    public static BuildingGrid Instance
    {
        get;
        private set;
    }

    public Vector2Int CurrentCellGridPos
    {
        get
        {
            grid.GetXY(GetMouseWorldPosition(), out int x, out int y);
            return new Vector2Int(x, y);
        }
    }

    public Vector3 CurrentCellWorldPos
    {
        get
        {
            Vector2Int currGridPos = CurrentCellGridPos;
            return grid.GetWorldPosition(currGridPos.x, currGridPos.y);

            // For cell center add: new Vector3(cellSize/2f, cellSize/2f)
        }
    }

    public BuildingData CurrentSelection
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
            (Grid<BuildGridObject> g, int x, int y) => new BuildGridObject(g, x, y));
    }

    // Update is called once per frame
    void Update()
    {
        // Place building on left click
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBuilding();
        }

        // Move building on right click
        // Right click again to cancel moving
        if (Input.GetMouseButtonDown(1))
        {
            if (!isMoving)
                MoveStart();
            else
                MoveCancel();
        }

        // Change selected building option using Q/E
        if (Input.GetKeyDown(KeyCode.Q) && !isMoving)
        {
            selectionIndex -= 1;

            // Loop to end of list as needed
            if (selectionIndex < 0) 
                selectionIndex = buildingTypes.Count - 1;

            OnSelectionChange?.Invoke(this, new EventArgs());
            Debug.Log("Now building: " + buildingTypes[selectionIndex].name);
        }
        if (Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            selectionIndex += 1;

            // Loop to start of list as needed
            if (selectionIndex >= buildingTypes.Count) 
                selectionIndex = 0;

            OnSelectionChange?.Invoke(this, new EventArgs());
            Debug.Log("Now building: " + buildingTypes[selectionIndex].name);
        }
    }

    // Attempts to create a new building at given grid position
    // Returns true if new building was successfully placed
    public bool PlaceBuilding(int gridX, int gridY)
    {
        // CALCULATE GRID COVERAGE
        // Find mouse position on grid and determine all cells covered by selected building
        List<Vector2Int> gridCoverage = buildingTypes[selectionIndex].GetGridCoverage(new Vector2Int(gridX, gridY));

        // VAlIDATE PLACEMENT
        // Check if all cells in the building's coverage are open
        bool canBuild = true;
        foreach (Vector2Int gridPos in gridCoverage)
        {
            if (!grid.GetItem(gridPos.x, gridPos.y).CanBuild)
            {
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
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
        else
        {
            Debug.Log("Cannot build: Invalid placement!");
            return false;
        }
    }

    // Places building at mouse position
    public bool PlaceBuilding()
    {
        grid.GetXY(GetMouseWorldPosition(), out int x, out int y);
        return PlaceBuilding(x, y);
    }

    public void MoveStart()
    {
        BuildGridObject gridObj = grid.GetItem(GetMouseWorldPosition());
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

    public void MoveCancel()
    {
        // Create building at old location
        grid.GetXY(moveGhost.transform.position, out int x, out int y);
        PlaceBuilding(x, y);

        // Moving fields reset by PlaceBuilding
    }

    // Destroys any existing building at current mouse position
    public void DestroyBuilding()
    {
        BuildGridObject gridObj = grid.GetItem(GetMouseWorldPosition());
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

    // Utility method: convert mouse screen position to world XY coordinates
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        return worldPos;
    }
}
