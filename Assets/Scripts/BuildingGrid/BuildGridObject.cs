using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGridObject : GridObject
{
    // Fields
    public new Grid<BuildGridObject> grid;
    private Building building;
    private Transform cellVisual;

    // Properties
    public bool CanBuild
    {
        get { return building == null; }
    }

    // Constructor
    public BuildGridObject(Grid<BuildGridObject> grid, int x, int y, Transform cellVisual) 
        : base(x, y)
    {
        this.grid = grid;
        this.cellVisual = cellVisual;
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
