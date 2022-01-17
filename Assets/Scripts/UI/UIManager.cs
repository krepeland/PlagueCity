using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager singleton;

    public Text FoodCountText;
    public Text GoldCountText;

    public Color DeltaNegative;
    public Color DeltaPositive;

    public Text FoodDeltaText;
    public Text GoldDeltaText;

    public Text DayNumberText;
    public Text HourText;

    public Animator RightSideAnimator;
    public bool IsRightSideHidden;

    public Image CounterHealthyObject;
    public Image CounterSickObject;
    public Image CounterDeadObject;
    public Text CounterText;

    public RectTransform LeftBorderObject;
    public RectTransform RightBorderObject;

    public bool IsMenuOpened;
    public GameObject MenuObject;


    public GameObject ExitAskObject;

    public RectTransform triggerHandCard;

    private void Awake()
    {
        singleton = this;
    }

    public void SwitchMenu() {
        IsMenuOpened = !IsMenuOpened;
        GameManager.singleton.AddPausingCount(IsMenuOpened ? 1 : -1);
        MenuObject.SetActive(IsMenuOpened);

        if (IsMenuOpened) {
            MainMenu.singleton.OpenOnly("Menu");
        }
    }

    public void HideRightSide() {
        IsRightSideHidden = !IsRightSideHidden;
        RightSideAnimator.SetBool("IsHidden", IsRightSideHidden);
    }

    public void SetNewFoodCount(int count) {
        FoodCountText.text = count.ToString();
    }

    public void SetHourText(int hour)
    {
        if (hour == 0)
            HourText.text = "12 AM";
        else
        {
            if (hour == 12)
                HourText.text = "12 PM";
            else
                HourText.text = hour <= 12 ? $"{hour} AM" : $"{hour - 12} PM";
        }
    }

    public void SetNewDeltaFoodCount(float delta)
    {
        delta = ((int)(delta * 100)) * 0.01f;
        FoodDeltaText.text = (delta >= 0 ? $"+{delta.ToString()}" : $"{delta.ToString()}");
        FoodDeltaText.color = (delta >= 0 ? DeltaPositive : DeltaNegative);
    }


    public void SetNewGoldCount(int count)
    {
        GoldCountText.text = count.ToString();
    }

    public void SetNewDeltaGoldCount(float delta)
    {
        delta = ((int)(delta * 1000)) * 0.001f;
        GoldDeltaText.text = (delta >= 0 ? $"+{delta.ToString()}" : $"{delta.ToString()}");
        GoldDeltaText.color = (delta >= 0 ? DeltaPositive : DeltaNegative);
    }

    public void SetDayNumber(long day) {
        DayNumberText.text = "DAY: " + day;
    }

    public void SetCounter(int total, int sick, int dead, int healthy) {
        CounterSickObject.transform.localScale = new Vector3((sick + dead) / (float)total, 1, 1);
        CounterDeadObject.transform.localScale = new Vector3((dead) / (float)total, 1, 1);

        CounterText.text = $"HEALTHY: {healthy}\nINFECTED: {sick}\nDEAD: {dead}";
    }

    public void SetBorders(float percentsLeft, float percentsRight) {
        var width = LeftBorderObject.transform.parent.GetComponent<RectTransform>().rect.width * 0.01f;
        LeftBorderObject.anchoredPosition = new Vector2(percentsLeft * width, 0);
        RightBorderObject.anchoredPosition = new Vector2(percentsRight * width, 0);
    }
}
