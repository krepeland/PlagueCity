using Assets.Scripts.Classes.SpecialBuild;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerBuild : MonoBehaviour
{
    public bool lockDestroy = false;

    Rigidbody2D rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        List<Collider2D> c2D = new List<Collider2D>();
        if (GetComponent<BoxCollider2D>().GetContacts(c2D) > 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        var list = CityGenerator.singleton.buildsWorld;
        Build build = null;
        foreach (var b in list)
            if (b.GO.Equals(gameObject))
                build = b;

        if (build != null)
        {
            //Debug.Log("Break Build");
            list.Remove(build);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lockDestroy)
            return;

        CleanerBuild c;
        if (collision.gameObject.TryGetComponent<CleanerBuild>(out c))
        {
            c.lockDestroy = true;
            Destroy(gameObject);
        }
    }
}
