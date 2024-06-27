using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingData data;
    private Vector2Int origin;

    public BuildingData Data
    {
        get { return data; }
    }

    public static Building Create(Vector3 worldPos, Vector2Int origin, BuildingData data)
    {
        Transform buildTransform = Instantiate(data.prefab, worldPos, Quaternion.identity);
        Building building = buildTransform.GetComponent<Building>();

        building.origin = origin;
        building.data = data;

        return building;
    }

    public List<Vector2Int> GetGridCoverage()
    {
        return data.GetGridCoverage(origin);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
