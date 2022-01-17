using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceConfig", menuName = "ScriptableObjects/ResourceConfig", order = 6)]
public class ResourceConfig : ScriptableObject
{
    [Header("Gold")]
    public float StartGold = 100;
    public float GoldFromHumanPerTick = 0.001f;
    public float GoldFromTopHumanPerTick = 0.0025f;

    [Header("Food")]
    public int MaxMeal = 1000;
    public float StartMeal = 250;
    public float MealForHumanPerTick = 0.01f;
    public int HungerHours = 72;
    public int HungerHoursMax = 168;


    [Header("Reputation")]
    public float StartReputation = 5;
}
