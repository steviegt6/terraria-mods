using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Nightshade.Common.Features.VerletIntegratedBody;

namespace Nightshade.Common.Features;

[Autoload(Side = ModSide.Client)]
internal partial class VerletIntegratedBodySystem : ModSystem
{
    private const int constraint_iteration_amount = 20;

    public override void PostUpdateProjectiles()
    {
        base.PostUpdateProjectiles();

        foreach (var body in VerletBodies)
        {
            if (body.IsActive)
            {
#region Update
                for (int i = 0; i < body.Links.Count; i++)
                {
                    VerletIntegratedBody.Link link = body.Links[i];
                    VerletIntegratedBody.Link newLink = link;

                    for (int j = 0; j < constraint_iteration_amount; j++)
                    {
                        Vector2 difference = body.Points[newLink.FirstPointIndex].Position - body.Points[newLink.SecondPointIndex].Position;
                        float distance = difference.Length();
                        float distanceScalar = (newLink.RestingDistance - distance) / distance;

                        float firstWeight = 1 / body.Points[newLink.FirstPointIndex].Mass;
                        float secondWeight = 1 / body.Points[newLink.SecondPointIndex].Mass;
                        float firstScalar = (firstWeight / (firstWeight + secondWeight)) * newLink.Stiffness;
                        float secondScalar = newLink.Stiffness - firstScalar;

                        VerletIntegratedBody.Point newFirstPoint = body.Points[newLink.FirstPointIndex];
                        newFirstPoint.Position += difference * firstScalar * distanceScalar;
                        body.Points[newLink.FirstPointIndex] = newFirstPoint;
                        VerletIntegratedBody.Point newSecondPoint = body.Points[newLink.SecondPointIndex];
                        newSecondPoint.Position -= difference * secondScalar * distanceScalar;
                        body.Points[newLink.SecondPointIndex] = newSecondPoint;
                    }

                    link = newLink;
                }
                for (int i = 0; i < body.Points.Count; i++)
                {
                    VerletIntegratedBody.Point point = body.Points[i];
                    VerletIntegratedBody.Point newPoint = point;

                    newPoint.Acceleration = body.GravityForce;// * point.Mass / point.Mass;

                    Vector2 velocity = newPoint.Position - newPoint.PreviousPosition;
                    velocity *= newPoint.DampingAmount;
                    newPoint.PreviousPosition = newPoint.Position;
                    newPoint.Position = newPoint.Position + velocity + newPoint.Acceleration * 0.5f;

                    if (newPoint.IsPinned)
                    {
                        newPoint.Position = newPoint.PreviousPosition = newPoint.PinnedPosition;
                    }

                    point = newPoint;
                }
#endregion
            }
        }
    }
}
