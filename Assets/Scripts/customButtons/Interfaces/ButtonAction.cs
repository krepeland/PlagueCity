using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonAction : MonoBehaviour
{
    public abstract void OnAiming(ButtonManipulator sendner, GameObject cursourOnTarget);
    public abstract void OnDownButton(ButtonManipulator sendner, GameObject cursourOnTarget);
    public abstract void OnActiveButton(ButtonManipulator sendner, GameObject cursourOnTarget);
    public abstract void OnUpButton(ButtonManipulator sendner, GameObject cursourOnTarget);
}
