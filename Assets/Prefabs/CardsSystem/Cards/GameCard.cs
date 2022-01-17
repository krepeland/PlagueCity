using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameCard : ButtonAction
{
    public GameObject prefabCursous = null;

    private Vector3 startPosition;
    private float offSetOnMouse = 200f;
    private Camera Camera;

    private RectTransform rTransform;

    private bool defaultState = true;
    private bool lockDefaultState = false;

    protected GameObject cursourDetected;
    private GameObject lastCursourDetected;

    public void SetStartPosition(Vector3 startPosition)
    {
        this.startPosition = startPosition;
    }

    public override void OnActiveButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
        Vector3 camPos;
        camPos = Camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        camPos.z = rTransform.position.z;

        Vector3 cardPosition;
        GameObject cursourObjDetected;

        var triggerHand = UIManager.singleton.triggerHandCard;
        if (triggerHand == null)
            return;

        if (AimCursour.TryUseAimCursour(cursourOnTarget, this, Camera, out cardPosition, out cursourObjDetected)
            && camPos.y > triggerHand.position.y)
        {
            if (lastCursourDetected == null || lastCursourDetected != cursourObjDetected)
            {
                DrawerWay.ClearNavLine();
                DrawerWay.Draw(cursourObjDetected.transform.position);

                lastCursourDetected = cursourObjDetected;
            }

            this.cursourDetected = cursourObjDetected;
            camPos = cardPosition;

            var vec = (cardPosition - cursourDetected.transform.position).normalized * Camera.orthographicSize * 0.3f;
            camPos = cursourDetected.transform.position + vec;

            transform.position = Vector3.Lerp(transform.position, camPos, Time.deltaTime * 10f);


        }
        else
        {
            rTransform.position = Vector3.Lerp(rTransform.position, camPos, Time.deltaTime * 10f);
            this.cursourDetected = null;
            lastCursourDetected = null;
            DrawerWay.ClearNavLine();
        }

        defaultState = false;
        lockDefaultState = true;
    }

    public override void OnAiming(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
        if (lockDefaultState)
            return;

        rTransform.localPosition = Vector3.Lerp(rTransform.localPosition, startPosition + (Vector3.up * offSetOnMouse), Time.deltaTime * 10f);
        defaultState = false;
    }

    public override void OnDownButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().raycastTarget = false;

        AimCursour.UseAimCursour(Camera, prefabCursous);
    }

    public override void OnUpButton(ButtonManipulator sendner, GameObject cursourOnTarget)
    {
        DrawerWay.ClearNavLine();

        //rTransform.localPosition = startPosition;
        lockDefaultState = false;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Image>().raycastTarget = false;
        cursourDetected = null;
        AimCursour.DisableAimCursour();

        if (cursourOnTarget != null)
        {
            var card = cursourOnTarget.GetComponent<GameCard>();
            if (card != null)
                CardsSystem.singleton.SwitchCard(this, card);
        }
    }

    protected void DestroyCard()
    {
        CardsSystem.singleton.RemoveCard(this);
        AimCursour.DisableAimCursour();

    }

    private void Start()
    {
        rTransform = GetComponent<RectTransform>();
        Camera = ItemsSelector.singleton.CameraController.GetComponent<Camera>();
        startPosition = rTransform.localPosition;

        transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (defaultState && !lockDefaultState)
        {
            rTransform.localPosition =
                Vector3.Lerp(rTransform.localPosition,
                startPosition,
                Time.deltaTime * 10f);

        }

    }

    private void LateUpdate()
    {
        defaultState = true;
    }

}
