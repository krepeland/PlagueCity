using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSystem : MonoBehaviour
{
    public ResourceConfig config;

    [SerializeField] private int countKnights;
    [SerializeField] private int countResidents;
    [SerializeField] private int countPlagueDoctor;

    [SerializeField] private float countGold;
    [SerializeField] private float countMeal;
    [SerializeField] private float countReputation;

    private MatchResources resources;

    public static ResourcesSystem singleton;

    private void Awake()
    {
        singleton = this;
        resources = new MatchResources(
            countKnights, countResidents, countPlagueDoctor,
            countGold, countMeal, countReputation);
    }

    void Start()
    {
        SetDataFromConfig(config);
    }

    void SetDataFromConfig(ResourceConfig config)
    {
        SetResources(new MatchResources(
            countKnights, countResidents, countPlagueDoctor,
            config.StartGold, config.StartMeal, config.StartReputation));

        FoodManager.singleton.MealForHumanPerTick = config.MealForHumanPerTick;
        Plague.singleton.MoneyPerHuman = config.GoldFromHumanPerTick;
        Plague.singleton.MoneyPerTopHuman = config.GoldFromTopHumanPerTick;
    }


    void Update()
    {

    }

    public MatchResources GetResourses()
    {
        return resources;
    }

    public int GetResoursesUnitCount<T>() where T : UnitBase
    {
        if (typeof(T).Name.Equals(typeof(KnightUnit).Name))
            return resources.CountKnights;

        if (typeof(T).Name.Equals(typeof(PlagueDoctorUnit).Name))
            return resources.CountPlagueDoctor;

        throw new System.Exception($"GetResoursesUnitCount haven't this UnitBase - {typeof(T).Name}");
    }


    public bool SetResources(MatchResources resources)
    {
        this.resources = resources;

        return true;
    }
    public void SetResoursesUnitCount<T>(int count) where T : UnitBase
    {
        if (typeof(T).Name.Equals(typeof(KnightUnit).Name))
        {
            resources.CountKnights = count;
            return;
        }
        if (typeof(T).Name.Equals(typeof(PlagueDoctorUnit).Name))
        {
            resources.CountPlagueDoctor = count;
            return;
        }

        throw new System.Exception($"GetResoursesUnitCount haven't this UnitBase - {typeof(T).Name}");
    }

}

public struct MatchResources
{
    public int CountKnights;
    public int CountResidents;
    public int CountPlagueDoctor;

    public float CountGold;
    public float CountMeal;
    public float CountReputation;
    public MatchResources(int countKnights, int countResidents, int countPlagueDoctor, float countGold, float countMeal, float countReputation)
    {
        CountResidents = countResidents;
        CountKnights = countKnights;
        CountPlagueDoctor = countPlagueDoctor;

        CountGold = countGold;
        CountMeal = countMeal;
        CountReputation = countReputation;
    }
}
