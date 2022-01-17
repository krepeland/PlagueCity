using Assets.Scripts.Classes.Geometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    private GlobalPoint root;
    private bool inicialized = false;
    public bool spawnedBarricade = false;
    public BarricadePoint barricadePoint;
    public void Inicialize(GlobalPoint point)
    {
        root = point;

        inicialized = true;
    }

    public void SpawnBarricade(GameObject prefab)
    {
        var GO = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        GO.name = "Barricade";
        spawnedBarricade = true;
        barricadePoint = GO.GetComponent<BarricadePoint>();
    }

    public GlobalPoint GetRootGP() => root;

    public List<GlobalPoint> GetNeighboringGlobalPoints()
    {
        return root.AllConnectedPoints;
    }

    public List<GridPoint> GetNeighboringBarricade()
    {
        var gps = GetNeighboringGlobalPoints();
        List<GridPoint> grP = new List<GridPoint>();

        foreach (var g in gps)
        {
            var gridPoin = g.GO.GetComponent<GridPoint>();

            if (gridPoin.spawnedBarricade)
                grP.Add(gridPoin);
        }

        return grP;

    }

}
