using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorsInfo : MonoBehaviour
{
    [SerializeField] protected List<GameObject> visitors = new List<GameObject>();
    void Start()
    {
        if (visitors != null && visitors.Count > 0)
        {
            foreach (var visitor in visitors)
            {
                if (visitor != null)
                {
                    VisitorSelector visCom;
                    if (!visitor.TryGetComponent<VisitorSelector>(out visCom))
                        throw new System.Exception("ItemsStorageSelector: have false visitor GameObject");
                }
            }
        }
    }

    public void AddVisitor(params GameObject[] visitors)
    {
        if (visitors != null)
        {
            foreach (var vis in visitors)
            {
                if (vis != null)
                    this.visitors.Add(vis);
            }
        }
    }

    public void RemoveVisitor<T>() where T : VisitorSelector
    {
        List<GameObject> list = new List<GameObject>();
        foreach (var vis in visitors)
        {
            var v = vis.GetComponent<VisitorSelector>();
            if (v == null)
                throw new System.Exception($"On {vis.name} absent {typeof(VisitorSelector).Name}");


            if (!(vis.GetComponent<VisitorSelector>() is T))
                list.Add(vis);
            
        }
        visitors = list;
    }

    public GameObject GetVisitor<T>() where T: VisitorSelector
    {
        foreach (var vis in visitors)
        {
            var v = vis.GetComponent<VisitorSelector>();
            if (v == null)
                throw new System.Exception($"On {vis.name} absent {typeof(VisitorSelector).Name}");

            if (vis.GetComponent<VisitorSelector>() is T)
                return vis;
        }

        return null;    
    }

    public List<GameObject> GetAllVisitors()
    {
        return visitors;
    }
}