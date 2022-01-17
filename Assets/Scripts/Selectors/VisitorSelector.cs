using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class VisitorSelector : MonoBehaviour
{
    protected ItemsSelector selector;
    protected GameObject target;
    public bool inicialized = false;

    public virtual void Initialize(GameObject target)
    {
        selector = ItemsSelector.singleton;
        this.target = target;
        inicialized = true;
    }
}
