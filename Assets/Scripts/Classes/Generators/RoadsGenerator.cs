using Assets.Scripts.Classes.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes.Generators
{
    public class RoadsGenerator
    {

        RoadConfig config;
        GlobalPoint startPoint;
        private List<GlobalPoint> listGr = new List<GlobalPoint>();

        private float angle;
        private Vector2 direct;

        public RoadsGenerator(RoadConfig config, GlobalPoint startPoint)
        {
            this.config = config;
            this.startPoint = startPoint;
        }

        public List<RoadGraph> Generate()
        {
            var list = new List<RoadGraph>();


            for (var i = 0; i < config.countRoads; i++)
            {
                angle = UnityEngine.Random.Range(0, 360f);
                direct = Mathematic.RotateVector(angle, Vector2.up);
                listGr = new List<GlobalPoint>();
                var graph = new RoadGraph(startPoint, null);

                GraphDeeping(graph);
                list.Add(graph);
            }

            return list;
        }

        private void GraphDeeping(RoadGraph graph)
        {
            listGr.Add(graph.Point);


            //Debug.Log(angle);

            //Debug.Log(direct);


            var chPoints = graph.Point.AllConnectedPoints;

            if (chPoints.Count <= 0)
                return;

            GlobalPoint minAnglePoint = null;
            float oldAngle = 180;
            for (var y = 0; y < chPoints.Count; y++)
            {
                if (listGr.Contains(chPoints[y]))
                    continue;

                var angleNow = Mathf.Abs(Vector2.SignedAngle((chPoints[y].Position - graph.Point.Position), direct));

                if (angleNow <= oldAngle)
                {
                    minAnglePoint = chPoints[y];
                    oldAngle = angleNow;
                }
            }

            if (minAnglePoint == null)
                return;

            var gr = new RoadGraph(minAnglePoint, graph);
            graph.SetChild(gr);
            GraphDeeping(gr);
        }

        public static class RoadFind
        {
            private static List<GlobalPoint> listGr = new List<GlobalPoint>();
            private static Vector2 direct;

            public static List<Vector3> FindZeroRoadTo(Vector3 position)
            {
                listGr = new List<GlobalPoint>();
                var stPoint = CityGenerator.singleton.GetStartPoint();

                if (stPoint == null)
                    return null;

                var graph = new RoadGraph(stPoint, null);

                GraphDeeping(graph, position, GetEndDistance(position));

                var res = new List<Vector3>();

                foreach (var element in listGr)
                {
                    res.Add(element.Position);
                }

                res.Add(position);

                return res;
            }

            public static List<Vector3> FindRoadTo(Vector3 unitPosition, Vector3 endPosition)
            {
                listGr = new List<GlobalPoint>();
                var stPoint = GetNearPointPosition(unitPosition);

                if ((stPoint.Position - unitPosition).magnitude > (endPosition - unitPosition).magnitude)
                    return new List<Vector3>() { endPosition };

                if (stPoint == null)
                    return null;

                var graph = new RoadGraph(stPoint, null);

                GraphDeeping(graph, endPosition, GetEndDistance(endPosition));

                var res = new List<Vector3>();

                foreach (var element in listGr)
                {
                    res.Add(element.Position);
                }

                res.Add(endPosition);

                return res;
            }

            public static List<GameObject> FindRoadAnGameObjects(GameObject unitPosition, GameObject endPosition)
            {
                listGr = new List<GlobalPoint>();
                var stPoint = GetNearPointPosition(unitPosition.transform.position);

                if ((stPoint.Position - unitPosition.transform.position).magnitude > (endPosition.transform.position - unitPosition.transform.position).magnitude)
                    return new List<GameObject>() { endPosition };

                if (stPoint == null)
                    return null;

                var graph = new RoadGraph(stPoint, null);

                GraphDeeping(graph, endPosition.transform.position, GetEndDistance(endPosition.transform.position));

                var res = new List<GameObject>();

                foreach (var element in listGr)
                {
                    res.Add(element.GO);
                }

                res.Add(endPosition);

                return res;
            }

            public static GlobalPoint GetNearPointPosition(Vector3 position)
            {
                var all = CityGenerator.singleton.GetAllPoints();

                if (all.Count == 0)
                    throw new Exception("Count world points is zero");

                GlobalPoint old = all[0];
                foreach (var point in all)
                {
                    if ((old.Position - position).magnitude > (position - point.Position).magnitude)
                        old = point;
                }

                return old;
            }

            private static float GetEndDistance(Vector3 position)
            {
                var all = CityGenerator.singleton.GetAllPoints();

                float minDis = (all[0].Position - position).magnitude;
                foreach (var p in all)
                {
                    var dis = (position - p.Position).magnitude;
                    if (dis < minDis)
                        minDis = dis;
                }

                return minDis;
            }

            private static void GraphDeeping(RoadGraph graph, Vector3 endPoint, float distanceEnd)
            {
                direct = endPoint - graph.Point.Position;

                listGr.Add(graph.Point);

                if (direct.magnitude <= distanceEnd + 1)
                    return;

                var chPoints = graph.Point.AllConnectedPoints;

                if (chPoints.Count <= 0)
                    return;

                GlobalPoint minAnglePoint = null;
                float oldAngle = 180;
                for (var y = 0; y < chPoints.Count; y++)
                {
                    if (listGr.Contains(chPoints[y]))
                        continue;

                    var angleNow = Mathf.Abs(Vector2.SignedAngle((chPoints[y].Position - graph.Point.Position), direct));

                    if (angleNow <= oldAngle)
                    {
                        minAnglePoint = chPoints[y];
                        oldAngle = angleNow;
                    }
                }

                if (minAnglePoint == null)
                    return;

                var gr = new RoadGraph(minAnglePoint, graph);
                graph.SetChild(gr);
                GraphDeeping(gr, endPoint, distanceEnd);
            }

        }

        public class RoadGraph
        {
            public GlobalPoint Point { get; private set; }
            public RoadGraph Parent { get; private set; }
            //public List<GlobalPoint> ChildsPoints { get; private set; } = new List<GlobalPoint>();
            public RoadGraph Child { get; private set; }

            public RoadGraph(GlobalPoint point, RoadGraph parent)
            {
                Point = point;
                Parent = parent;
            }

            //public void AddChildPoints(params GlobalPoint[] points)
            //{
            //    //ChildsPoints.AddRange(points);
            //}
            public void DrawGizmos()
            {
                Gizmos.DrawSphere(Point.Position, 0.3f);
            }

            public void SetChild(RoadGraph child)
            {
                Child = child;
            }

            public IEnumerator<RoadGraph> GetEnumerator()
            {
                yield return this;

                if (Child != null)
                    foreach (var a in Child)
                        yield return a;
            }
        }
    }
}
