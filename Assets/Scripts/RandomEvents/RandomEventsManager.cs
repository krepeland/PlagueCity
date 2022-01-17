using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventsManager : MonoBehaviour, ITickable
{
    
    public static RandomEventsManager singleton;

    Dictionary<RandomEvent, int> Events = new Dictionary<RandomEvent, int>();
    Dictionary<RandomEvent, int> RepeatsCount = new Dictionary<RandomEvent, int>();

    public Transform RandomEventCardsContainer;
    public Transform CardPrefab;

    public int MinHoursToEvent = 7;
    public int MaxHoursToEvent = 15;

    public int TickTilNextEvent = 5;

    public RandomEvent RequiredEvent;

    public List<(RandomEvent, int)> EventsDelay = new List<(RandomEvent, int)>();
    public HashSet<RandomEvent> EventsDelayHashset = new HashSet<RandomEvent>();

    private void Awake()
    {
        singleton = this;
    }

    void Start() {

        var events = Resources.LoadAll<RandomEvent>("Events");
        foreach (var e in events) {
            Events[e] = e.Weight;
            RepeatsCount[e] = e.RepeatsCount;
        }

        Ticker.AddTickable(this);
    }

    public void Tick(int luck)
    {
        TickTilNextEvent -= 1;
        if (TickTilNextEvent <= 0)
        {
            TickTilNextEvent += Random.Range(MinHoursToEvent, MaxHoursToEvent);
            TryDoEvent();
        }

        for(var i = 0; i < EventsDelay.Count; i++)
        {
            EventsDelay[i] = (EventsDelay[i].Item1, EventsDelay[i].Item2 - 1);
            if (EventsDelay[i].Item2 <= 0)
            {
                EventsDelayHashset.Remove(EventsDelay[i].Item1);
                EventsDelay.RemoveAt(i);
                i--;
            }
        }
    }

    public bool TryDoEvent() {
        var e = GetRandomEvent();
        if (e == null) return false;
        DoEvent(e);

        return true;
    }

    public void DoEvent(RandomEvent randomEvent) {
        RepeatsCount[randomEvent] -= 1;

        if (randomEvent.MinEventsDelay > 0)
        {
            EventsDelay.Add((randomEvent, randomEvent.MinEventsDelay));
            EventsDelayHashset.Add(randomEvent);
        }

        Debug.Log(randomEvent.Title);
        var card = Instantiate(CardPrefab, RandomEventCardsContainer, false);
        card.transform.SetAsFirstSibling();
        card.GetComponent<RandomEventCard>().Init(randomEvent);
    }

    public void AddWeightAndDelta(EventWeightAndRepeatChange e) {
        RepeatsCount[e.Event] += e.RepeatDelta;

        if (e.Now) {
            DoEvent(e.Event);
        }

        if (e.NextHour)
        {
            RequiredEvent = e.Event;
            TickTilNextEvent = 1;
        }

        if (Events[e.Event] + e.WeightDelta > 0)
            Events[e.Event] = Events[e.Event] + e.WeightDelta;
        else
            Events[e.Event] = 0;
    }

    public RandomEvent GetRandomEvent() {
        if (RequiredEvent != null) {
            var e = RequiredEvent;
            RequiredEvent = null;
            return e;
        }

        var luck = Ticker.Luck;
        var sumWeight = 0;
        foreach (var e in Events) {
            if (RepeatsCount[e.Key] != 0 && e.Key.MinLuck <= luck && e.Key.MaxLuck >= luck && !EventsDelayHashset.Contains(e.Key))
            {
                if (e.Value <= 0) continue;
                sumWeight += e.Value;
            }
        }
        var randValue = Random.Range(0, sumWeight + 1);
        foreach (var e in Events)
        {
            if (RepeatsCount[e.Key] != 0 && e.Key.MinLuck <= luck && e.Key.MaxLuck >= luck && !EventsDelayHashset.Contains(e.Key))
            {
                if (e.Value <= 0) continue;

                randValue -= e.Value;
                if (randValue <= 0)
                {
                    return e.Key;
                }
            }
        }
        return null;
    }
}
