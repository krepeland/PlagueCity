using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorCard : GameCard
{
    public override void OnUpButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
        if ((cursourOnTarget != null && cursourOnTarget.tag == "Button") || (cursourDetected == null))
        {
            base.OnUpButton(sendner, cursourOnTarget);
            return;
        }
        if (cursourDetected != null || cursourOnTarget != null)
        {
            cursourOnTarget = cursourDetected;

            DestroyCard();
            UnitsSystem.singleton.CreateAndNavigateUnit<PlagueDoctorUnit>(cursourOnTarget);
        }
        base.OnUpButton(sendner, cursourOnTarget);

    }
}
