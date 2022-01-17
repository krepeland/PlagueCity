using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSystemSettings", menuName = "ScriptableObjects/UnitSystemSettings", order = 1)]
public class UnitSystemSettings : ScriptableObject
{
    public int plagueDoctorDoActionCountTicks = 10;
    public int knightDoActionCountTicks = 3; 
}
