using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class BuildingData : ScriptableObject
{
    // Fields
    public string name;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;
    public Dictionary<Items, int> cost;

    // Methods
    public List<Vector2Int> GetGridCoverage(Vector2Int offset)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                list.Add(offset + new Vector2Int(x, y));
            }
        }

        return list;
    }
}
