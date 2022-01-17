using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes.Geometry
{
    public class GlobalPoint
    {
        public Vector3 Position { get => GO.transform.position; set => GO.transform.position = value; }

        public float X { get => GO.transform.position.x; }
        public float Y { get => GO.transform.position.y; }
        public int index { get; private set; }
        public GameObject GO { get; private set; }
        public GlobalEdge ParentEdges { get; private set; }
        public GlobalPoint ParentPoint { get; private set; }
        public List<GlobalPoint> ChildsPoints { get; private set; } = new List<GlobalPoint>();
        public List<GlobalEdge> ConnectedEdges { get; private set; } = new List<GlobalEdge>();
        public List<GlobalPoint> AllConnectedPoints { get; private set; } = new List<GlobalPoint>();

        public GlobalPoint(Vector2 position, GameObject gameObject, GlobalPoint parentPoint, int index)
        {
            GO = gameObject;
            this.Position = position;
            this.index = index;
            ParentPoint = parentPoint;

            if (parentPoint == null)
                return;
            var pE = new GlobalEdge(this, parentPoint);

            parentPoint.AttachPoint(this, pE);
            ParentEdges = pE;
        }

        public void Merge(GlobalPoint point)
        {
            point.Position = Position;

            //Position = (point.Position + Position) / 2f;
            //AttachPoint(point.ParentPoint);

            //foreach (var a in point.ChildsPoints)
            //{

            //    AttachPoint(a);
            //}
            ////point.ConnectedEdges.Clear();
            ////foreach (var edge in point.ConnectedEdges)
            ////{
            ////    edge.SubstitutionPoint(point, this);
            ////}

            //point.Destroy();
        }

        public List<GlobalPoint> SortedOnMinimalDistance(List<GlobalPoint> points)
        {
            LinkedList<GlobalPoint> list = new LinkedList<GlobalPoint>(points);
            var pointss = new List<GlobalPoint>();
            list.Remove(this);

            while (list.Count > 0)
            {
                GlobalPoint min = null;
                foreach (var p in list)
                {
                    if (p == this)
                        continue;

                    if (min == null)
                        min = p;
                    else
                    {
                        if ((min.Position - Position).sqrMagnitude > (p.Position - Position).sqrMagnitude)
                            min = p;
                    }
                }

                list.Remove(min);
                pointss.Add(min);
            }

            return pointss;
        }

        public List<GlobalPoint> FindOnlyWithMyIndex(List<GlobalPoint> points)
        {
            LinkedList<GlobalPoint> list = new LinkedList<GlobalPoint>(points);
            var pointss = new List<GlobalPoint>();
            list.Remove(this);

            foreach (var p in list)
                if (p.index == index && this != p)
                    pointss.Add(p);

            return pointss;
        }

        public void BreackConnect(GlobalPoint point, GlobalEdge edge)
        {
            ChildsPoints.Remove(point);
            ConnectedEdges.Remove(edge);
            if (ParentPoint == point)
            {
                ParentPoint = null;
                ParentEdges = null;
            }
        }
        public void Destroy()
        {
            foreach (var ob in ChildsPoints)
            {
                if (ob != null)
                    ob.ParentEdges = null;
            }
            if (ParentPoint != null)
                ParentPoint.RemoveChildPoint(this);
            ParentPoint = null;

            while (ConnectedEdges.Count > 0)
            {
                ConnectedEdges[0].Destroy();
            }

            GameObject.Destroy(GO);
        }

        public void RemoveChildPoint(GlobalPoint point)
        {
            ConnectedEdges.Remove(point.ParentEdges);
            ChildsPoints.Remove(point);
        }

        public GlobalEdge AttachPoint(GlobalPoint point)
        {
            ChildsPoints.Add(point);
            return AttachEdge(new GlobalEdge(this, point));
        }

        public GlobalEdge AttachPoint(GlobalPoint point, GlobalEdge edge)
        {
            ChildsPoints.Add(point);
            return AttachEdge(edge);
        }
        private GlobalEdge AttachEdge(GlobalEdge edge)
        {
            ConnectedEdges.Add(edge);
            return edge;
        }

        public void DrawGizmos(float radius)
        {
            Gizmos.DrawSphere(Position, radius);

            if (ConnectedEdges != null && ConnectedEdges.Count > 0)
                foreach (var i in ConnectedEdges)
                {
                    if (i != null && i.First != null && i.Second != null)
                        Gizmos.DrawLine(i.First.Position, i.Second.Position);

                }
        }

        public void AddAllConnectedPoint(GlobalPoint point)
        {
            AllConnectedPoints.Add(point);
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return GO.GetHashCode() / 2 + Position.GetHashCode() / 2;
        }

        public IEnumerator<GlobalPoint> GetEnumerator()
        {
            yield return this;
            foreach (var ob in ChildsPoints)
            {
                foreach (var ob2 in ob)
                    yield return ob2;
            }
        }
    }
}
