using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JarvisMarch
{
    public static Vector2[] ConvexHull(List<Vector2> poligon)
    {
        List<Vector2> hull = new List<Vector2>();

        if (poligon.Count >= 3)
        {
            int leftmost = 0;
            for (int i = 1; i < poligon.Count; ++i)
                if (poligon[i].x < poligon[leftmost].x)
                    leftmost = i;


            int current = leftmost;

            int next = 0;

            do
            {
                hull.Add(poligon[current]);
                next = (current + 1) % poligon.Count;
                for (int i = 0; i < poligon.Count; ++i)
                {
                    if (i == current || i == next)
                        continue;

                    float direction = Line_eq(poligon[i], poligon[current], poligon[next]);
                    if (direction < 0)
                        next = i;
                    if (direction == 0 && Vector2.Distance(poligon[current], (poligon[i])) > Vector2.Distance(poligon[current], (poligon[next])))
                        next = i;

                }
                current = next;

            } while (current != leftmost);
        }

        return hull.ToArray();
    }

    static float Line_eq(Vector2 A0, Vector2 A1, Vector2 A2)
    {
        return A0.x * (A1.y - A2.y) + A0.y * (A2.x - A1.x) + A1.x * A2.y - A2.x * A1.y;
    }
}
