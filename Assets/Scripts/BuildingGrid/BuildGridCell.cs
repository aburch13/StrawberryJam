using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGridCell : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer cellSprite;
    public readonly Color restingColor;
    public Color canPlaceColor;
    public Color cannotPlaceColor;

    public void UpdateColor(Color color)
    {
        cellSprite.color = color;
    }
}
