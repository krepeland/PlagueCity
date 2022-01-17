using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{
    public string WindowName;
    public bool IsOpened;

    public void Appear(bool isAlt)
    {
        if (IsOpened) return;
        IsOpened = true;
        GetComponent<Animator>().SetTrigger(isAlt ? "On2" : "On1");
    }

    public void Disappear(bool isAlt)
    {
        if (!IsOpened) return;
        IsOpened = false;
        GetComponent<Animator>().SetTrigger(isAlt ? "Off2" : "Off1");
    }
}
