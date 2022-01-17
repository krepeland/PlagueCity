using Assets.Scripts.Classes.Geometry;
using Assets.Scripts.Classes.SpecialBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes.Generators
{
    public class BuildsGenerator
    {
        public List<Build> Builds { get; private set; }

        private BuildsConfig config;
        public BuildsGenerator(BuildsConfig buildsConfig)
        {
            config = buildsConfig;
        }

        public List<Build> Generate(List<GlobalPoint> globalPoints)
        {
            var result = new List<Build>();

            foreach (var point in globalPoints)
            {
                foreach (var edge in point.ConnectedEdges)
                {
                    var branch = CreateBranchBuilds(edge);
                    result.AddRange(branch);
                }
            }

            return result;
        }

        private List<Build> CreateBranchBuilds(GlobalEdge edge)
        {
            var result = new List<Build>();

            var branchVector = edge.GetVector();
            var startPosition = (Vector2)edge.Second.Position;

            var rPosition = Mathematic.RotateVector(90f, branchVector);
            var lPosition = Mathematic.RotateVector(-90f, branchVector);

            var rightLine = CreateBranchLine(startPosition + rPosition.normalized * config.WidthWay, branchVector);
            var leftLine = CreateBranchLine(startPosition + lPosition.normalized * config.WidthWay, branchVector);

            result.AddRange(rightLine);
            result.AddRange(leftLine);

            
            
            var rightLineAdd = CreateBranchLine(startPosition + rPosition.normalized * config.WidthWay * 2, branchVector);
            var leftLineAdd = CreateBranchLine(startPosition + lPosition.normalized * config.WidthWay * 2, branchVector);

            result.AddRange(rightLineAdd);
            result.AddRange(leftLineAdd);


            var rightLineAdd2 = CreateBranchLine(startPosition + rPosition.normalized * config.WidthWay * 3, branchVector);
            var leftLineAdd2 = CreateBranchLine(startPosition + lPosition.normalized * config.WidthWay * 3, branchVector);

            result.AddRange(rightLineAdd2);
            result.AddRange(leftLineAdd2);

            return result;
        }

        private List<Build> CreateBranchLine(Vector2 startPosition, Vector2 direct)
        {
            var result = new List<Build>();
            var rotate = Vector2.SignedAngle(Vector2.up, direct);
            var lenghtBranch = direct.magnitude;

            Vector2 positionBuild = startPosition;
            Build oldBuild = null;
            while (lenghtBranch > config.GetMaxSize())
            {
                if (config.DistaceSpawn >= (Vector2.zero - positionBuild).magnitude)
                {
                    var build = CreateBuild(positionBuild, rotate);
                    result.Add(build);
                    lenghtBranch -= build.Size.x + config.DistanceBetween;
                    positionBuild += (build.Size.x + config.DistanceBetween) * direct.normalized;
                    oldBuild = build;
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private Build CreateBuild(Vector2 position, float angle)
        {
            var xR = UnityEngine.Random.Range(-config.SizeRandom.x, config.SizeRandom.x);
            var yR = UnityEngine.Random.Range(-config.SizeRandom.y, config.SizeRandom.y);

            var build = new Build(config.SizeBuild + new Vector2(xR, yR), position, angle);

            return build;
        }
    }
}
