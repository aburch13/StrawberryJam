using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGridObject : GridObject
{
    // Fields
    public new Grid<BuildGridObject> grid;
    private Building building;

    // Properties
    public bool CanBuild
    {
        get { return building == null; }
    }

    // Constructor
    public BuildGridObject(Grid<BuildGridObject> grid, int x, int y) 
        : base(x, y)
    {
        this.grid = grid;
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
}
