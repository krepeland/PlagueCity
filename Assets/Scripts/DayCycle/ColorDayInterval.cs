using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDayInterval : MonoBehaviour, ITickable
{
    public List<Color> HourColors = new List<Color>(24);
    int HourNow;
    public float T;
    public float deltaTime;

    public static ColorDayInterval singleton;

    void Awake() {
        singleton = this;
    }

    void Start()
    {
        Ticker.AddTickable(this);
        Tick(0);
        deltaTime = Ticker.singleton.SecondsPerTick;
    }

    public void Tick(int luck)
    {
        HourNow = Ticker.singleton.Hour;
        T = 0;
    }

    public void Update()
    {
        T += Time.deltaTime;
        var next = (HourNow + 1) % 24;
        GetComponent<SpriteRenderer>().color = Color.Lerp(HourColors[HourNow], HourColors[next], T / deltaTime);
    }

}
