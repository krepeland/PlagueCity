using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlagueConfig", menuName = "ScriptableObjects/PlagueConfig", order = 5)]
public class PlagueConfig : ScriptableObject
{
    public float StartRadius = 0.5f;

    public short StartHour = 24;

    public short MaxImmunity = 50;
    public short MinImmunity = -50;

    public short StartImmunity = 15;
    public short InfectionPerHuman = 8;

    public short ImmunityDamageInfected = 1;
    public short ImmunityDamageSick = 3;

    public float InfectionDistance = 5f;

    public int IncubationHoursMin = 3;
    public int IncubationHoursMax = 12;

    public int SickHoursMin = 48;
    public int SickHoursMax = 72;

    public int SkipLuck = 12;

    public float[] HourInfectionCoefficient = new float[24] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f};

    [Header("Obstacles")]
    public float PlagueStopChance_Strong = 100;
    public float PlagueStopChance_Medium = 95;
    public float PlagueStopChance_Weak = 80;
    public float PlagueStopChance_None = 50;
}
