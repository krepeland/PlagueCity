using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStage
{
    Loading,
    Game,
    Win,
    Defeat
}

public class GameManager : MonoBehaviour
{
    public GameConfig GameConfig;

    public static GameManager singleton;

    public float WinPercentsMin;
    public float WinPercentsMax;

    private int pausingCount;
    public bool IsPaused;
    public HashSet<IPauseable> Pauseables = new HashSet<IPauseable>();

    public static GameStage GameStage;

    public GameObject StageWinWindow;
    public GameObject StageDefeatWindow;

    void Awake()
    {
        GameStage = GameStage.Loading;
        singleton = this;

        WinPercentsMin = GameConfig.WinPercentsMin;
        WinPercentsMax = GameConfig.WinPercentsMax;

        Pauseables = new HashSet<IPauseable>();
    }

    private void Start()
    {
        UIManager.singleton.SetBorders(WinPercentsMin, WinPercentsMax);
        pausingCount = 0;
        AddPausingCount(-1000);
        
    }

    public void AddPausingCount(int count) {
        pausingCount += count;

        if (pausingCount > 0)
        {
            SetPause(true);
        }

        if (pausingCount <= 0)
        {
            pausingCount = 0;
            SetPause(false);
        }
    }

    public void AddPausable(IPauseable pauseable) {
        Pauseables.Add(pauseable);
    }

    public bool DeletePauseable(IPauseable pauseable) {
        if (Pauseables.Contains(pauseable)) {
            Pauseables.Remove(pauseable);
            return true;
        }
        return false;
    }

    public void SetPause(bool isPaused) {
        Debug.Log("Game pause: " + isPaused);
        IsPaused = isPaused;
        foreach (var e in Pauseables) {
            e.SetPause(isPaused);
        }
    }

    public void CheckForWin(int totalAlive, int totalInfected, int totalSick, int totalDead)
    {
        switch (GameStage) {
            case GameStage.Game:
                if (totalSick + totalInfected + totalAlive == 0)
                {
                    GameStage = GameStage.Defeat;
                    ActivateStageWindow(StageDefeatWindow);

                    var e = StageDefeatWindow.transform.GetChild(1);
                        e.GetChild(1).GetComponent<Text>().text =
                        $"Death took <color=\"brown\">{totalDead}</color> people\n" +
                        $"<color=\"brown\">{0}</color> units died\n\n" +
                        $"<color=\"brown\">THIS is your legacy...</color>";
                }
                else
                {
                    if (totalSick + totalInfected == 0)
                    {
                        GameStage = GameStage.Win;
                        ActivateStageWindow(StageWinWindow);

                        StageWinWindow.transform.GetChild(1).GetChild(1).GetComponent<Text>().text =
                            $"Saved <color=\"green\">{totalAlive}</color> people\n" +
                            $"Death took <color=\"brown\">{totalDead}</color> people\n" +
                            $"<color=\"brown\">{0}</color> units died";
                    }
                }
                break;
        }
    }

    public void ActivateStageWindow(GameObject stageWindow) {
        stageWindow.SetActive(true);
        AddPausingCount(1);
    }
}
