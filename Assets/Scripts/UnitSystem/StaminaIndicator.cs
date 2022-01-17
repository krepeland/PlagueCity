using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaIndicator : MonoBehaviour
{
    [SerializeField] Image indicator;

    private void Start()
    {
        //indicator = GetComponent<Image>();
    }

    public void SetStamina(int startStamina, int nowStamina)
    {
        if (indicator == null)
            return;

        var corrector = (1f / startStamina) * nowStamina;
        indicator.fillAmount = corrector;
    }
}
