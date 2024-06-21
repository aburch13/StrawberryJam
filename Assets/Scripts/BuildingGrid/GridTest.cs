using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    public Grid<int> grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<int>(5, 5, 10, Vector3.zero, () => 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            grid.SetItem(GetMouseWorldPosition(), grid.GetItem(GetMouseWorldPosition()) + 1);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        return worldPos;
    }
}
