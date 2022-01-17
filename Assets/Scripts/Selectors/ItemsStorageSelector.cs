using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsStorageSelector : VisitorsInfo
{
    public static ItemsStorageSelector singleton;

    private void Awake()
    {
        singleton = this;
    }
}
