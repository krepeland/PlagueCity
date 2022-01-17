using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadePoint : MonoBehaviour
{
    [SerializeField] private GameObject fence; //GridPoint

    public List<GameObject> connectors = new List<GameObject>();

    private GridPoint point;
    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        point = transform.parent.GetComponent<GridPoint>();
        var barricades = point.GetNeighboringBarricade();

        if (barricades != null)
            foreach (var bar in barricades)
                SpawnFence(transform.position, bar.transform.position, bar);

    }

    private void SpawnFence(Vector3 a, Vector3 b, GridPoint to)
    {
        var vec = (b - a).normalized;
        var position = (a + b) / 2;
        var rotation = //Quaternion.LookRotation((b - a), Vector3.up);
        //Quaternion.FromToRotation((b - a), b);
        new Quaternion(0, 1, 0, 0);

        var spawnedFence = Instantiate(fence, position, rotation);
        spawnedFence.transform.rotation = Quaternion.Euler(0, 180,
            Vector3.SignedAngle(new Vector3(1,0,0), vec, spawnedFence.transform.forward));
        spawnedFence.transform.localScale = new Vector3((b - a).magnitude, 0.1f,1);

        connectors.Add(spawnedFence);

        to.barricadePoint.connectors.Add(spawnedFence);
    }


    protected void OnDestroy()
    {
        foreach (var go in connectors)
        {
            if (go != null)
                Destroy(go);
        }

        connectors = new List<GameObject>();
    }
}
