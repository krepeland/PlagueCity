using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EventWeightAndRepeatChange {
    [SerializeField]
    public bool Now;
    [SerializeField]
    public bool NextHour;
    [SerializeField]
    public RandomEvent Event;
    [SerializeField]
    public int WeightDelta;
    [SerializeField]
    public int RepeatDelta;
}

[Serializable]
public struct ActionStruct
{
    [SerializeField]
    public bool IsAddToText;
    [SerializeField]
    public RE_Action RE_Action;
    [SerializeField]
    public float value;
}

[Serializable]
public struct RequiredConditionStruct
{
    [SerializeField]
    public bool IsAddToText;
    [SerializeField]
    public RE_Condition RE_Condition;
    [SerializeField]
    public float value;
}

[Serializable]
public struct EventAnswer
{
    public bool CloseCardOnPress;
    public string AnswerText;
    [TextArea()]
    public string AnswerResultText;
    public Sprite AnswerResultSprite;
    [SerializeField]
    public List<ActionStruct> Actions;
    [SerializeField]
    public List<RequiredConditionStruct> RequiredConditions;
    [SerializeField]
    public List<EventWeightAndRepeatChange> ChangeOtherEventsWeightAndRepeats;
}

[CreateAssetMenu(fileName = "RandomEvent", menuName = "RandomEvents/RandomEvent", order = 1)]
public class RandomEvent : ScriptableObject
{
    [Header("Core data")]

    [Range(0, 1000)]
    public int Weight = 1;
    [Range(3, 18)]
    public int MinLuck = 3;
    [Range(3, 18)]
    public int MaxLuck = 18;

    public int MinEventsDelay = 0;

    /// <summary>
    /// Place -1 to infinity repeats
    /// </summary>
    public int RepeatsCount = -1;

    [Header("Content")]
    public string Title;
    public Sprite MainSprite;
    [TextArea()]
    public string Desription;

    public List<ActionStruct> Standart_Actions = new List<ActionStruct>();
    public List<EventWeightAndRepeatChange> Standart_ChangeOtherEventsWeight = new List<EventWeightAndRepeatChange>();

    public List<EventAnswer> Answers = new List<EventAnswer>();
}
