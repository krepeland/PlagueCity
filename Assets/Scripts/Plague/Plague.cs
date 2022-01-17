using Assets.Scripts.Classes.SpecialBuild;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plague : MonoBehaviour, ITickable
{
    public static Plague singleton;

    public static HashSet<ITickable> InfectedHouses;

    public static PlagueConfig PlagueConfig;

    public static float CoefficientNow = 1;

    [Header("Configs")]
    [SerializeField]
    PlagueConfig plagueConfig;

    public int TotalAlive;
    public int TotalInfected;
    public int TotalSick;
    public int TotalDead;

    public float MoneyPerHuman;
    public float MoneyPerTopHuman;

    public Transform FocusInfectionPrefab;

    public void Awake()
    {
        singleton = this;
        InfectedHouses = new HashSet<ITickable>();
        PlagueConfig = plagueConfig;
    }

    void Start()
    {
        Ticker.AddTickable(this);

        StartCoroutine(ActivePlague());

    }

    private void UpdateTotalCount() {
        var houses = CityGenerator.singleton.buildsWorld;
        var OldCount = TotalAlive + TotalInfected + TotalSick;
        var newMoneys = 0f;

        TotalAlive = 0;
        TotalInfected = 0;
        TotalSick = 0;
        TotalDead = 0;
        foreach (var house in houses) {
            var h = house.GO.GetComponent<House>();

            TotalAlive += h.HealthyPeople;
            TotalInfected += h.InfectedPeople.Count;
            TotalSick += h.SickPeople.Count;
            TotalDead += h.DeadPeople;

            newMoneys += (h.HealthyPeople + h.InfectedPeople.Count) * (h.IsTopHouse ? 
                MoneyPerTopHuman :
                MoneyPerHuman);
        }

        FoodManager.singleton.UpdateMealFromPopulation(OldCount, TotalAlive + TotalInfected + TotalSick);
        GoldManager.singleton.UpdateGoldFromPopulation(newMoneys);

        var total = TotalAlive + TotalDead + TotalInfected + TotalSick;
        UIManager.singleton.SetCounter(total, TotalSick, TotalDead, TotalAlive+TotalInfected);
    }

    public void StartPlague(float radius)
    {
        var house = GetRandomHouse();
        house.GO.GetComponent<House>().Infect(-100);
        Instantiate(FocusInfectionPrefab, house.GO.transform.position, transform.rotation).GetComponent<FocusInfection>().Radius = radius;
    }

    Build GetRandomHouse(float minRange = 2) {
        var houses = CityGenerator.singleton.GetAllHouses();
        var random = Random.Range(houses.Count - (int)(houses.Count / minRange), houses.Count);
        return houses[random];
    }

    public static bool AddInfectedHouse(House house)
    {
        if (!InfectedHouses.Contains(house))
        {
            InfectedHouses.Add(house);
            return true;
        }
        return false;
    }

    public void Tick(int luck)
    {
        CoefficientNow = PlagueConfig.HourInfectionCoefficient[Ticker.singleton.Hour];
        foreach (var house in InfectedHouses)
        {
            house.Tick(luck);
        }

        UpdateTotalCount();
        GameManager.singleton.CheckForWin(TotalAlive, TotalInfected, TotalSick, TotalDead);
    }

    public static float GetLuckedFailValue(float failValue, int luck, float luckCoefficient)
    {
        if (luck == 0) luck++;
        return failValue * (1 - luckCoefficient) + (failValue * (3 / luck) * luckCoefficient);
    }

    public static bool TryLuck(float successPercents, int luck, float luckCoefficient = 1)
    {
        return Random.Range(0f, 100f) > GetLuckedFailValue(100 - successPercents, luck, luckCoefficient);
    }

    IEnumerator ActivePlague()
    {
        yield return new WaitForSeconds(3f);
        StartPlague(plagueConfig.StartRadius);
        GameManager.GameStage = GameStage.Game;
        //Ticker.singleton.TickTime = Ticker.singleton.SecondsPerTick * plagueConfig.StartHour;
    }
}
