using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeVisitor : VisitorSelector
{
    [SerializeField] private GameObject prefab;
    public void MakeBarricade()
    {
        var point = target.GetComponent<GridPoint>();

        if (point != null)
            point.SpawnBarricade(prefab);

        target.GetComponent<VisitorsInfo>().RemoveVisitor<BarricadeVisitor>();
        Destroy(gameObject);
    }
}
