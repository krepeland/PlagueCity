using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes.Geometry
{
    public class GlobalLoop
    {
        public List<LoopPoint> LoopPoints { get; private set; } = new List<LoopPoint>();
        public GraphConfig Config { get; private set; }

        private GameObject prefab;

        private LoopPoint root;
        public GlobalLoop(GraphConfig config, GameObject pointPrefab)
        {
            prefab = pointPrefab;
            Config = config;

            root = GenerateRoot(config, pointPrefab);
        }

        public LoopPoint DoIteration(bool makeDoubling, float lenghtStep)
        {
            root = root.DoGenerate(makeDoubling, lenghtStep);

            return root;
        }

        private void GenerateLoop(GraphConfig config, GameObject pointPrefab)
        {
            var root = GenerateRoot(config, pointPrefab);

            for (var i = 0; i < config.countPoints; i++)
            {
                root = root.DoGenerate(false, 1);
            }
        }

        private LoopPoint GenerateRoot(GraphConfig config, GameObject pointPrefab)
        {
            var left = new LoopPoint(pointPrefab, Vector3.left * config.step, null, null, null, config, LoopPoints);
            var right = new LoopPoint(pointPrefab, Vector3.right * config.step, null, left, null, config, LoopPoints);
            var up = new LoopPoint(pointPrefab, Vector3.up * config.step, null, right, left, config, LoopPoints);
            var down = new LoopPoint(pointPrefab, Vector3.down * config.step, null, left, right, config, LoopPoints);

            left.GlobalPoint.AddAllConnectedPoint(up.GlobalPoint);
            left.GlobalPoint.AddAllConnectedPoint(down.GlobalPoint);
            right.GlobalPoint.AddAllConnectedPoint(up.GlobalPoint);
            right.GlobalPoint.AddAllConnectedPoint(down.GlobalPoint);
            up.GlobalPoint.AddAllConnectedPoint(right.GlobalPoint);
            up.GlobalPoint.AddAllConnectedPoint(left.GlobalPoint);
            down.GlobalPoint.AddAllConnectedPoint(right.GlobalPoint);
            down.GlobalPoint.AddAllConnectedPoint(left.GlobalPoint);

            left.SetLeft(up);
            left.SetRight(down);

            right.SetRight(up);
            right.SetLeft(down);

            LoopPoints.Add(left);
            LoopPoints.Add(right);
            LoopPoints.Add(up);

            left.DoGenerate(false, config.step);
            down.DoGenerate(false, config.step);
            right.DoGenerate(false, config.step);

            return up.DoGenerate(true, config.step);

        }
    }

    public class LoopPoint
    {
        public Vector3 Position { get => GlobalPoint.Position; set => GlobalPoint.Position = value; }

        public LoopPoint Parent { get; private set; }
        public LoopPoint Left { get; private set; }
        public LoopPoint Right { get; private set; }

        public GlobalPoint GlobalPoint { get; private set; }

        private GameObject prefab;

        private GraphConfig config;

        private List<LoopPoint> allPoints;

        private float lenghtStep = 1;

        public LoopPoint(GameObject prefab, Vector3 position, LoopPoint parent, LoopPoint left, LoopPoint right, GraphConfig config, List<LoopPoint> allPoints)
        {
            Left = left;
            Right = right;
            Parent = parent;

            this.allPoints = allPoints;
            this.config = config;
            this.prefab = prefab;

            if (Parent == null)
                GlobalPoint = CreateGLPoint(position, null);
            else
                GlobalPoint = CreateGLPoint(position, Parent.GlobalPoint);

            allPoints.Add(this);
        }

        public void SetLeft(LoopPoint left)
        {
            Left = left;
        }

        public void SetRight(LoopPoint right)
        {
            Right = right;
        }

        public void Instead(LoopPoint point)
        {
            if (point == null)
                return;

            Left.SetRight(point);
            Right.SetLeft(point);
        }

        public void Instead(LoopPoint first, LoopPoint second)
        {
            if (first == null || second == null)
                return;

            Left.SetRight(first);

            first.SetLeft(Left);
            first.SetRight(second);

            second.SetLeft(first);
            second.SetRight(Right);

            Right.SetLeft(second);

        }


        public LoopPoint DoGenerate(bool makeDoubling, float lenghtStep)
        {
            this.lenghtStep = lenghtStep;


            if (makeDoubling)
            {
                if (Left != null && Right != null && ((Left.Position - Position).magnitude >= config.minDistanceDubling || (Right.Position - Position).magnitude >= config.minDistanceDubling))
                    if (!(this is LoopPointDubled))
                        return MakeDubling();
            }

            Right.MakeConnect();

            return Right;
        }

        private LoopPoint MakeDubling()
        {
            var first = CreateChildPoint<LoopPointDubled>(-config.dublinAngle);
            var second = CreateChildPoint<LoopPointDubled>(config.dublinAngle);

            GlobalPoint.AddAllConnectedPoint(first.GlobalPoint);
            GlobalPoint.AddAllConnectedPoint(second.GlobalPoint);

            Instead(first, second);

            return second;
        }

        public void MakeConnect()
        {
            GlobalPoint.AttachPoint(Left.GlobalPoint);

            GlobalPoint.AddAllConnectedPoint(Left.GlobalPoint);
            Left.GlobalPoint.AddAllConnectedPoint(GlobalPoint);

            Left.SetRight(this);

            var p2 = Left.CreateChildPoint<LoopPointEasy>(0);

            Left.GlobalPoint.AddAllConnectedPoint(p2.GlobalPoint);
            p2.GlobalPoint.AddAllConnectedPoint(Left.GlobalPoint);

            Left.Instead(p2);
        }

        private GlobalPoint CreateGLPoint(Vector3 position, GlobalPoint parent) => CreateGLPoint(position, parent, 0);

        private GlobalPoint CreateGLPoint(Vector3 position, GlobalPoint parent, int index)
        {
            var obj = CreateWayPoint(position);
            var gp = new GlobalPoint(obj.transform.position, obj, parent, index);
            AddAddon(gp);
            if (parent != null)
                gp.AddAllConnectedPoint(parent);

            return gp;
        }

        private GameObject CreateWayPoint(Vector3 position)
        {
            var obj = GameObject.Instantiate(prefab, position, Quaternion.identity);
            CreateAddonsOnObject(obj);

            return obj;
        }

        private void AddAddon(GlobalPoint gp)
        {
            var hp = gp.GO.AddComponent<GridPoint>();
            hp.Inicialize(gp);
        }

        private static void CreateAddonsOnObject(GameObject obj)
        {
            var tS = obj.AddComponent<VisitorsInfo>();
            var coll = obj.AddComponent<CircleCollider2D>();
            coll.radius = 2f;
            tS.AddVisitor(ItemsStorageSelector.singleton.GetVisitor<BarricadeVisitor>());

            obj.tag = "RoadPoint";
        }

        private LoopPoint CreateChildPoint<T>(float angle) where T : LoopPoint
        {
            var leftVec = (Position - Left.Position).normalized;
            var rightVec = (Position - Right.Position).normalized;

            var middleVec = (leftVec + rightVec).normalized / 2f;

            if (Vector3.Dot(Position, middleVec) < 0)
                middleVec *= -1;

            middleVec = Mathematic.RotateVector(angle, middleVec);

            LoopPoint chPoint = null;

            var posSpawn = (Position + middleVec.normalized * (config.step * lenghtStep));
            posSpawn = posSpawn.normalized * (posSpawn.magnitude * 1.05f);

            if (typeof(T).Name == typeof(LoopPointDubled).Name)
                chPoint = new LoopPointDubled(prefab, posSpawn, this, Left, Right, config, allPoints);

            if (typeof(T).Name == typeof(LoopPointEasy).Name)
                chPoint = new LoopPointEasy(prefab, posSpawn, this, Left, Right, config, allPoints);

            return chPoint;
        }
    }

    public class LoopPointDubled : LoopPoint
    {
        public LoopPointDubled(GameObject prefab, Vector3 position, LoopPoint parent, LoopPoint left, LoopPoint right, GraphConfig config, List<LoopPoint> allPoints)
            : base(prefab, position, parent, left, right, config, allPoints)
        { }
    }

    public class LoopPointEasy : LoopPoint
    {
        public LoopPointEasy(GameObject prefab, Vector3 position, LoopPoint parent, LoopPoint left, LoopPoint right, GraphConfig config, List<LoopPoint> allPoints)
            : base(prefab, position, parent, left, right, config, allPoints)
        { }
    }

}
