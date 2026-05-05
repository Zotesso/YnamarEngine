using Microsoft.Xna.Framework.Graphics;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient.Graphics
{
    [ProtoContract]
    public class PolygonHitbox
    {
        [ProtoMember(1)]
        public Vec2[] Points; // World coordinates
        public Vector2[] UVs;      // texture coordinates (0–1)

        public PolygonHitbox(Vec2[] points)
        {
            Points = points;
            GenerateUVs();
        }

        public PolygonHitbox() { }

        public void GenerateUVs()
        {
            float minX = Points.Min(p => p.X);
            float maxX = Points.Max(p => p.X);
            float minY = Points.Min(p => p.Y);
            float maxY = Points.Max(p => p.Y);

            float width = maxX - minX;
            float height = maxY - minY;

            UVs = new Vector2[Points.Length];

            for (int i = 0; i < Points.Length; i++)
            {
                float u = (Points[i].X - minX) / width;
                float v = (Points[i].Y - minY) / height;

                UVs[i] = new Vector2(u, v);
            }
        }

        //public override bool Intersects(Hitbox other)
        //{
        //    if (other is PolygonHitbox poly)
        //        return SAT.Intersects(this, poly);

        //    return false;
        //}

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

        [ProtoContract]
        public struct Vec2
        {
            [ProtoMember(1)]
            public float X { get; set; }

            [ProtoMember(2)]
            public float Y { get; set; }

            public Vec2(float x, float y)
            {
                X = x;
                Y = y;
            }

            // Optional helpers
            public static implicit operator Vector2(Vec2 v) => new Vector2(v.X, v.Y);
            public static implicit operator Vec2(Vector2 v) => new Vec2(v.X, v.Y);
        }
    }
}
