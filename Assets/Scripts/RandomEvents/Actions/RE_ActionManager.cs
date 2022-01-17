using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RE_Action
{
    None,
    Gold,
    Food,
    SpawnPlague,
    AddUnit_Knight,
    AddUnit_Doctor,

}

public static class RE_ActionManager
{
    public static string CallAction(RE_Action action, float value)
    {
        switch (action) {
            case RE_Action.Gold: {
                    GoldManager.singleton.AddGold(value, true);
                    return (value >= 0 ? "<color=\"green\">+" : "<color=\"brown\">") + $"{value.ToString()} gold</color>";
                }

            case RE_Action.Food: {
                    FoodManager.singleton.AddFood(value, true);
                    return (value >= 0 ? "<color=\"green\">+" : "<color=\"brown\">") + $"{value.ToString()} food</color>";
                }

            case RE_Action.SpawnPlague: {
                    Plague.singleton.StartPlague(value);
                    return "<color=\"brown\">New outbreak of plague</color>";
                }

            case RE_Action.AddUnit_Knight:
                {
                    for (var i = 0; i < value; i++)
                    {
                        CardsSystem.singleton.AddCard<KnightCard>();
                    }
                    return "<color=\"green\">Knight added to your hand</color>";
                }

            case RE_Action.AddUnit_Doctor:
                {
                    for (var i = 0; i < value; i++)
                    {
                        CardsSystem.singleton.AddCard<PlagueDoctorCard>();
                    }
                    return "<color=\"green\">Doctor added to your hand</color>";
                }
        }
        return "";
    }
}
