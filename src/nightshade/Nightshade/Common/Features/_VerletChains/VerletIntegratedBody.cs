using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace Nightshade.Common.Features;

internal class VerletIntegratedBody
{
    private class Point
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

    private class Link
    {
        public Point FirstPoint;
        public Point SecondPoint;
        public float RestingDistance;
        public float Stiffness;

        public Link(Point firstPoint, Point secondPoint)
        {
            FirstPoint = firstPoint;
            SecondPoint = secondPoint;
        }
    }

    private const int constraint_iteration_amount = 3;

    private readonly List<Point> _points;
    private readonly List<Link> _links;

    public Vector2 GravityForce { get; set; } = new Vector2(0, 1);

    public VerletIntegratedBody(Vector2 startingPosition, Vector2 endingPosition,
        int pointAmount = 10,
        float restingDistance = 200)
    {
        _points = [];
        _links = [];

        for (int i = 0; i < pointAmount; i++)
        {
            Point point = new Point(Vector2.Lerp(startingPosition, endingPosition, (float)i / (pointAmount - 1)))
            {
                Mass = 10,
                DampingAmount = 0.98f,
            };
            _points.Add(point);
        }
        _points.First().IsPinned = true;
        //_points.Last().IsPinned = true;

        for (int i = 0; i < pointAmount - 1; i++)
        {
            Link link = new Link(_points[i], _points[i + 1])
            {
                RestingDistance = restingDistance,
                Stiffness = 1f,
            };
            _links.Add(link);
        }
    }

    public void Update()
    {
        foreach (Link link in _links)
        {
            for (int i = 0; i < constraint_iteration_amount; i++)
            {
                Vector2 difference = link.FirstPoint.Position - link.SecondPoint.Position;
                float distance = difference.Length();
                float distanceScalar = (link.RestingDistance - distance) / distance;

                float firstWeight = 1 / link.FirstPoint.Mass;
                float secondWeight = 1 / link.SecondPoint.Mass;
                float firstScalar = (firstWeight / (firstWeight + secondWeight)) * link.Stiffness;
                float secondScalar = link.Stiffness - firstScalar;

                link.FirstPoint.Position += difference * firstScalar * distanceScalar;
                link.SecondPoint.Position -= difference * secondScalar * distanceScalar;
            }
        }
        foreach (Point point in _points)
        {
            point.Acceleration = GravityForce;// * point.Mass / point.Mass;

            Vector2 velocity = point.Position - point.PreviousPosition;
            velocity *= point.DampingAmount;
            point.PreviousPosition = point.Position;
            point.Position = point.Position + velocity + point.Acceleration * 0.5f;

            if (point.IsPinned)
            {
                point.Position = point.PreviousPosition = point.PinnedPosition;
            }
        }
    }

    public List<Vector2> GetPointPositions()
    {
        return [.. _points.Select(point => point.Position)];
    }

    public void SetFirstPosition(Vector2 position)
    {
        _points.First().Position = position;
        _points.First().PreviousPosition = position;
        _points.First().PinnedPosition = position;
    }

    public void SetLastPosition(Vector2 position)
    {
        _points.Last().Position = position;
        _points.Last().PreviousPosition = position;
        _points.Last().PinnedPosition = position;
    }
}