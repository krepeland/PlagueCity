using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGameObject : ButtonAction
{
    [SerializeField] private GameObject target;

    public override void OnActiveButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
    }

    public override void OnAiming(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
    }

    public override void OnDownButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
    }

    public override void OnUpButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
        if (target == null ||(cursourOnTarget != null && !cursourOnTarget.Equals(gameObject)))
            return;

        var active = target.activeSelf ? false : true;

        target.SetActive(active);
    }
}
