using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildGridObject : GridObject
{
    // Fields
    public new Grid<BuildGridObject> grid;
    private Building building;
    private BuildGridCell cellVisual;

    // Properties
    public bool CanBuild
    {
        get { return building == null; }
    }

    // Constructor
    public BuildGridObject(Grid<BuildGridObject> grid, int x, int y, BuildGridCell cellVisual) 
        : base(x, y)
    {
        this.grid = grid;
        this.cellVisual = cellVisual;
        cellVisual.UpdateColor(cellVisual.canPlaceColor);
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
        cellVisual.UpdateColor(cellVisual.cannotPlaceColor);
    }

    public void ClearBuilding()
    {
        building = null;
        cellVisual.UpdateColor(cellVisual.canPlaceColor);
    }
}
