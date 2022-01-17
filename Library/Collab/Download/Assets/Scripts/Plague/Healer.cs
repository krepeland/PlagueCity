using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour, ITickable
{
    public short Ticks;
    public float Radius;
    public short ImmunityPerTick;
    public short HealPeoplePerTick;

    void Init(HealerConfig config) {
        Ticks = config.HealingTicksCount;
        Radius = config.HealingRadius;
        ImmunityPerTick = config.ImmunityPerTick;
        HealPeoplePerTick = config.HealPeoplePerTick;
    }

    void Start()
    {
        Ticker.AddTickable(this);
    }

    public void Tick(int luck)
    {
        var houses = Physics2D.CircleCastAll(transform.position, Radius, Vector2.zero);
        foreach (var house in houses)
        {
            if (house.collider.tag != "House" || house.collider.gameObject == gameObject) continue;
            house.collider.GetComponent<House>().Heal(ImmunityPerTick, HealPeoplePerTick);
        }
        Ticks -= 1;
        //if (Ticks <= 0)
        //{
        //    ImmunityPerTick = 0;
        //    Radius = 0;
        //    Ticker.TryDeleteTickable(this, gameObject);
        //}
    }

    private void OnDestroy()
    {
        Ticker.TryDeleteTickable(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
