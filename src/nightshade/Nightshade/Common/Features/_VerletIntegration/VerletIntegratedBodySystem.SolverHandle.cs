using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria.ModLoader;

using static Nightshade.Common.Features.VerletIntegratedBody;

namespace Nightshade.Common.Features;

[Autoload(Side = ModSide.Client)]
internal partial class VerletIntegratedBodySystem : ModSystem
{
    public struct ChainInitializationParameters
    {
        public Vector2 StartingPosition;
        public Vector2 EndingPosition;

        public int PointAmount = 10;
        public Vector2 Gravity = new Vector2(0, 1);
        public float PointMass = 1;
        public float PointDampingAmount = 0.98f;
        public float LinkRestingDistance = 200f;
        public float LinkStiffness = 1f;

        public ChainInitializationParameters(Vector2 startingPosition, Vector2 endingPosition)
        {
            StartingPosition = startingPosition;
            EndingPosition = endingPosition;
        }
    }

    private static VerletIntegratedBody[] VerletBodies = new VerletIntegratedBody[1000];

    public struct VerletIntegratedBodyHandle : IEnumerable<VerletIntegratedBody.Point>
    {
        private int index;

        public VerletIntegratedBodyHandle(int index)
        {
            this.index = index;
        }

        public void InitializeAsChain(ChainInitializationParameters parameters)
        {
            var body = VerletBodies[index];

            body.Points.Clear();
            body.Links.Clear();

            for (var i = 0; i < parameters.PointAmount; i++)
            {
                var point = new VerletIntegratedBody.Point(Vector2.Lerp(parameters.StartingPosition, parameters.EndingPosition, (float)i / (parameters.PointAmount - 1)))
                {
                    Mass = parameters.PointMass,
                    DampingAmount = parameters.PointDampingAmount,
                    IsPinned = i == 0 || i == parameters.PointAmount - 1
                };
                body.Points[i] = point;
            }

            for (var i = 0; i < parameters.PointAmount - 1; i++)
            {
                var link = new Link(i, i + 1)
                {
                    RestingDistance = parameters.LinkRestingDistance,
                    Stiffness = parameters.LinkStiffness,
                };
                body.Links[i] = link;
            }

            body.GravityForce = parameters.Gravity;
            body.IsActive = true;

            VerletBodies[index] = body;
        }

        public void Deinitialize()
        {
            var body = VerletBodies[index];
            body.IsActive = true;
            VerletBodies[index] = body;
        }

        public VerletIntegratedBody.Point this[int i]
        {
            get
            {
                var body = VerletBodies[index];
                return body.Points[i];
            }
        }

        public int PointAmount
        {
            get
            {
                var body = VerletBodies[index];
                return body.Points.Count;
            }
        }

        public IEnumerator<VerletIntegratedBody.Point> GetEnumerator() => VerletBodies[index].Points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static VerletIntegratedBodyHandle? RequestHandler()
    {
        for (var i = 0; i < VerletBodies.Length; i++)
        {
            var body = VerletBodies[i];
            if (!body.IsActive)
            {
                return new VerletIntegratedBodyHandle(i);
            }
        }
        return null;
    }

    public override void Load()
    {
        base.Load();

        for (var n = 0; n < 1000; n++)
        {
            VerletBodies[n] = new VerletIntegratedBody();
        }
    }
}