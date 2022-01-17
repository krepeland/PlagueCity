using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour, ITickable
{
    ResourcesSystem resourcesSystem;
    public float GoldFromHumanPerTick;

    [SerializeField] private float goldPerTick = 0;

    public static GoldManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        Ticker.AddTickable(this);
        resourcesSystem = ResourcesSystem.singleton;
        var resources = resourcesSystem.GetResourses();
        //UpdateUIGoldCount(resources.CountGold);
    }

    public void Tick(int luck)
    {
        AddGold(goldPerTick, false);
    }

    public void UpdateGoldFromPopulation(float newMoneys)
    {
        goldPerTick = newMoneys;
        goldPerTick = Mathf.Round(goldPerTick * 1000) * 0.001f;
    }

    public void UpdateUIGoldCount(float count)
    {
        UIManager.singleton.SetNewGoldCount((int)count);
        UIManager.singleton.SetNewDeltaGoldCount(goldPerTick);
    }

    public void AddGold(float count, bool isShown) {
        var resources = resourcesSystem.GetResourses();
        resources.CountGold += count;
        if (resources.CountGold < 0) resources.CountGold = 0;

        UpdateUIGoldCount(resources.CountGold);
        resourcesSystem.SetResources(resources);
    }
}
