using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStates : MonoBehaviour
{
    public StateBuildType states = StateBuildType.Normal;

    public StateBuildType States
    {
        get
        {
            return states;
        }
        set
        {
            if (value == StateBuildType.Fire)
                Fire();
            if (value == StateBuildType.Fix)
                Fix();

            states = value;
        }
    }

    private void Fire()
    {
        var house = GetComponent<House>();
        house.DeadPeople = house.PeopleCount;
        house.HealthyPeople = 0;
        house.InfectedPeople = new List<short>();
        house.SickPeople = new List<short>();

    }

    private void Fix()
    {
        var house = GetComponent<House>();
        house.DeadPeople += (short)house.SickPeople.Count;
        house.SickPeople = new List<short>();
        
    
    }

    void Start()
    {

    }

    void Update()
    {

    }
}

public enum StateBuildType
{
    Normal,
    Fire,
    Dead,
    Plague,
    Fix
}
