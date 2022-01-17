using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, ITickable
{
    public short PeopleCount = 0;
    public short HealthyPeople = 0;
    public List<short> InfectedPeople = new List<short>();
    public List<short> SickPeople = new List<short>();
    public short DeadPeople = 0;

    public float Immunity;
    Material material;

    public bool IsTopHouse;

    void Start() {
        if (transform.position.magnitude < CityGenerator.singleton.buildsConfig.DistaceTopSpawn)
            IsTopHouse = true;

        Immunity = Plague.PlagueConfig.StartImmunity;
        PeopleCount = (short)Random.Range(5, 11);
        HealthyPeople = PeopleCount;
        Plague.AddInfectedHouse(this);

        GetComponent<MeshRenderer>().material = MaterialManager.singleton.HouseMaterial;
        GetComponent<MeshRenderer>().material.SetTexture("Texture2D_c66bb91ef604482d8657f70ebb2100ec", 
            IsTopHouse ? MaterialManager.singleton.GetRandomHouseTopTexture() : MaterialManager.singleton.GetRandomHouseTexture());
        material = GetComponent<MeshRenderer>().material;
    }

    public void Tick(int luck)
    {
        if (Random.Range(0, luck) >= Plague.PlagueConfig.SkipLuck) {
            return;
        }
        Immunity = Mathf.Clamp(Immunity, Plague.PlagueConfig.MinImmunity, Plague.PlagueConfig.MaxImmunity);
        var res = (short)ProcessInHouseInfection();
        res = (short)Mathf.Clamp(res, Plague.PlagueConfig.MinImmunity, Plague.PlagueConfig.MaxImmunity);
        ProcessOuterInfection(res, luck);
    }

    public void Heal(short immunityAdd, short HealPeoplePerTick) {
        Immunity += immunityAdd;
        for (var i = 0; i < HealPeoplePerTick; i++) {
            if (SickPeople.Count > 0)
            {
                SickPeople.RemoveAt(0);
                HealthyPeople += 1;
            }
            else {
                if (InfectedPeople.Count > 0)
                {
                    InfectedPeople.RemoveAt(0);
                    HealthyPeople += 1;
                }
            }
        }
    }

    int ProcessInHouseInfection() {
        Immunity -= (InfectedPeople.Count * Plague.PlagueConfig.ImmunityDamageInfected + SickPeople.Count * Plague.PlagueConfig.ImmunityDamageSick);
        while (HealthyPeople > 0 && Immunity < -Plague.PlagueConfig.InfectionPerHuman)
        {
            HealthyPeople -= 1;
            Immunity += Plague.PlagueConfig.InfectionPerHuman;
            InfectedPeople.Add((short)Random.Range(Plague.PlagueConfig.IncubationHoursMin, Plague.PlagueConfig.IncubationHoursMax + 1));
        }

        if (Immunity < -Plague.PlagueConfig.InfectionPerHuman)
            Immunity = 0;

        for (var i = 0; i < InfectedPeople.Count;)
        {
            InfectedPeople[i]--;
            if (InfectedPeople[i] <= 0)
            {
                SickPeople.Add((short)Random.Range(Plague.PlagueConfig.SickHoursMin, Plague.PlagueConfig.SickHoursMax + 1));
                InfectedPeople.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        for (var i = 0; i < SickPeople.Count;)
        {
            SickPeople[i]--;
            if (SickPeople[i] <= 0)
            {
                DeadPeople++;
                SickPeople.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        var newColor = new Color(
            ((float)PeopleCount - DeadPeople) / PeopleCount,
            ((float)HealthyPeople + (float)InfectedPeople.Count) / PeopleCount,
            ((float)HealthyPeople + (float)InfectedPeople.Count) / PeopleCount,
            1);
        newColor += (Color.white - newColor) * 0.25f;
        material.SetColor("Color_78faa8d9a04a4668bf761ed891cd3339", newColor);

        return -(short)(SickPeople.Count * Plague.PlagueConfig.ImmunityDamageInfected);
    }

    void ProcessOuterInfection(short infection, int luck)
    {
        if (infection >= 0) return;

        infection = (short)Mathf.Ceil(infection * Plague.CoefficientNow);
        var distance = Plague.PlagueConfig.InfectionDistance;
        var houses = Physics2D.CircleCastAll(transform.position, distance, Vector2.zero);
        foreach (var house in houses) {
            if (house.collider.tag != "House" || house.collider.gameObject == gameObject) continue;

            var rayObstacles = Physics2D.RaycastAll(transform.position, house.collider.transform.position - transform.position, distance);
            var Power = 3;
            foreach (var obstacle in rayObstacles) {
                if (obstacle.collider.gameObject.layer == 7) {
                    Power = 0;
                    break;
                }
                if (obstacle.collider.gameObject.layer == 8)
                {
                    Power -= 2;
                }
                if (obstacle.collider.gameObject.layer == 9)
                {
                    Power -= 1;
                }
            }
            //luck = 3;
            switch (Power) {
                case 0:
                    //if (Plague.TryLuck(Plague.PlagueConfig.PlagueStopChance_Strong, luck, 0.625f)) continue;
                    continue;
                    break;
                case 1:
                    if (Plague.TryLuck(Plague.PlagueConfig.PlagueStopChance_Medium, luck, 0.625f)) continue;
                    break;
                case 2:
                    if (Plague.TryLuck(Plague.PlagueConfig.PlagueStopChance_Weak, luck, 0.625f)) continue;
                    break;
                case 3:
                    if (Plague.TryLuck(Plague.PlagueConfig.PlagueStopChance_None, luck, 0.625f)) continue;
                    break;
            }

            var houseScript = house.collider.gameObject.GetComponent<House>();
            houseScript.Infect(infection);
        }
    }

    public void Infect(short infection) {
        Immunity += infection;
    }

    public void ForceSickHuman() {
        if (HealthyPeople > 0) {
            HealthyPeople -= 1;
            SickPeople.Add((short)Random.Range(Plague.PlagueConfig.SickHoursMin, Plague.PlagueConfig.SickHoursMax + 1));
        }
    }

    public void Kill(int count) {

        if (count == 0) return;

        for (var i = 0; i < count; i++)
        {
            if (PeopleCount - DeadPeople == 0) return;

            var id = Random.Range(0, 3);

            switch (id) {
                case 0:
                    if (HealthyPeople > 0)
                    {
                        HealthyPeople -= 1;
                        DeadPeople += 1;
                    }
                    else {
                        count += 1;
                    }
                    break;
                case 1:
                    if (InfectedPeople.Count > 0)
                    {
                        InfectedPeople.RemoveAt(0);
                        DeadPeople += 1;
                    }
                    else
                    {
                        count += 1;
                    }
                    break;
                case 2:
                    if (SickPeople.Count > 0)
                    {
                        SickPeople.RemoveAt(0);
                        DeadPeople += 1;
                    }
                    else
                    {
                        count += 1;
                    }
                    break;
            }
        }
    }

    //void OnDrawGizmos() {
    //    Gizmos.DrawWireSphere(transform.position, Plague.PlagueConfig.InfectionDistance);
    //}
}
