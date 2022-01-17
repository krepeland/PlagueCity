using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private GameInput inputActions;
    [SerializeField] private Camera _camera;
    [SerializeField] private GraphicRaycasterUI raycasterUI;


    public float MoveSpeed = 1;

    public Vector3 CameraTargetPos;
    private float CameraTargetSize;

    private Vector3 dragStartPosition;

    public Vector2 Input_MousePosition;
    public bool IsMouseOnUI;
    public Vector2 Input_Move;
    public bool Input_MouseLeft;
    public bool Input_MouseRight;
    public bool Input_MouseMiddle;

    public float Radius = 4;

    float angle;

    public static CameraController singleton;

    private void Awake()
    {
        singleton = this;
        inputActions = new GameInput();

        inputActions.Gameplay.Move.performed += ctx => Input_Move = ctx.ReadValue<Vector2>();
        inputActions.Gameplay.Move.canceled += ctx => Input_Move = Vector2.zero;

        inputActions.Gameplay.Mouse_Delta.performed += ctx => MouseMoved(ctx.ReadValue<Vector2>());
        inputActions.Gameplay.Mouse_Delta.canceled += ctx => MouseMoved(Vector2.zero);

        inputActions.Gameplay.Mouse_Left.started += ctx => Input_MousePressed(0);
        inputActions.Gameplay.Mouse_Left.canceled += ctx => Input_MouseCanceled(0);

        inputActions.Gameplay.Mouse_Right.started += ctx => Input_MousePressed(1);
        inputActions.Gameplay.Mouse_Right.canceled += ctx => Input_MouseCanceled(1);

        inputActions.Gameplay.Mouse_Middle.started += ctx => Input_MousePressed(2);
        inputActions.Gameplay.Mouse_Middle.canceled += ctx => Input_MouseCanceled(2);

        inputActions.Gameplay.Mouse_Position.performed += ctx =>
        {
            Input_MousePosition = ctx.ReadValue<Vector2>();
            if (!Input_MouseLeft)
                IsMouseOnUI = raycasterUI.RayCastToResults(Input_MousePosition);
        };

        inputActions.Gameplay.Mouse_Scroll.performed += ctx => Input_Scroll(ctx.ReadValue<Vector2>());
        inputActions.Gameplay.Mouse_Scroll.canceled += ctx => Input_Scroll(Vector2.zero);

        inputActions.Gameplay.Pause.started += ctx => UIManager.singleton.SwitchMenu();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }


    void Start()
    {
        CameraTargetPos = transform.position;
        CameraTargetSize = _camera.orthographicSize;
        //angle = Random.Range(0f, 360f);
        transform.parent.rotation = Quaternion.Euler(0, 0, angle);
    }

    void MouseMoved(Vector2 offset)
    {
        if (Input_MouseLeft && !IsMouseOnUI)
        {
            CameraTargetPos = transform.position - _camera.ScreenToWorldPoint(Input_MousePosition) + dragStartPosition;
        }
    }

    void Input_Scroll(Vector2 value)
    {
        //Debug.Log(value);

        if (GameManager.singleton.IsPaused)
            return;

        CameraTargetSize -= value.y * 0.01f;
        if (CameraTargetSize > 10)
            CameraTargetSize = 10;
        if (CameraTargetSize < 1)
            CameraTargetSize = 1;
    }

    /* Mouse buttons:
     * 0 - Left
     * 1 - Right
     * 2 - Middle
     */

    void Input_MousePressed(int mouseButton)
    {
        switch (mouseButton)
        {
            case 0:
                Input_MouseLeft = true;
                dragStartPosition = _camera.ScreenToWorldPoint(Input_MousePosition);

                break;

            case 1:
                Input_MouseRight = true;

                break;

            case 2:
                Input_MouseMiddle = true;

                break;
        }
    }

    void Input_MouseCanceled(int mouseButton)
    {
        switch (mouseButton)
        {
            case 0:
                Input_MouseLeft = false;
                break;

            case 1:
                Input_MouseRight = false;

                break;

            case 2:
                Input_MouseMiddle = false;

                break;
        }
    }

    void Update()
    {
        if (GameManager.singleton.IsPaused)
        {
            CameraTargetPos = transform.position;
            if(Input_MouseLeft)
                dragStartPosition = _camera.ScreenToWorldPoint(Input_MousePosition);
            return;
        }

        CameraTargetPos += GetRotatedVector(new Vector3(-Input_Move.x, Input_Move.y, 0)) * CameraTargetSize * Time.deltaTime * MoveSpeed;

        var pos = new Vector2(CameraTargetPos.x, CameraTargetPos.y);
        if (pos.magnitude > Radius)
        {
            pos = pos.normalized * Radius;
            CameraTargetPos = new Vector3(pos.x, pos.y, CameraTargetPos.z);
        }

        if (MoveSpeed != 0)
            transform.position = Vector3.Lerp(transform.position, CameraTargetPos, Time.deltaTime * 5);
        else {
            CameraTargetPos = transform.position;
        }

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, CameraTargetSize, Time.deltaTime * 5);
    }

    Vector3 GetRotatedVector(Vector3 vector)
    {
        var sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        var cos = Mathf.Cos(angle * Mathf.Deg2Rad);
        return new Vector3(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos, 0);
    }
}
