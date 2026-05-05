using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors.Models.Animation
{
    [ProtoContract]
    public class PolygonHitbox
    {
        [ProtoMember(1)]
        public Vec2[] Points { get; set; }
        public PolygonHitbox() { }

        public PolygonHitbox(Vec2[] points)
        {
            Points = points;
        }
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
