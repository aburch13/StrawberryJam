using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    // Fields
    private Grid<GridObject> grid;
    public int gridWidth;
    public int gridHeight;
    public float cellSize;
    [SerializeField] private List<BuildingData> buildingOptions;
    private int selectedBuildingIndex = 0;

    private void Awake()
    {
        grid = new Grid<GridObject>(
            gridWidth, 
            gridHeight, 
            cellSize, 
            new Vector3(gridWidth, gridHeight) * -cellSize / 2f, 
            (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Attempt to place building on left click
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXY(GetMouseWorldPosition(), out int x, out int y);
            List<Vector2Int> gridCoverage = buildingOptions[selectedBuildingIndex].GetGridCoverage(new Vector2Int(x, y));
            GridObject gridObj = grid.GetItem(x, y);

            // Are all cells in the building's coverage open?
            bool canBuild = true;
            foreach (Vector2Int gridPos in gridCoverage)
            {
                if (!grid.GetItem(gridPos.x, gridPos.y).CanBuild)
                {
                    canBuild = false;
                    break;
                }
            }

            // Create building if all covered cells are open
            // Store reference to building in each cell
            if (canBuild)
            {
                Transform buildTransform = Instantiate(buildingOptions[selectedBuildingIndex].prefab, grid.GetWorldPosition(x, y), Quaternion.identity);

                foreach (Vector2Int gridPos in gridCoverage)
                {
                    grid.GetItem(gridPos.x, gridPos.y).SetTransform(buildTransform);
                }

                Debug.Log(buildingOptions[selectedBuildingIndex].name + " has been built!");
            }
            else
            {
                Debug.Log("Cannot build there!");
            }
        }

        // Change selected building option
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedBuildingIndex -= 1;

            // Loop to end of list as needed
            if (selectedBuildingIndex < 0) 
                selectedBuildingIndex = buildingOptions.Count - 1;

            Debug.Log("Now building: " + buildingOptions[selectedBuildingIndex].name);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedBuildingIndex += 1;

            // Loop to start of list as needed
            if (selectedBuildingIndex >= buildingOptions.Count) 
                selectedBuildingIndex = 0;

            Debug.Log("Now building: " + buildingOptions[selectedBuildingIndex].name);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        return worldPos;
    }
}
