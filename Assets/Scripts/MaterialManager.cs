using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager singleton;

    public Material HouseMaterial;
    public Texture[] HouseSprites;
    public Texture[] HouseTopSprites;

    public Material MainRoadsMaterial;
    public Material RoadsMaterial;
    public Material RiverMaterial;
    public Material OutLineMaterial;

    public Texture GetRandomHouseTexture() {
        return HouseSprites[Random.Range(0, HouseSprites.Length)];
    }

    public Texture GetRandomHouseTopTexture()
    {
        return HouseTopSprites[Random.Range(0, HouseTopSprites.Length)];
    }

    public void Awake()
    {
        singleton = this;
    }
}
