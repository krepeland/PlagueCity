using Assets.Scripts.Classes.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes.Generators
{
    public class WaterGenerator
    {
        public WaterGenerator()
        {

        }

        public (River, Lake) Generate()
        {
            var river = new River(Vector2.zero);
            var lake = new Lake(river);

            OffSet(river, lake);

            return (river, lake);
        }

        public void OffSet(River river, Lake lake)
        {
            var list = new List<GlobalPoint>(river.Points);
            list.AddRange(lake.Points);

            var randomDown = UnityEngine.Random.Range(1f, 2f);
            var randomFront = UnityEngine.Random.Range(-5f, 5f);
            foreach (var point in list)
            {
                point.Position += Vector3.down * 4f * randomDown;
                point.Position += Vector3.right * randomFront;
            }
        }
    }

    public class River
    {
        public List<GlobalPoint> Points { get; private set; } = new List<GlobalPoint>();
        public River(Vector2 startPosition)
        {
            GlobalPoint old = null;
            for (var i = 0; i < 30; i++)
            {
                Vector2 position;
                if (old == null)
                    position = startPosition;
                else
                    position = (Vector2)old.Position + (Vector2.up * 0.8f) + (Vector2.right * UnityEngine.Random.Range(-0.4f, 0.4f));

                GameObject go = new GameObject();
                GlobalPoint point = new GlobalPoint(position, go, old, 0);

                if (old != null)
                {
                    point.AddAllConnectedPoint(old);
                    old.AddAllConnectedPoint(point);
                }
                Points.Add(point);
                old = point;
            }
        }

        public void DrawGizmos()
        {
            foreach (var point in Points)
            {
                point.DrawGizmos(0);
                Gizmos.DrawCube(point.Position, new Vector3(0.2f, 0.2f, 0.2f));
            }
        }
    }

    public class Lake
    {
        public List<GlobalPoint> Points { get; private set; } = new List<GlobalPoint>();
        public Lake(River river)
        {
            if (river.Points.Count == 0)
                throw new Exception("River is break");

            var parent = river.Points[0];

            CreateLine(parent.Position, Vector2.right, parent);
            CreateLine(parent.Position, -Vector2.right, parent);
        }

        private void CreateLine(Vector2 startPosition, Vector2 direct, GlobalPoint parent)
        {
            GlobalPoint old = parent;
            var countIteration = 30;

            for (var i = 0; i < countIteration; i++)
            {
                Vector2 position;
                if (old == null)
                    position = startPosition;
                else
                    position = (Vector2)old.Position + (direct.normalized * 0.8f) + (Vector2.up * UnityEngine.Random.Range(-0.3f, 0.3f) - Vector2.up * 0.15f); GameObject go = new GameObject();

                GlobalPoint point = new GlobalPoint(position, go, old, 0);

                if (old != null)
                {
                    point.AddAllConnectedPoint(old);
                    old.AddAllConnectedPoint(point);
                }

                if (countIteration - 1 <= i)
                    point.Position -= Vector3.up * 15;

                Points.Add(point);
                old = point;
            }
        }

        public void DrawGizmos()
        {
            foreach (var point in Points)
            {
                point.DrawGizmos(0);
                Gizmos.DrawCube(point.Position, new Vector3(0.2f, 0.2f, 0.2f));
            }
        }

    }
}
