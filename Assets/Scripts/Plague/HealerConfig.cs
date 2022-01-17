using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealerConfig", menuName = "ScriptableObjects/HealerConfig", order = 6)]
public class HealerConfig : ScriptableObject
{
    public float HealingRadius = 0.75f;
    public short HealingTicksCount = 5;
    public short ImmunityPerTick = 10;
    public short HealPeoplePerTick = 2;
}
