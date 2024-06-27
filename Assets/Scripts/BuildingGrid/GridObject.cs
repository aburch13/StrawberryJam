using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    // Fields
    public Grid<GridObject> grid;
    public int x;
    public int y;
    private Building building;

    // Properties
    public bool CanBuild
    {
        get { return building == null; }
    }

    // Constructor
    public GridObject(Grid<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    // Methods
    public Building GetBuilding()
    {
        return building;
    }

    public void SetBuilding(Building building)
    {
        this.building = building;
        grid.FlagDirty(x, y);
    }

    public void ClearBuilding()
    {
        building = null;
    }

    public override string ToString()
    {
        return x + ", " + y + "\n" + building;
    }
}
