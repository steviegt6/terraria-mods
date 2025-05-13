using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Nightshade.Common.Features.VerletIntegratedBody;

namespace Nightshade.Common.Features;

[Autoload(Side = ModSide.Client)]
internal partial class VerletIntegratedBodySystem : ModSystem
{
    private static VerletIntegratedBody[] VerletBodies = new VerletIntegratedBody[1000];

    public struct VerletIntegratedBodyHandler
    {
        private int Index;

        public VerletIntegratedBodyHandler(int index)
        {
            this.Index = index;
        }

        public void InitializeAsChain(Vector2 startingPosition, Vector2 endingPosition,
            int pointAmount = 10, float gravityForceX = 0, float gravityForceY = 1,
            float mass = 1, float damping = 0.98f,
            float restingDistance = 200f, float stiffness = 1f
            )
        {
            VerletIntegratedBody body = VerletBodies[this.Index];

            body.Points.Clear();
            body.Links.Clear();

            for (int i = 0; i < pointAmount; i++)
            {
                VerletIntegratedBody.Point point = new VerletIntegratedBody.Point(Vector2.Lerp(startingPosition, endingPosition, (float)i / (pointAmount - 1)))
                {
                    Mass = mass,
                    DampingAmount = damping,
                    IsPinned = i == 0 || i == pointAmount - 1
                };
                body.Points[i] = point;
            }

            for (int i = 0; i < pointAmount - 1; i++)
            {
                Link link = new Link(i, i + 1)
                {
                    RestingDistance = restingDistance,
                    Stiffness = stiffness,
                };
                body.Links[i] = link;
            }

            body.GravityForce = new Vector2(gravityForceX, gravityForceY);
            body.IsActive = true;

            VerletBodies[this.Index] = body;
        }

        public void Deinitialize()
        {
            VerletIntegratedBody body = VerletBodies[this.Index];
            body.IsActive = true;
            VerletBodies[this.Index] = body;
        }
    }

    public static VerletIntegratedBodyHandler? RequestHandler()
    {
        for (int i = 0; i < VerletBodies.Length; i++)
        {
            VerletIntegratedBody body = VerletBodies[i];
            if (!body.IsActive)
            {
                return new VerletIntegratedBodyHandler(i);
            }
        }
        return null;
    }

    public override void Load()
    {
        base.Load();

        for (int n = 0; n < 1000; n++)
        {
            VerletBodies[n] = new VerletIntegratedBody();
        }
    }
}
