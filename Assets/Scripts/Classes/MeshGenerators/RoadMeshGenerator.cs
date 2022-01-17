using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Classes.Generators.RoadsGenerator;
using Assets.Scripts.Classes.Geometry;

public class RoadMeshGenerator
{
    List<Vector3> vertices;
    List<Vector3> normals;
    List<int> triangles;
    List<Color> colors;
    List<Vector2> uv;
    Dictionary<Vector2, HashSet<Vector2>> points;

    public class Edge 
    {
        public Vector2 Pos1;
        public Vector2 Pos2;

        public Edge(Vector2 pos1, Vector2 pos2) {
            Pos1 = pos1;
            Pos2 = pos2;
        }
    }

    public static (List<Edge>, List<Edge>) GetSidesEdges(List<Edge> edges, float width, float pointRadius) {
        var result = (new List<Edge>(), new List<Edge>());

        foreach (var b in edges)
        {
            var pos1 = new Vector2(b.Pos1.x, b.Pos1.y);
            var pos2 = new Vector2(b.Pos2.x, b.Pos2.y);

            var direction = (pos2 - pos1).normalized;
            var offsetDirection = direction * pointRadius;
            var offset = new Vector2(direction.y, -direction.x) * width;

            result.Item1.Add(new Edge(pos1 + offset + offsetDirection, pos2 + offset - offsetDirection));
            result.Item2.Add(new Edge(pos1 - offset + offsetDirection, pos2 - offset - offsetDirection));
        }

        return result;
    }

    public RoadMeshGenerator(List<Edge> edges, float meshWidth, float pointRadius, float posZ, Material material = null)
    {
        vertices = new List<Vector3>();
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
        uv = new List<Vector2>();
        points = new Dictionary<Vector2, HashSet<Vector2>>();

        var GO = new GameObject();
        GO.transform.position = new Vector3(0, 0, posZ);
        GO.transform.localScale = new Vector3(1, 1, -1);
        GO.name = "Roads";
        var mF = GO.AddComponent<MeshFilter>();
        var mR = GO.AddComponent<MeshRenderer>();
        mR.material = material;

        var mesh = new Mesh();

        var pointIndex = 0;
        foreach (var b in edges)
        {
            var pos1 = new Vector2(b.Pos1.x, b.Pos1.y);
            var pos2 = new Vector2(b.Pos2.x, b.Pos2.y);

            foreach (var e in GetPoints(pos1, pos2, meshWidth, pointRadius))
            {
                vertices.Add(e);
                normals.Add(new Vector3(0, 0, 1));
                colors.Add(Color.white);
                uv.Add(e);
                triangles.Add(pointIndex);
                pointIndex++;
            }
        }

        foreach (var e in points)
        {
            var list = new List<Vector2>();
            foreach (var p in e.Value)
                list.Add(p);
            if (list.Count < 3) continue;

            var hull = JarvisMarch.ConvexHull(list);

            for (var i = 2; i < hull.Length; i++)
            {
                vertices.Add(hull[0]);
                vertices.Add(hull[i - 1]);
                vertices.Add(hull[i]);

                normals.Add(new Vector3(0, 0, 1));
                normals.Add(new Vector3(0, 0, 1));
                normals.Add(new Vector3(0, 0, 1));

                colors.Add(Color.white);
                colors.Add(Color.white);
                colors.Add(Color.white);

                uv.Add(hull[0]);
                uv.Add(hull[i - 1]);
                uv.Add(hull[i]);

                triangles.Add(pointIndex);
                triangles.Add(pointIndex + 2);
                triangles.Add(pointIndex + 1);

                pointIndex += 3;
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
        mesh.uv = uv.ToArray();

        mF.mesh = mesh;
    }


    IEnumerable<Vector2> GetPoints(Vector2 point1, Vector2 point2, float offsetSize, float pointSize)
    {
        var direction = (point2 - point1).normalized;
        var offsetDirection = direction * pointSize;
        var offset = new Vector2(direction.y, -direction.x) * offsetSize;

        var key1 = new Vector2(Mathf.Round(point1.x * 1000) * 0.001f, Mathf.Round(point1.y * 1000) * 0.001f);
        var key2 = new Vector2(Mathf.Round(point2.x * 1000) * 0.001f, Mathf.Round(point2.y * 1000) * 0.001f);

        if (!points.ContainsKey(key1))
            points[key1] = new HashSet<Vector2>();
        points[key1].Add(point1 + offset + offsetDirection);
        points[key1].Add(point1 - offset + offsetDirection);

        if (!points.ContainsKey(key2))
            points[key2] = new HashSet<Vector2>();
        points[key2].Add(point2 + offset - offsetDirection);
        points[key2].Add(point2 - offset - offsetDirection);

        yield return point1 + offset + offsetDirection;
        yield return point1 - offset + offsetDirection;
        yield return point2 + offset - offsetDirection;
        yield return point2 + offset - offsetDirection;
        yield return point1 - offset + offsetDirection;
        yield return point2 - offset - offsetDirection;
    }
}
