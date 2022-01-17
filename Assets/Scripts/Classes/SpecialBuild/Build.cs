using Assets.Scripts.Classes.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes.SpecialBuild
{
    public class Build
    {
        public GameObject GO { get; private set; }
        public Vector2 Position { get => GO.transform.position; private set => GO.transform.position = value; }
        public float AngleRotation { get; private set; }
        //public GlobalBox GlobalBox { get; private set; }
        public Vector2 Size { get; private set; }

        public Mesh mesh;

        public Build(Vector2 size, Vector2 position, float angleRotation)
        {
            GO = new GameObject();
            Position = position;
            var mF = GO.AddComponent<MeshFilter>();
            var mR = GO.AddComponent<MeshRenderer>();
            GO.AddComponent<CleanerBuild>();
            var rig = GO.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.useFullKinematicContacts = true;
            rig.sleepMode = RigidbodySleepMode2D.NeverSleep;
            GO.AddComponent<BuildStates>();

            AngleRotation = angleRotation;
            Size = size;

            mesh = MakeMesh(size);

            mF.mesh = mesh;

            var mC = GO.AddComponent<BoxCollider2D>();

            GO.transform.rotation = Quaternion.Euler(0, 0, angleRotation);

            GO.AddComponent<BuildStates>();
            GO.AddComponent<House>();
            GO.tag = "House";

            var lA = new List<GameCard>();
            var tS = GO.AddComponent<AimCursourInfo>();
            lA.Add(CardsSystem.singleton.GetPatternCard<PlagueDoctorCard>());
            lA.Add(CardsSystem.singleton.GetPatternCard<KnightCard>());

            foreach (var l in lA)
                if (l != null)
                    tS.AddCard(l);

        }

        private Mesh MakeMesh(Vector2 size)
        {
            var mesh = new Mesh();
            mesh.vertices = new Vector3[]
            {
                new Vector3(size.x, size.y) / 2,
                new Vector3(- size.x, size.y) / 2,
                new Vector3(- size.x, size.y) / 2,
                new Vector3(size.x, - size.y) / 2,
                new Vector3(size.x, - size.y) / 2,
                new Vector3(- size.x, - size.y) / 2
            };
            mesh.normals = new Vector3[]
            {
                new Vector3(0,0,1),
                new Vector3(0,0,1),
                new Vector3(0,0,1),
                new Vector3(0,0,1),
                new Vector3(0,0,1),
                new Vector3(0,0,1)
            };
            mesh.triangles = new int[]
            {
            0,1,3,4,2,5
            };

            mesh.colors = new Color[]
            {
                Color.green,
                Color.green,
                Color.green,
                Color.green,
                Color.green,
                Color.green,
            };

            mesh.uv = size.x <= size.y ?
        new Vector2[]
        {
                new Vector3(1, 1),
                new Vector3(0, 1),
                new Vector3(0, 1),
                new Vector3(1, 0),
                new Vector3(1, 0),
                new Vector3(0, 0)
        } :
        new Vector2[]{
                new Vector3(1, 0),
                new Vector3(1, 1),
                new Vector3(1, 1),
                new Vector3(0, 0),
                new Vector3(0, 0),
                new Vector3(0, 1)
        };
            return mesh;
        }

        public void DrawGizmos()
        {
        }
    }
}
