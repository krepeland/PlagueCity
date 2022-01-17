using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour, ITickable
{
    ResourcesSystem resourcesSystem;
    public float MealForHumanPerTick;

    public List<(int, float)> TimedMeals = new List<(int, float)>();

    [SerializeField] private float mealPerTick = 0;

    public static FoodManager singleton;

    public int HoursToHunger;

    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        Ticker.AddTickable(this);
        resourcesSystem = ResourcesSystem.singleton;
        var resources = resourcesSystem.GetResourses();
        HoursToHunger = resourcesSystem.config.HungerHours;
        //UpdateUIFoodCount(resources.CountMeal);
    }

    public void AddTimedMeals(float mealPerTick, int hours) {
        TimedMeals.Add((hours, mealPerTick));
        ChangeMealPerTick(mealPerTick);
    }

    public void UpdateMealFromPopulation(int oldCount, int newCount) {
        var delta = oldCount - newCount;
        ChangeMealPerTick(delta * MealForHumanPerTick);
    }

    public void ChangeMealPerTick(float delta)
    {
        Debug.Log("MealPerTick: " + mealPerTick + "; delta = " + delta);
        mealPerTick += delta;
        mealPerTick = Mathf.Round(mealPerTick * 100) * 0.01f;
    }

    void UpdateTimedMeals() {
        for (var i = 0; i < TimedMeals.Count; i++) {
            var e = TimedMeals[i];
            e.Item1 -= 1;
            if (e.Item1 <= 0)
            {
                ChangeMealPerTick(-e.Item2);
                TimedMeals.RemoveAt(i);
                i--;
            }
            else {
                TimedMeals[i] = e;
            }
        }
    }

    public void Tick(int luck)
    {
        var resources = resourcesSystem.GetResourses();
        //Debug.Log("Meal: " + resources.CountMeal + "; HoursToHunger = " + HoursToHunger + "; MealPerTick: " + mealPerTick);

        if (resources.CountMeal >= -mealPerTick)
        {
            AddFood(mealPerTick, false);
            resources = resourcesSystem.GetResourses();
            if (resources.CountMeal > resourcesSystem.config.MaxMeal) {
                resources.CountMeal = resourcesSystem.config.MaxMeal;
            }

            if (HoursToHunger < 0) HoursToHunger = 0;
            HoursToHunger += 2;
            if (HoursToHunger > resourcesSystem.config.HungerHours) HoursToHunger = resourcesSystem.config.HungerHours;
        }
        else
        {
            if (-mealPerTick + 1 >= 0 && Random.Range(0, -mealPerTick + 1) > resources.CountMeal)
            {
                HoursToHunger -= 1;
            }
            else {
                HoursToHunger += 1;
                if (HoursToHunger > resourcesSystem.config.HungerHours) HoursToHunger = resourcesSystem.config.HungerHours;
            }
            resources.CountMeal = 0;
        }

        if (HoursToHunger <= 0) {
            foreach (var house in CityGenerator.singleton.buildsWorld)
                if (Random.Range(0, resourcesSystem.config.HungerHoursMax + 1) < -HoursToHunger)
                {
                    house.GO.GetComponent<House>().Kill(1);
                }
        }

        UpdateUIFoodCount(resources.CountMeal);

        resourcesSystem.SetResources(resources);

        UpdateTimedMeals();
    }

    public void UpdateUIFoodCount(float count) {
        UIManager.singleton.SetNewFoodCount((int)count);
        UIManager.singleton.SetNewDeltaFoodCount(mealPerTick);
    }

    public void AddFood(float count, bool isShown)
    {
        var resources = resourcesSystem.GetResourses();
        resources.CountMeal += count;
        if (resources.CountMeal < 0) resources.CountMeal = 0;

        UpdateUIFoodCount(resources.CountMeal);
        resourcesSystem.SetResources(resources);
    }
}
