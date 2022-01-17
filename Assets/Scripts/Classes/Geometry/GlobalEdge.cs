using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes.Geometry
{
    public class GlobalEdge
    {
        public GlobalPoint First { get; private set; }
        public GlobalPoint Second { get; private set; }

        public GlobalBox BoxCollision { get; private set; }

        public GlobalEdge(GlobalPoint First, GlobalPoint Second)
        {
            this.First = First;
            this.Second = Second;

            BoxCollision = new GlobalBox(this);
        }

        public void SubstitutionPoint(GlobalPoint havePoint, GlobalPoint sub)
        {
            if (First == havePoint)
                First = sub;

            if (Second == havePoint)
                Second = sub;
        }

        public void DrawGizmos()
        {
            if (First != null && Second != null)
                Gizmos.DrawLine(First.Position, Second.Position);
        }

        public void Destroy()
        {
            if (Second == null || First == null)
            {
                Second = null;
                First = null;
                return;
            }

                First.BreackConnect(Second, this);
            
            Second.BreackConnect(First, this);
        }

        public float GetDistance()
        {
            return (First.Position - Second.Position).magnitude;
        }

        public float GetSqrtMagnitude()
        {
            return (First.Position - Second.Position).sqrMagnitude;
        }

        public bool isIntersects(GlobalEdge edge)
        {
            if ((First == null && Second == null) || edge == null || (edge.First == null && edge.Second == null) || !BoxCollision.isIntersects(edge.BoxCollision))
                return false;

            var inter = Mathematic.IntersectionStraightLines(First.Position, Second.Position, edge.First.Position, edge.Second.Position);

            if (BoxCollision.isIntersects(inter) && edge.BoxCollision.isIntersects(inter))
                return true;


            return false;
        }

        public Vector2 GetVector()
        {
            return (First.Position - Second.Position);
        }
    }
}
