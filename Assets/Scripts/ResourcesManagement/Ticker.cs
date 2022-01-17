using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Ticker : MonoBehaviour, IPauseable
{
    public static Ticker singleton = null;
    public static HashSet<ITickable> Tickables;
    public static HashSet<Tuple<ITickable, GameObject>> TickablesToDelete;

    public static bool IsPaused;
    public static int Luck;

    public static int Luck1;
    public static int Luck2;
    public static int Luck3;
    public static int SpecialLuck;

    public List<Button> SpeedButtons = new List<Button>();
    public List<float> SecondsPerTickNormal = new List<float>() { 4.5f, 2.25f, 1f };
    public int SpeedNow = 0;

    public float SecondsPerTick = 4.5f;
    public float TickTime;

    public long Ticks = 0;
    public long Day = 0;
    public int Hour = 0;

    public Transform TimerUIObject;

    void Awake()
    {
        singleton = this;
        IsPaused = false;
        Tickables = new HashSet<ITickable>();
        TickablesToDelete = new HashSet<Tuple<ITickable, GameObject>>();
    }

    void Start() {
        UIManager.singleton.SetDayNumber(Day);
        UIManager.singleton.SetHourText(Hour);

        GameManager.singleton.AddPausable(this);

        SwitchSelectedSpeedButton();
    }

    public void SetSpeedIndex(int index) {
        var oldSpeedValue = SecondsPerTick;
        SpeedNow = index;
        SecondsPerTick = SecondsPerTickNormal[index];
        ColorDayInterval.singleton.deltaTime = SecondsPerTick;

        var c = SecondsPerTick / oldSpeedValue;
        TickTime *= c;
        ColorDayInterval.singleton.T *= c;

        SwitchSelectedSpeedButton();
    }

    void SwitchSelectedSpeedButton() {
        for (var i = 0; i < SpeedButtons.Count; i++) {
            SpeedButtons[i].interactable = (i != SpeedNow);
        }
    }

    void Update()
    {
        if (IsPaused)
        {
            return;
        }

        TickTime += Time.deltaTime;
        if (TickTime > SecondsPerTick)
        {
            TickTime -= SecondsPerTick;
            Tick();
        }
        TimerUIObject.transform.rotation = Quaternion.Euler(0, 0, Hour * 15 + (TickTime/SecondsPerTick*15));
    }

    public static bool AddTickable(ITickable tickable)
    {
        if (!Tickables.Contains(tickable))
        {
            Tickables.Add(tickable);
            return true;
        }
        return false;
    }

    public static bool TryDeleteTickable(ITickable tickable, GameObject objectToDestroy = null)
    {
        if (Tickables.Contains(tickable))
        {
            TickablesToDelete.Add(new Tuple<ITickable, GameObject>(tickable, objectToDestroy));
            return true;
        }
        return false;
    }

    void ProcessDeleteTickables()
    {
        foreach (var e in TickablesToDelete)
        {
            if (e.Item1 != null)
                Tickables.Remove(e.Item1);

            if (e.Item2 != null)
                Destroy(e.Item2);
        }
        TickablesToDelete = new HashSet<Tuple<ITickable, GameObject>>();
    }

    public void Tick()
    {
        ProcessDeleteTickables();
        Ticks++;
        if (++Hour >= 24)
        {
            Hour = 0;
            Day++;
            Luck2 = UnityEngine.Random.Range(1, 7);

            if (Day % 3 == 0)
            {
                Luck3 = UnityEngine.Random.Range(1, 7);
            }

            UIManager.singleton.SetDayNumber(Day);
        }
        UIManager.singleton.SetHourText(Hour);

        if (Ticks % 3 == 0)
        {
            Luck1 = UnityEngine.Random.Range(1, 7);
        }

        Luck = Luck1 + Luck2 + Luck3 + SpecialLuck;

        if (Luck < 3) Luck = 3;
        if (Luck > 18) Luck = 18;

        Debug.Log($"Tick: {Ticks}; Luck: {Luck} : {Luck1} + {Luck2} + {Luck3}");
        foreach (var tickable in Tickables)
        {
            if (tickable != null)
                tickable.Tick(Luck);
        }
    }

    public void SetPause(bool isPaused)
    {
        IsPaused = isPaused;
    }
}
