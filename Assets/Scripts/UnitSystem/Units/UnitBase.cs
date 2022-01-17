using Assets.Scripts.Classes.Geometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UnitBase : MonoBehaviour, ITickable
{
    public float Speed { get; private set; } = 0.1f;
    public int Healh { get; private set; } = 1;

    public GameObject cardPref;

    public int liveTime;
    private int startLiveTime;

    private List<Vector3> wayPoints = new List<Vector3>();

    private bool initialized = false;
    private bool lockAction = false;

    protected GameObject target;

    private int indexPointNow = 0;
    private long ticks;

    private StaminaIndicator sIndicator;

    void Start()
    {
        if (cardPref == null)
            throw new System.Exception("Unit doesnt cardPref");

        startLiveTime = liveTime;
        sIndicator = GetComponent<StaminaIndicator>();
    }

    public virtual void Initialize(List<Vector3> wayPoints, float speed, int healh, GameObject target)
    {
        initialized = true;

        ticks = Ticker.singleton.Ticks;
        this.Speed = speed;
        this.Healh = healh;
        this.target = target;
        this.wayPoints = wayPoints;

        Ticker.AddTickable(this);
    }

    private void FixedUpdate()
    {
        if (!initialized)
            return;

        Walk();
    }
    protected virtual bool Walk()
    {
        if (indexPointNow >= wayPoints.Count)
        {
            if (!lockAction)
                DoAction();

            lockAction = true;

            return false;
        }


        var position = wayPoints[indexPointNow];
        position.z = 0.5f;

        transform.position = Vector3.Lerp(transform.position, position, Speed);
        return true;
    }

    protected virtual void TryAttack(UnitBase unit)
    {

    }

    public void SetWalkWay(List<Vector3> way)
    {
        indexPointNow = 0;
        wayPoints = way;
        lockAction = false;
    }

    public abstract int GetCountUnitsResources();
    public abstract void SetCountUnitsResources(int count);

    public int SetDamage(int damage)
    {
        int myDamage = Healh;
        Healh -= damage;

        if (Healh <= 0)
            Dead();

        return myDamage;
    }

    protected virtual void DoAction()
    {

    }

    public void Dead()
    {
        Ticker.TryDeleteTickable(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Unit")
        {
            var unit = collision.gameObject.GetComponent<UnitBase>();
            TryAttack(unit);
        }
    }

    public void Tick(int luck)
    {
        if (indexPointNow < 100)
            indexPointNow++;

        if (liveTime > 0)
        {
            liveTime--;
            UpdateStaminaIndicator();
        }
        else
        {
            CardsSystem.singleton.AddCard(cardPref);
            Dead();
        }
    }

    private void UpdateStaminaIndicator()
    {
        if (sIndicator != null)
            sIndicator.SetStamina(startLiveTime, liveTime);
    }
}
