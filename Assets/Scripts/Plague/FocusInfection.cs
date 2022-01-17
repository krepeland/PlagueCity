using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusInfection : MonoBehaviour, ITickable
{
    public float Radius;
    public float Ticks = 3;
    public short Power = -20;
    public int CountForceSickPerTick = 3;

    public Animator CircleAnimation;
    void Start()
    {
        Ticker.AddTickable(this);
    }

    public void Tick(int luck)
    {
        if (Ticks <= 0) return;

        var houses = Physics2D.CircleCastAll(transform.position, Radius, Vector2.zero);
        foreach (var house in houses)
        {
            if (house.collider.tag != "House" || house.collider.gameObject == gameObject) continue;
            house.collider.GetComponent<House>().Infect(Power);

            for(var i = 0; i < CountForceSickPerTick; i++)
                house.collider.GetComponent<House>().ForceSickHuman();
        }

        if (CircleAnimation != null)
        {
            CircleAnimation.transform.localScale = new Vector3(2, 2, 2) * Radius;
            CircleAnimation.SetTrigger("On");
        }

        Ticks -= 1;
        if (Ticks <= 0)
        {
            Ticker.TryDeleteTickable(this, gameObject);
        }
    }
}
