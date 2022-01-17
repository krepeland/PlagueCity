using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class NavigationUnit : ButtonAction
{
    private Vector3 startPosition;
    private float offSetOnMouse = 200f;
    private Camera Camera;

    //private RectTransform rTransform;

    private bool defaultState = true;
    private bool lockDefaultState = false;

    protected GameObject cursourDetected;
    private GameObject lastCursourDetected;

    private GameCard cardPref;

    public override void OnDownButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
        AimCursour.UseAimCursour(Camera, cardPref.prefabCursous);
    }

    public override void OnActiveButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
        Vector3 camPos;
        camPos = Camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //camPos.z = rTransform.position.z;

        Vector3 cardPosition;
        GameObject cursourObjDetected;
        if (AimCursour.TryUseAimCursour(cursourOnTarget, cardPref, Camera, out cardPosition, out cursourObjDetected))
        {
            if (lastCursourDetected == null || lastCursourDetected != cursourObjDetected)
            {
                DrawerWay.ClearNavLine();
                DrawerWay.Draw(transform.position, cursourObjDetected.transform.position);

                lastCursourDetected = cursourObjDetected;
            }

            this.cursourDetected = cursourObjDetected;
            camPos = cardPosition;
           // transform.position = Vector3.Lerp(transform.position, camPos, Time.deltaTime * 10f);


        }
        else
        {
            //rTransform.position = Vector3.Lerp(rTransform.position, camPos, Time.deltaTime * 10f);
            this.cursourDetected = null;
            lastCursourDetected = null;
            DrawerWay.ClearNavLine();
        }

        defaultState = false;
        lockDefaultState = true;
    }

    public override void OnUpButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
        DrawerWay.ClearNavLine();

        lockDefaultState = false;
        //cursourDetected = null;
        AimCursour.DisableAimCursour();

        if (cursourDetected == null)
            return;

        var unitBase = gameObject.GetComponent<UnitBase>();


        UnitsSystem.singleton.NavigateUnit(unitBase, cursourDetected);

    }

    void Start()
    {
        var pref = GetComponent<UnitBase>().cardPref;
        Camera = ItemsSelector.singleton.CameraController.GetComponent<Camera>();

        if (pref != null)
            cardPref = pref.GetComponent<GameCard>();
    }

    void Update()
    {

    }

    public override void OnAiming(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
    }
}
