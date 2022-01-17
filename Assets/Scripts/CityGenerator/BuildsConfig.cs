using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildsConfig", menuName = "ScriptableObjects/BuildsConfig", order = 1)]

public class BuildsConfig : ScriptableObject
{
    public Vector2 SizeBuild;

    public Vector2 SizeRandom;

    public float DistanceBetween;
    public float WidthWay;

    public float DistaceSpawn = 20f;

    public float DistaceTopSpawn = 2f;

    public float GetMaxSize()
    {
        return Mathf.Max(SizeBuild.x, SizeBuild.y);
    }
}
