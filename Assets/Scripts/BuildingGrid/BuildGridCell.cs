using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGridCell : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer cellSprite;
    public Color restingColor;
    public Color canPlaceColor;
    public Color cannotPlaceColor;

    void Awake()
    {
        cellSprite.color = restingColor;
    }

    public void UpdateColor(Color color)
    {
        cellSprite.color = color;
    }
}
