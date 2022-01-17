using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GraphConfig", menuName = "ScriptableObjects/GraphConfig", order = 1)]

public class GraphConfig : ScriptableObject
{
    [Header("Build settings")]
    public float step = 1f;
    public int countDublin = 3;
    public int countPoints = 6;

    public float offSetDublin = 0.2f;

    public float dublinAngle = 20f;
    public float rootQuadSize = 0.2f;

    public float mergeDistance = 0.5f;

    public float minDistanceDubling = 0.5f;

    [Header("Random")]

    [Range(0, 1)] public float probabilityDublin = 0.2f;
}
