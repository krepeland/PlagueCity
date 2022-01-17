using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoadConfig", menuName = "ScriptableObjects/RoadConfig", order = 1)]
public class RoadConfig : ScriptableObject
{
    [Range(0, 10)] public int countRoads;

    [Range(0, 360)] public float randomRangeDirectAngle;
}
