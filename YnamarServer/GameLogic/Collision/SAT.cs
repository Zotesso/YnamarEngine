using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.GameLogic.Collision
{
    public static class SAT
    {
        public static bool Intersects(PolygonHitbox a, PolygonHitbox b)
        {
            // Check axes from both polygons
            if (!CheckAxes(a.Points, b.Points)) return false;
            if (!CheckAxes(b.Points, a.Points)) return false;

            return true;
        }
        public static bool Intersects(PolygonHitbox a, BaseRectangleHitbox b)
        {
            var bPoly = ToPolygon(b);
            return Intersects(a, new PolygonHitbox(bPoly));
        }

        public static Vector2[] ToPolygon(BaseRectangleHitbox rect)
        {
            return new Vector2[]
            {
                new Vector2(rect.X, rect.Y),
                new Vector2(rect.X + rect.Width, rect.Y),
                new Vector2(rect.X + rect.Width, rect.Y + rect.Height),
                new Vector2(rect.X, rect.Y + rect.Height)
            };
        }

        private static bool CheckAxes(Vector2[] a, Vector2[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                // Edge
                Vector2 p1 = a[i];
                Vector2 p2 = a[(i + 1) % a.Length];

                Vector2 edge = p2 - p1;

                // Perpendicular axis
                Vector2 axis = new Vector2(-edge.Y, edge.X);

                axis = Vector2.Normalize(axis);

                // Project both polygons onto axis
                Project(a, axis, out float minA, out float maxA);
                Project(b, axis, out float minB, out float maxB);

                // Check overlap
                if (maxA < minB || maxB < minA)
                    return false; // Separation found
            }

            return true;
        }

        private static void Project(Vector2[] points, Vector2 axis, out float min, out float max)
        {
            float dot = Vector2.Dot(points[0], axis);
            min = dot;
            max = dot;

            for (int i = 1; i < points.Length; i++)
            {
                dot = Vector2.Dot(points[i], axis);

                if (dot < min) min = dot;
                if (dot > max) max = dot;
            }
        }
    }
}
