using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Assets.Scripts.Classes.Generators.RoadsGenerator;

public static class AimCursour
{
    private static Vector3 offSetCard = new Vector3(-2, -1, 0);
    private static GameObject cursour;

    public static void UseAimCursour(Camera camera, GameObject prefabCursour)
    {
        if (prefabCursour == null)
            throw new System.Exception($"{typeof(AimCursour).Name} take prefabCursour as null");

        var camPos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        cursour = GameObject.Instantiate(prefabCursour, ItemsSelector.singleton.transform);
        cursour.transform.position = camPos;
    }

    public static void DisableAimCursour()
    {
        GameObject.Destroy(cursour);
    }

    public static bool TryUseAimCursour(GameObject cursourOnTarget, GameCard card, Camera camera, out Vector3 cardPosition, out GameObject cursourObjectDetected)
    {
        if (cursour == null)
        {
            cursourObjectDetected = null;
            cardPosition = default;
            return false;
        }

        var camPos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        camPos.z = 0;

        if (cursourOnTarget != null && cursourOnTarget.tag == "HandCards")
        {
            cardPosition = default;
            cursourObjectDetected = null;
            cursour.SetActive(false);
            return false;
        }
        else
        {
            cursour.SetActive(true);
            var objects = Physics2D.CircleCastAll(camPos, 0.5f, Vector2.up, 0.01f);

            if (objects.Length > 0)
            {
                AimCursourInfo oldAimCursourInfo = null;
                if (FindAimInfoToCard(card, camPos, objects, out oldAimCursourInfo))
                {
                    cardPosition = oldAimCursourInfo.transform.position + offSetCard;
                    cursour.transform.position = Vector3.Lerp(cursour.transform.position,
                        oldAimCursourInfo.transform.position, Time.deltaTime * 10f);

                    cursourObjectDetected = oldAimCursourInfo.gameObject;
                    return true;
                }
            }
        }

        cursour.transform.position = Vector3.Lerp(cursour.transform.position, camPos, Time.deltaTime * 10f);

        cardPosition = default;
        cursourObjectDetected = null;

        return false;
    }

    private static bool FindAimInfoToCard(GameCard card, Vector3 camPos, RaycastHit2D[] objects, out AimCursourInfo aimCursour)
    {
        AimCursourInfo oldAimCursourInfo = null;
        foreach (var obj in objects)
        {
            var aimCursourInfo = obj.collider.gameObject.GetComponent<AimCursourInfo>();
            if (aimCursourInfo != null)
            {
                if (oldAimCursourInfo == null)
                {
                    oldAimCursourInfo = aimCursourInfo;
                }
                else
                {
                    var oldMagnit = (camPos - oldAimCursourInfo.transform.position).magnitude;
                    var newMagnit = (camPos - aimCursourInfo.transform.position).magnitude;

                    if (oldMagnit > newMagnit)
                    {
                        Debug.Log(aimCursourInfo.HasGameType(card));

                        if (aimCursourInfo.HasGameType(card))
                        {
                            oldAimCursourInfo = aimCursourInfo;
                        }
                    }
                }
            }
        }

        if (oldAimCursourInfo != null)
        {
            aimCursour = oldAimCursourInfo;
            return true;
        }

        aimCursour = null;
        return false;
    }
}

public static class DrawerWay
{

    public static List<GameObject> allLine = new List<GameObject>();
    public static void Draw(Vector3 end)
    {
        
        var way = RoadFind.FindZeroRoadTo(end);

        var pointPref = GameResources.singleton.navPoint;
        var linePref = GameResources.singleton.navLenght;

        for (var i = 0; i < way.Count; i++)
        {
            var pointObj = GameObject.Instantiate(pointPref, way[i], Quaternion.identity);
            allLine.Add(pointObj);

            if (i > 0)
            {
                var fence = SpawnFence(way[i], way[i - 1], linePref);
                //fence.transform.SetParent(pointObj.transform);
                allLine.Add(fence);
            }
        }
    }

    public static void Draw(Vector3 start, Vector3 end)
    {

        var way = RoadFind.FindRoadTo(start, end);// RoadFind.FindZeroRoadTo(end);
        
        var pointPref = GameResources.singleton.navPoint;
        var linePref = GameResources.singleton.navLenght;

        for (var i = 0; i < way.Count; i++)
        {
            var pointObj = GameObject.Instantiate(pointPref, way[i], Quaternion.identity);
            allLine.Add(pointObj);

            if (i > 0)
            {
                var fence = SpawnFence(way[i], way[i - 1], linePref);
                //fence.transform.SetParent(pointObj.transform);
                allLine.Add(fence);
            }
        }
    }

    public static void ClearNavLine()
    {
        foreach (var line in allLine)
        {
            GameObject.Destroy(line);
        }

        allLine = new List<GameObject>();
    }

    private static GameObject SpawnFence(Vector3 a, Vector3 b, GameObject pref)
    {
        var vec = (b - a).normalized;
        var position = (a + b) / 2;
        var rotation = //Quaternion.LookRotation((b - a), Vector3.up);
        //Quaternion.FromToRotation((b - a), b);
        new Quaternion(0, 1, 0, 0);

        var spawnedFence = GameObject.Instantiate(pref, position, rotation);
        spawnedFence.transform.rotation = Quaternion.Euler(0, 180,
            Vector3.SignedAngle(new Vector3(1, 0, 0), vec, spawnedFence.transform.forward));
        spawnedFence.transform.localScale = new Vector3((b - a).magnitude, 0.1f, 1);

        return spawnedFence;
    }

}
