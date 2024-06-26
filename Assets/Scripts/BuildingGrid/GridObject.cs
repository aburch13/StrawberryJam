using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    // Fields
    public Grid<GridObject> grid;
    public int x;
    public int y;
    public int value;
    private Transform transform;

    // Properties
    public bool CanBuild
    {
        get { return transform == null; }
    }

    // Constructor
    public GridObject(Grid<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    // Methods
    public void SetTransform(Transform transform)
    {
        this.transform = transform;
        grid.FlagDirty(x, y);
    }

    public void ClearTransform()
    {
        transform = null;
    }

    public void AddValue(int value)
    {
        this.value += value;
        grid.FlagDirty(x, y);
    }

    public override string ToString()
    {
        return x + ", " + y + "\n" + transform;
    }
}
