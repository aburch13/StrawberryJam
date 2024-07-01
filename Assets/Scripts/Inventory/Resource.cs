using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resources
{
    Wood,
    Stone,
    Food
}

[Serializable]
public class Resource
{
    public Resources type;
    public int amount;
}
