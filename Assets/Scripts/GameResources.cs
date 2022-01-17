using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    public static GameResources singleton;

    [Header("Barricade")]
    public GameObject barricadePoint;
    public GameObject barricadeLenght;

    [Header("Navigate line")]

    public GameObject navPoint;
    public GameObject navLenght;

    public void Awake()
    {
        singleton = this;
    }

}
