using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] private WayPoint root;
    [SerializeField] private List<WayPoint> childs;

    public void Start()
    {
        childs = new List<WayPoint>();
    }

    public void SetRoot(WayPoint root)
    {
        this.root = root;
    }

    public void AddChildWayPoint(WayPoint point)
    {
        childs.Add(point);
    }
}
