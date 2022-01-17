using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes.Geometry
{
    public class GlobalBox
    {
        public float Up { get => Mathf.Max(edge.First.Position.y, edge.Second.Position.y) - offsetCollision; }
        public float Down { get => Mathf.Min(edge.First.Position.y, edge.Second.Position.y) + offsetCollision; }
        public float Right { get => Mathf.Max(edge.First.Position.x, edge.Second.Position.x) - offsetCollision; }
        public float Left { get => Mathf.Min(edge.First.Position.x, edge.Second.Position.x) + offsetCollision; }

        private float offsetCollision = 0.000001f;

        private GlobalEdge edge;
        public GlobalBox(GlobalEdge edge)
        {
            this.edge = edge;
        }

        public bool isIntersects(GlobalBox box)
        {
            if (edge.First == null || edge.Second == null || box.edge == null || box.edge.First == null || box.edge.Second == null)
                return false;

            return Right >= box.Left && Left <= box.Right
                    && Up >= box.Down && Down <= box.Up;
        }

        public bool isIntersects(GlobalPoint point)
        {
            return Right >= point.Position.x && Left <= point.Position.x
                    && Up >= point.Position.y && Down <= point.Position.y;
        }

        public bool isIntersects(Vector2 point)
        {
            return Right >= point.x && Left <= point.x
                    && Up >= point.y && Down <= point.y;
        }


    }
}
