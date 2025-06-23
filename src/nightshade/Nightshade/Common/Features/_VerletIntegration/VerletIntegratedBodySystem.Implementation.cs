using Terraria.ModLoader;

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
                for (var i = 0; i < body.Links.Count; i++)
                {
                    var link = body.Links[i];
                    var newLink = link;

                    #region Solve constraints
                    for (var j = 0; j < constraint_iteration_amount; j++)
                    {
                        var difference = body.Points[newLink.FirstPointIndex].Position - body.Points[newLink.SecondPointIndex].Position;
                        var distance = difference.Length();
                        var distanceScalar = (newLink.RestingDistance - distance) / distance;

                        var firstWeight = 1 / body.Points[newLink.FirstPointIndex].Mass;
                        var secondWeight = 1 / body.Points[newLink.SecondPointIndex].Mass;
                        var firstScalar = (firstWeight / (firstWeight + secondWeight)) * newLink.Stiffness;
                        var secondScalar = newLink.Stiffness - firstScalar;

                        var newFirstPoint = body.Points[newLink.FirstPointIndex];
                        newFirstPoint.Position += difference * firstScalar * distanceScalar;
                        body.Points[newLink.FirstPointIndex] = newFirstPoint;
                        var newSecondPoint = body.Points[newLink.SecondPointIndex];
                        newSecondPoint.Position -= difference * secondScalar * distanceScalar;
                        body.Points[newLink.SecondPointIndex] = newSecondPoint;
                    }
                    #endregion

                    link = newLink;
                }
                for (var i = 0; i < body.Points.Count; i++)
                {
                    var point = body.Points[i];
                    var newPoint = point;

                    #region Point gravity
                    newPoint.Acceleration = body.GravityForce;// * point.Mass / point.Mass;

                    var velocity = newPoint.Position - newPoint.PreviousPosition;
                    velocity *= newPoint.DampingAmount;
                    newPoint.PreviousPosition = newPoint.Position;
                    newPoint.Position = newPoint.Position + velocity + newPoint.Acceleration * 0.5f;
                    #endregion

                    #region Pin points
                    if (newPoint.IsPinned)
                    {
                        newPoint.Position = newPoint.PreviousPosition = newPoint.PinnedPosition;
                    }
                    #endregion

                    point = newPoint;
                }
            }
        }
    }
}