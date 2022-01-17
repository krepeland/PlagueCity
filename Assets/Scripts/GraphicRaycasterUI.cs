using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class GraphicRaycasterUI : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    public List<RaycastResult> Results = new List<RaycastResult>();
    public bool IsMouseOnGUI;


    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    public bool RayCastToResults(Vector2 mousePosition)
    {
        Results = new List<RaycastResult>();
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = mousePosition;

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, Results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        IsMouseOnGUI = Results.Count > 0;

        return IsMouseOnGUI;
    }
}
