using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonManipulator : MonoBehaviour, IButton
{
    public List<ButtonAction> actions = new List<ButtonAction>();

    private bool isPressed = false;
    private float cameraSpeed = 0;

    public void ButtonAiming()
    {
        var cursoure = ItemsSelector.singleton.GetCursoreTarget();
        foreach (var action in actions)
        {
            if (action != null)
                action.OnAiming(this, cursoure);
        }

    }

    public void ButtonDown()
    {
        var cursoure = ItemsSelector.singleton.GetCursoreTarget();

        foreach (var action in actions)
        {
            if (action != null)
                action.OnDownButton(this, cursoure);
        }

        LockPositionCamera();

        isPressed = true;
    }

    private void ButtonUp()
    {
        var cursoure = ItemsSelector.singleton.GetCursoreTarget();

        foreach (var action in actions)
        {
            if (action != null)
                action.OnUpButton(this, cursoure);
        }

        FreeLockPositionCamera();

        isPressed = false;
    }

    private void LockPositionCamera()
    {
        var cameraController = ItemsSelector.singleton.CameraController;



        cameraSpeed = cameraController.MoveSpeed;
        cameraController.MoveSpeed = 0;
    }

    private void FreeLockPositionCamera()
    {
        var cameraController = ItemsSelector.singleton.CameraController;

        cameraController.MoveSpeed = cameraSpeed;
    }

    void Update()
    {

        if (isPressed)
        {
            var cursoure = ItemsSelector.singleton.GetCursoreTarget();

            foreach (var action in actions)
            {
                if (action != null)
                    action.OnActiveButton(this, cursoure);
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
                ButtonUp();
        }
    }

    void Awake()
    {
        List<ButtonAction> newAction = new List<ButtonAction>();

        foreach (var action in actions)
        {
            if (action != null)
                newAction.Add(action);
        }

        actions = newAction;
    }
}

public interface IButton
{
    void ButtonDown();
}
