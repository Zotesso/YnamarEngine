using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.GameLogic.Collision
{
    public class PolygonHitbox : Hitbox
    {
        public Vector2[] Points; // World coordinates

        public PolygonHitbox(Vector2[] points)
        {
            Points = points;
        }

        public override bool Intersects(Hitbox other)
        {
            if (other is PolygonHitbox poly)
                return SAT.Intersects(this, poly);

            return false;
        }

        public static Vector2 Rotate(Vector2 point, Vector2 origin, float angleRad)
        {
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            Vector2 translated = point - origin;

            return new Vector2(
                translated.X * cos - translated.Y * sin,
                translated.X * sin + translated.Y * cos
            ) + origin;
        }
    }
}
