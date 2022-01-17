using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaysConfig", menuName = "ScriptableObjects/WaysConfig", order = 1)]
public class WaysConfig : ScriptableObject
{
    public int density = 1;
    public float size = 1;

    public int countMainStreets = 3;
    public int countLevelStreets = 3;

    public Vector2 perlinPixelZone;
    [Range(0, 1)] public float intensityOnPerlin;
    public float stepOnPerlin = 2.4f;

    public float distanceBetweenPoints = 0.5f;
    public float maximalDistanceConnect = 0.7f;
    public Vector2 offsetPoints;

    public int seed = 0;
}
