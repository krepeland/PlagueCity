using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RE_Condition
{
    None,
    Gold,
    Food,

}

public static class RE_ConditionManager
{
    public static (bool, string) GetConditionInfo(RE_Condition condition, float value) {
        switch (condition) {
            case RE_Condition.Gold:
                return (ResourcesSystem.singleton.GetResourses().CountGold >= value, $"-{value} gold");

            case RE_Condition.Food:
                return (ResourcesSystem.singleton.GetResourses().CountMeal >= value, $"-{value} food");
        }
        return (true, "");
    }
}
