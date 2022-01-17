using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes.Geometry;

public class LakeMeshGenerator
{
    List<Vector3> vertices;
    List<Vector3> normals;
    List<int> triangles;
    List<Color> colors;
    List<Vector2> uv;

    public LakeMeshGenerator(List<RoadMeshGenerator.Edge> edges, Material material, float posZ) {
        vertices = new List<Vector3>();
        vertices = new List<Vector3>();
        normals = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
        uv = new List<Vector2>();

        var GO = new GameObject();
        GO.transform.position = new Vector3(0, 0, posZ);
        GO.transform.localScale = new Vector3(1, 1, -1);
        GO.name = "Roads";
        var mF = GO.AddComponent<MeshFilter>();
        var mR = GO.AddComponent<MeshRenderer>();
        mR.material = material;

        var mesh = new Mesh();

        var pointIndex = 0;
        var height = 15;

        var pos1 = Vector2.one * -1000;
        var pos2 = Vector2.one * 1000;
        foreach (var e in edges)
        {
            var edge = e;

            if (edge.Pos1.x > edge.Pos2.x)
            {
                edge = new RoadMeshGenerator.Edge(edge.Pos2, edge.Pos1);

                if (pos1.x < edge.Pos1.x)
                    pos1 = edge.Pos1;
                if (pos1.x < edge.Pos2.x)
                    pos1 = edge.Pos2;
            }
            else {
                if (pos2.x > edge.Pos1.x)
                    pos2 = edge.Pos1;
                if (pos2.x > edge.Pos2.x)
                    pos2 = edge.Pos2;
            }

            pointIndex = AddMeshOnEdge(edge, pointIndex, height);
        }

        pointIndex = AddMeshOnEdge(new RoadMeshGenerator.Edge(pos1, ((pos1 + pos2)/2)), pointIndex, height);
        pointIndex = AddMeshOnEdge(new RoadMeshGenerator.Edge(((pos1 + pos2) / 2), pos2), pointIndex, height);

        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
        mesh.uv = uv.ToArray();

        mF.mesh = mesh;
    }

    int AddMeshOnEdge(RoadMeshGenerator.Edge edge, int pointIndex, float height) {

        var pos = edge.Pos1;
        vertices.Add(pos);
        normals.Add(new Vector3(0, 0, 1));
        colors.Add(Color.white);
        uv.Add(pos);
        triangles.Add(pointIndex);
        pointIndex++;

        pos = edge.Pos2;
        vertices.Add(pos);
        normals.Add(new Vector3(0, 0, 1));
        colors.Add(Color.white);
        uv.Add(pos);
        triangles.Add(pointIndex);
        pointIndex++;

        pos = edge.Pos2 + Vector2.down * height;
        vertices.Add(pos);
        normals.Add(new Vector3(0, 0, 1));
        colors.Add(Color.white);
        uv.Add(pos);
        triangles.Add(pointIndex);
        pointIndex++;

        pos = edge.Pos1;
        vertices.Add(pos);
        normals.Add(new Vector3(0, 0, 1));
        colors.Add(Color.white);
        uv.Add(pos);
        triangles.Add(pointIndex);
        pointIndex++;

        pos = edge.Pos2 + Vector2.down * height;
        vertices.Add(pos);
        normals.Add(new Vector3(0, 0, 1));
        colors.Add(Color.white);
        uv.Add(pos);
        triangles.Add(pointIndex);
        pointIndex++;

        pos = edge.Pos1 + Vector2.down * height;
        vertices.Add(pos);
        normals.Add(new Vector3(0, 0, 1));
        colors.Add(Color.white);
        uv.Add(pos);
        triangles.Add(pointIndex);
        pointIndex++;

        return pointIndex;
    }
}
