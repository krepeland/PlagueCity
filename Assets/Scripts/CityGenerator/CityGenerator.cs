using Assets.Scripts.Classes.Geometry;
using Assets.Scripts.Classes.SpecialBuild;
using System.Collections;
using Assets.Scripts.Classes.Generators;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Classes.Generators.RoadsGenerator;

public class CityGenerator : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private GameObject wayPointPref;

    [Header("Configs")]
    [SerializeField] private WaysConfig waysConfig;
    [SerializeField] private GraphConfig graphConfig;
    [SerializeField] public BuildsConfig buildsConfig;
    [SerializeField] private RoadConfig roadConfig;

    [Header("Debug Settings")]
    [SerializeField] private float radiusPoint = 0.1f;
    [SerializeField] private bool drawIntersect = false;

    private List<GlobalPoint> pointsWorld = new List<GlobalPoint>();
    private List<GlobalEdge> edgesWorld = new List<GlobalEdge>();
    public List<Build> buildsWorld = new List<Build>();
    private List<RoadGraph> roadGraphs = new List<RoadGraph>();

    private River river;
    private Lake lake;

    private List<GlobalPoint> rootPoints = new List<GlobalPoint>();

    private List<GlobalEdge> edgesIntersect = new List<GlobalEdge>();


    private GlobalLoop glLoop;
    private int countSteps;
    private int countStepsInvert;
    private int doublingCounter = 0;
    private int countDublingSteps = 1;

    public static CityGenerator singleton;

    //private void Awake()
    //{
    //    singleton = this;
    //}

    void Awake()
    {
        singleton = this;
        CreateNewGossamer(graphConfig);
        CreateBuilds(buildsConfig);
        ClearRoadBuilds(pointsWorld, 0.01f);
        countSteps = graphConfig.countPoints;

        CreateRoads(roadConfig);
        CreateWater();

        var list = new List<GlobalPoint>(river.Points);
        list.AddRange(lake.Points);

        ClearRoadBuilds(list, 0.1f);
        ClearRoadBuildsLake(lake.Points, 0.7f);

        CreateMeshes();
    }

    /// <summary>
    /// Hard Method
    /// </summary>
    /// <returns></returns>
    public List<Build> GetAllHouses()
    {
        ClearBreackBuildWorld();

        return buildsWorld;
    }

    private void ClearBreackBuildWorld()
    {
        List<Build> clear = new List<Build>();

        foreach (var house in buildsWorld)
        {
            if (house.GO != null)
                clear.Add(house);
        }

        buildsWorld = clear;
    }

    public void CreateMeshes()
    {

        var mainRoadsEdges = new List<RoadMeshGenerator.Edge>();
        foreach (var b in roadGraphs)
            foreach (var i in b)
            {
                if (i.Child == null) continue;
                mainRoadsEdges.Add(new RoadMeshGenerator.Edge(i.Point.Position, i.Child.Point.Position));
            }
        var mainRoadsBuilder = new RoadMeshGenerator(mainRoadsEdges, 0.075f, 0.075f, -1, MaterialManager.singleton.MainRoadsMaterial);

        var OutlineWidth = 0.025f;
        var mainRoadsBuilderOutline = new RoadMeshGenerator(mainRoadsEdges, 0.075f + OutlineWidth, 0.075f, -1.1f, MaterialManager.singleton.OutLineMaterial);


        var edges = new List<RoadMeshGenerator.Edge>();
        foreach (var b in pointsWorld)
            foreach (var e in b.ConnectedEdges)
            {
                if (e.Second.Position.magnitude > buildsConfig.DistaceSpawn + 1 || e.First.Position.magnitude > buildsConfig.DistaceSpawn + 1) continue;
                edges.Add(new RoadMeshGenerator.Edge(e.First.Position, e.Second.Position));
            }
        var roadsBuilder = new RoadMeshGenerator(edges, 0.025f, 0.025f, -4, MaterialManager.singleton.RoadsMaterial);

        OutlineWidth = 0.025f;
        var roadsBuilderOutline = new RoadMeshGenerator(edges, 0.025f + OutlineWidth, 0.025f, -4.1f, MaterialManager.singleton.OutLineMaterial);


        var sidedEdges = RoadMeshGenerator.GetSidesEdges(edges, 0.035f, 0.03f);

        var roadsBuilderOutline1 = new RoadMeshGenerator(sidedEdges.Item1, 0.005f, 0.025f, -2.2f, MaterialManager.singleton.OutLineMaterial);
        var roadsBuilderOutline2 = new RoadMeshGenerator(sidedEdges.Item2, 0.005f, 0.025f, -2.2f, MaterialManager.singleton.OutLineMaterial);
                                                                               

        var riverEdges = new List<RoadMeshGenerator.Edge>();
        foreach (var b in river.Points)
            foreach (var i in b)
            {
                foreach (var e in i.ConnectedEdges)
                {
                    riverEdges.Add(new RoadMeshGenerator.Edge(e.First.Position, e.Second.Position));
                }
            }
        var riverBuilder = new RoadMeshGenerator(riverEdges, 0.2f, 0.05f, -3, MaterialManager.singleton.RiverMaterial);

        OutlineWidth = 0.05f;
        var riverBuilderOutline = new RoadMeshGenerator(riverEdges, 0.2f + OutlineWidth, 0.05f, -5f, MaterialManager.singleton.OutLineMaterial);
    }

    private List<GlobalPoint> GetUnical(List<GlobalPoint> list)
    {
        var res = new List<GlobalPoint>();

        for (var i = 0; i < list.Count; i++)
        {
            var inters = false;
            for (var y = 0; y < res.Count; y++)
            {
                if (y == i)
                    continue;

                if (list[i].Equals(list[y]))
                    inters = true;
            }

            if (!inters)
                res.Add(list[i]);
        }

        return res;
    }

    private void RotatePoints(List<GlobalPoint> points, float angle)
    {
        foreach (var point in points)
        {
            point.Position = Mathematic.RotateVector(angle, point.Position);
        }
    }

    private void CreateWater()
    {
        Assets.Scripts.Classes.Generators.WaterGenerator generator = new Assets.Scripts.Classes.Generators.WaterGenerator();
        var info = generator.Generate();
        river = info.Item1;
        lake = info.Item2;

        var list = new List<GlobalPoint>(river.Points);
        list.AddRange(lake.Points);

        ClearRoadBuilds(list, 0.2f);


        var offset = new Vector3(0, 0.2f, 0);
        var edges = new List<RoadMeshGenerator.Edge>();
        foreach (var point in lake.Points) {
            foreach (var e in point.ChildsPoints) {
                edges.Add(new RoadMeshGenerator.Edge(point.Position + offset, e.Position + offset));
            }
        }
        var lakeMeshGenerator = new LakeMeshGenerator(edges, MaterialManager.singleton.RiverMaterial, -0.5f);
    }

    private void CreateRoads(RoadConfig config)
    {
        RoadsGenerator generator = new RoadsGenerator(roadConfig, pointsWorld[1]);

        roadGraphs = generator.Generate();
    }

    public GlobalPoint GetStartPoint()
    {
        if (pointsWorld.Count >= 1)
            return pointsWorld[1];

        return null;
    }

    public List<GlobalPoint> GetAllPoints()
    {
        return pointsWorld;
    }

    private float ctd = 0;
    private float sizer = 0;

    private void CreateBuilds(BuildsConfig buildsConfig)
    {
        BuildsGenerator generator = new BuildsGenerator(buildsConfig);

        buildsWorld = generator.Generate(pointsWorld);
    }

    private void ClearRoadBuilds(List<GlobalPoint> allPoints, float radius)
    {
        foreach (var point in allPoints)
        {
            foreach (var chPoint in point.AllConnectedPoints)
            {
                var vector = (point.Position - chPoint.Position);
                var magnitude = vector.magnitude;

                RaycastHit2D[] hits =
                Physics2D.CircleCastAll(chPoint.Position, radius, vector, magnitude);

                if (hits.Length != 0)
                    foreach (var hit in hits)
                        if (hit.collider.gameObject.tag == "House")
                            Destroy(hit.collider.gameObject);
            }
        }
    }

    private void ClearRoadBuildsLake(List<GlobalPoint> allPoints, float radius)
    {
        foreach (var point in allPoints)
        {
            foreach (var chPoint in point.AllConnectedPoints)
            {
                var vector = Vector3.down;
                var magnitude = 20f;

                RaycastHit2D[] hits =
                Physics2D.CircleCastAll(chPoint.Position, radius, vector, magnitude);

                if (hits.Length != 0)
                    foreach (var hit in hits)
                        if (hit.collider.gameObject.tag == "House")
                            Destroy(hit.collider.gameObject);
            }
        }
    }


    private void CreateNewGossamer(GraphConfig graphConfig)
    {
        glLoop = new GlobalLoop(graphConfig, wayPointPref);

        foreach (var element in glLoop.LoopPoints)
        {
            pointsWorld.Add(element.GlobalPoint);
        }

        countSteps = graphConfig.countPoints;

        RunBuildGossamerElement();
    }

    private void ResizeGlossamer()
    {
        foreach (var i in pointsWorld)
        {
            i.Position += i.Position * i.Position.magnitude / 2f;
        }
    }

    private void RunBuildGossamerElement()
    {
        while (countSteps > 0)
        {
            bool dublin = false;
            if (doublingCounter >= countDublingSteps)
            {
                dublin = true;
                doublingCounter = -1;

                countDublingSteps = GetRandomDublinCount(0, 1) + ((countStepsInvert / 40) * (countStepsInvert / 40));
                ctd += 0.4f;
                sizer += 0.4f;
            }

            var point = glLoop.DoIteration(dublin, 1 + (countStepsInvert * 0.001f)).GlobalPoint;
            pointsWorld.Add(point);

            countSteps--;
            countStepsInvert++;
            doublingCounter++;
        }
    }

    private void OnDrawGizmos()
    {
        if (pointsWorld != null)
            foreach (var p in pointsWorld)
                p.DrawGizmos(radiusPoint);


        if (drawIntersect)
            foreach (var i in edgesIntersect)
                i.DrawGizmos();

        if (buildsWorld != null)
            foreach (var b in buildsWorld)
                b.DrawGizmos();

        if (roadGraphs != null)
            foreach (var b in roadGraphs)
                foreach (var i in b)
                    i.DrawGizmos();

        if (river != null)
            river.DrawGizmos();

        if (lake != null)
            lake.DrawGizmos();
    }

    private List<GlobalPoint> MergePoints(List<GlobalPoint> points, float distance)
    {
        var newPoints = new List<GlobalPoint>();

        foreach (var p in points)
        {
            var sort = p.SortedOnMinimalDistance(points);
            newPoints.Add(p);


            foreach (var s in sort)
            {
                if (s == p)
                    continue;

                if ((s.Position - p.Position).sqrMagnitude <= distance * distance)
                {
                    p.Merge(s);
                }
            }
        }

        return newPoints;
    }

    private void CreateAddConnections(List<GlobalPoint> points)
    {
        foreach (var b in points)
        {
            if (b.ChildsPoints.Count <= 1)
            {
                var sortedAll = b.SortedOnMinimalDistance(points);

                var countAttach2 = 2;
                var currectAttach2 = countAttach2 > sortedAll.Count ? sortedAll.Count : countAttach2;

                for (var i = 0; i < currectAttach2; i++)
                {
                    if (currectAttach2 >= sortedAll.Count)
                        break;

                    var p = sortedAll[i];
                    if (b.ParentPoint == null || b.ParentPoint != p || p.Position != b.Position)
                        b.AttachPoint(p);
                    else
                        currectAttach2++;
                }
            }
        }
    }

    public void CreateConnectsPoints(List<GlobalPoint> points)
    {
        foreach (var b in points)
        {
            if (b.index == 0)
                continue;

            var onlyIndex = b.FindOnlyWithMyIndex(points);
            var sorted = b.SortedOnMinimalDistance(onlyIndex);

            var countAttach = 2;
            var currectAttach = countAttach > sorted.Count ? sorted.Count : countAttach;

            for (var i = 0; i < currectAttach; i++)
                b.AttachPoint(sorted[i]);
        }
    }

    public GlobalPoint CreateGraph(GraphConfig graphConfig, Vector3 startPosition, int numberRecurs, float angle, Vector3 direct, GlobalPoint parentPoint, float lenghtStep, int startNumberIndex)
    {
        if (numberRecurs <= 0)
            return null;

        var position = startPosition + (direct.normalized * (lenghtStep));//new Vector3(0, graphConfig.step * i, 0);

        var tryDubling = TryDublingPoint(graphConfig);

        GlobalPoint point = null;
        if (tryDubling)
            point = CreateGLPoint(position, parentPoint, 0);
        else
            point = CreateGLPoint(position, parentPoint, startNumberIndex);

        if (isIntersect(point))
        {
            Debug.Log("Destroy point");
            point.Destroy();
            return null;
        }
        pointsWorld.Add(point);

        if (parentPoint == null)
            rootPoints.Add(point);

        numberRecurs--;
        lenghtStep *= 1.3f;
        if (tryDubling)
        {

            var lAngle = Mathematic.RotateVector(angle, direct);
            var rAngle = Mathematic.RotateVector(-angle, direct);
            CreateGraph(graphConfig, position, numberRecurs, graphConfig.dublinAngle * lenghtStep, rAngle, point, lenghtStep, startNumberIndex);   /// 2
            CreateGraph(graphConfig, position, numberRecurs, graphConfig.dublinAngle * lenghtStep, lAngle, point, lenghtStep, startNumberIndex);   /// 2
        }
        else
        {
            startNumberIndex++;
            CreateGraph(graphConfig, position, numberRecurs, graphConfig.dublinAngle, direct, point, lenghtStep, startNumberIndex);

        }

        return point;
    }

    private void ClearIntersectEdges(List<GlobalPoint> points)
    {
        foreach (var p in points)
        {
            var buffer = new List<GlobalEdge>();

            if (TryGetIntersectEdges(p, ref buffer))
            {
                foreach (var edge in buffer)
                {
                    edgesIntersect.Add(edge);
                    edge.Destroy();
                }
            }
        }
    }

    private bool isIntersect(GlobalPoint point)
    {
        foreach (var p in pointsWorld)
            foreach (var e in p.ConnectedEdges)
                if (e.isIntersects(point.ParentEdges))
                    return true;

        return false;
    }

    private bool TryGetIntersectEdges(GlobalPoint point, ref List<GlobalEdge> edges)
    {
        var intersect = false;
        edges = new List<GlobalEdge>();
        foreach (var p in pointsWorld)
            foreach (var e in p.ConnectedEdges)
                if (e.isIntersects(point.ParentEdges))
                {
                    intersect = true;
                    edges.Add(e);
                }
        return intersect;
    }

    private bool TryDublingPoint(GraphConfig graphConfig)
    {
        var random = Random.Range(0f, 1f);
        if (graphConfig.probabilityDublin >= random)
        {
            return true;
        }

        return false;
    }

    private GlobalPoint CreateGLPoint(Vector3 position, GlobalPoint parent, int index)
    {
        var obj = CreateWayPoint(position);
        var gp = new GlobalPoint(obj.transform.position, obj, parent, index);
        return gp;

    }

    private GameObject CreateWayPoint(Vector3 position)
    {
        return GameObject.Instantiate(wayPointPref, position, Quaternion.identity);
    }

    private int GetRandomDublinCount(int a, int b)
    {
        return UnityEngine.Random.Range(a, b);
    }

}
