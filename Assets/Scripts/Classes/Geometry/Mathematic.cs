using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes.Geometry
{
    public static class Mathematic
    {
        public static Vector2 RotateVector(float angle, Vector2 vector)
        {
            angle = angle * Mathf.PI / 180f;

            var x = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
            var y = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle);

            return new Vector2(x, y);
        }

        public static Vector2 IntersectionStraightLines(Vector2 xy1, Vector2 xy2, Vector2 re1, Vector2 re2)
        {
            var angle = Vector2.Angle(xy1 - xy2, re1 - re2);
            if (angle == 0 || angle == 180)
                return Vector2.zero;

            var x1 = xy1.x;
            var y1 = xy1.y;
            var x2 = xy2.x;
            var y2 = xy2.y;
            var r1 = re1.x;
            var e1 = re1.y;
            var r2 = re2.x;
            var e2 = re2.y;

            var a1 = x2 - x1;
            var b1 = e2 - e1;
            var a2 = r2 - r1;
            var b2 = y2 - y1;

            var y = ((y1 * a1 * b1) - (e1 * a2 * b2) + ((r1 - x1) * b2 * b1)) / ((a1 * b1) - (a2 * b2));

            var x = ((y - y1) * a1) / b2 + x1;

            return new Vector2(x, y);
        }
    }
}
