using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace Nightshade.Common.Features;

internal struct VerletIntegratedBody
{
    public struct Point
    {
        public Vector2 Position;
        public Vector2 PreviousPosition;
        public Vector2 Acceleration;
        public float Mass;
        public float DampingAmount;
        public bool IsPinned;
        public Vector2 PinnedPosition;

        public Point(Vector2 position)
        {
            Position = PreviousPosition = PinnedPosition = position;
            Acceleration = Vector2.Zero;
            IsPinned = false;
        }
    }

    public struct Link
    {
        public int FirstPointIndex;
        public int SecondPointIndex;
        public float RestingDistance;
        public float Stiffness;

        public Link(int firstPoint, int secondPoint)
        {
            FirstPointIndex = firstPoint;
            SecondPointIndex = secondPoint;
        }
    }

    public readonly List<Point> Points;
    public readonly List<Link> Links;

    public bool IsActive;
    public Vector2 GravityForce;

    public VerletIntegratedBody()
    {
        Points = [];
        Links = [];
        IsActive = false;
        GravityForce = new Vector2(0, 1);
    }
}