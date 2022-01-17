using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public int TutorialStage = -1;

    float T = 0;

    public List<GameObject> TutorialObjects;
    public List<Text> TutorialTexts;

    public HashSet<int> values = new HashSet<int>();

    private void Start()
    {
        if (TutorialStage == -1)
        {
            TutorialStage = 0;
            TutorialObjects[0].SetActive(true);
            T = 0;
        }
    }

    void Update()
    {
        T += Time.deltaTime;

        switch (TutorialStage) {
            case 0:
                TutorialStage = 1;
                break;

            case 1:
                Ticker.singleton.SecondsPerTick = 100000000;
                var e = CameraController.singleton.Input_Move;
                if (e.y > 0) values.Add(0);
                if (e.x < 0) values.Add(1);
                if (e.y < 0) values.Add(2);
                if (e.x > 0) values.Add(3);

                if (CameraController.singleton.Input_MouseLeft)
                    values.Add(4);

                var sum = values.Count;
                if (sum == 5) Continue();
                if (values.Contains(4)) sum -= 1;

                TutorialTexts[0].text = $"	Tutorial\n" +
                    $"  Welcome to PLAGUE CITY!\n" +
                    $"{(sum==4 ? $"<color=#00b700ff>{'\u2713'}</color>" : "-")}USE <color=\"{(values.Contains(0) ? "green" : "red")}\">W</color>,<color=\"{(values.Contains(1) ? "green" : "red")}\">A</color>,<color=\"{(values.Contains(2) ? "green" : "red")}\">S</color>,<color=\"{(values.Contains(3) ? "green" : "red")}\">D</color> to move the camera\n" +
                    $"{(values.Contains(4) ? $"<color=#00b700ff>{'\u2713'}</color>" : "-")}USE <color=\"{(values.Contains(4) ? "green" : "red")}\">LMB</color> to move the camera";
                break;

            case 2:
                Ticker.singleton.SecondsPerTick = 100000000;
                values = new HashSet<int>();
                TutorialObjects[0].SetActive(false);
                //TutorialObjects[1].SetActive(true);
                Ticker.singleton.Tick();
                TutorialStage = 3;
                break;

            case 3:
                if (TutorialObjects[1].transform.childCount == 0)
                {
                    TutorialStage = 4;
                    Ticker.singleton.Tick();
                    Ticker.singleton.SetSpeedIndex(1);
                }
                break;

            case 4:
                if (Ticker.singleton.Ticks >= 5) {
                    TutorialStage = 5;
                    TutorialObjects[2].SetActive(true);
                    TutorialTexts[1].text = "	 Tutorial\n-Send the unit to the infected(red) house.\n\nTo do this, hover over his card in the bottom panel and drag, choosing a target for it.";
                }
                break;

            case 5:
                Ticker.singleton.SecondsPerTick = 100000000;
                if (TutorialObjects[4].transform.childCount < 6) {
                    Continue();
                }
                break;

            case 6:
                TutorialObjects[3].SetActive(true);
                TutorialObjects[2].SetActive(false);
                Ticker.singleton.SetSpeedIndex(0);
                break;

            case 7:
                TutorialObjects[3].SetActive(false);
                TutorialStage = 8;
                enabled = false;
                break;
        }
    }

    public void Continue() {
        TutorialStage += 1;
    }

    public void Skip() {
        TutorialObjects[0].SetActive(false);
        TutorialObjects[2].SetActive(false);
        TutorialObjects[3].SetActive(false);
        TutorialStage = 8;
        Ticker.singleton.SetSpeedIndex(0);
        enabled = false;
    }
}
