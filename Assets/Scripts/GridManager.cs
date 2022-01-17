using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager singleton;

    private void Awake()
    {
        singleton = this;
    }

}
