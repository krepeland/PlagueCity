using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemsSelector : MonoBehaviour
{
    public static ItemsSelector singleton;

    [SerializeField] private GameObject uiSelector;

    [SerializeField] private bool isPlagued;
    [SerializeField] private bool isDead;
    [SerializeField] private bool isFire;

    [SerializeField] private Camera Camera = null;
    public CameraController CameraController { get; private set; }

    [Header("VisitorsSystem")]
    [SerializeField] private Transform content;

    private List<GameObject> initializedVisitors = new List<GameObject>();

    private GameObject cursoureTarget;
    private GameObject build;

    private GameObject pressedButton;

    private void Awake()
    {
        singleton = this;

        if (Camera == null)
            throw new System.Exception("UnitsSystem: Camera is null");

    }
    private void Start()
    {
        CameraController = Camera.GetComponent<CameraController>();
    }
    public void EnableSelector(GameObject build)
    {
        uiSelector.SetActive(true);
        this.build = build;
    }

    public void DisableSelector()
    {
        uiSelector.SetActive(false);
    }

    public GameObject GetCursoreTarget() => cursoureTarget;

    private void Update()
    {
        cursoureTarget = SeeTarget();
        var target = SelectTarget();

        if (cursoureTarget != null && cursoureTarget.tag == "Button")
        {
            var button = cursoureTarget.GetComponent<ButtonManipulator>();
            if (button != null)
                button.ButtonAiming();
        }

        if (target != null && target.tag == "Button")
        {
            var button = target.GetComponent<ButtonManipulator>();
            if (button != null)
            {
                button.ButtonDown();
                pressedButton = target;
            }
        }

        //if (target != null)
        //{
        //    var visitors = FindVisitors(target);
        //    if (visitors == null)
        //        return;

        //    InitializeVisitors(visitors, target);
        //    EnableSelector(target);
        //}
    }

    private GameObject SeeTarget()
    {
        GameObject res = null;

        {
            RaycastHit2D hit;
            var camPos = Camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            hit = Physics2D.Raycast(camPos, Camera.transform.forward * 10);

            if (hit.transform != null)
            {
                res = hit.transform.gameObject;

                if (CameraController.IsMouseOnUI && res.tag != "Button")
                    return null;

            }
        }

        return res;
    }

    private GameObject SelectTarget()
    {
        GameObject res = null;

        //if (!CameraController.IsMouseOnUI)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit2D[] hits;
            var camPos = Camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Debug.DrawRay(camPos, Camera.transform.forward * 10);

            hits = Physics2D.RaycastAll(camPos, Camera.transform.forward * 10);

            if (hits.Length > 0)
            {
                GameObject old = hits[0].transform.gameObject;
                foreach (var hit in hits)
                {
                    //if (hit.transform.gameObject.tag == "Button" 
                    //    & ((pressedButton != null && !pressedButton.Equals(hit.transform.gameObject)) || pressedButton == null))
                    if (old.transform.position.z < hit.transform.position.z)
                        old = hit.transform.gameObject;
                }

                return old;
            }
        }

        return res;
    }

    private void ClearOldInfo()
    {
        if (initializedVisitors.Count > 0)
            foreach (var obj in initializedVisitors)
            {
                Destroy(obj);
            }

        initializedVisitors = new List<GameObject>();
    }

    private void InitializeVisitors(VisitorsInfo visitors, GameObject target)
    {
        var vis = visitors.GetAllVisitors();

        foreach (var v in vis)
        {
            var obj = Instantiate(v, content);
            obj.GetComponent<VisitorSelector>().Initialize(target);

            initializedVisitors.Add(obj);
        }
    }

    private VisitorsInfo FindVisitors(GameObject target)
    {
        return target.GetComponent<VisitorsInfo>();
    }
}
