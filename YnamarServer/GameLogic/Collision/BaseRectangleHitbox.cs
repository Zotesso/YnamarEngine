namespace YnamarServer.GameLogic.Collision
{
    public class BaseRectangleHitbox : Hitbox
    {
        public float X, Y, Width, Height;
        public override bool Intersects(Hitbox other)
        {
            if (other is BaseRectangleHitbox rect)
            {
                return !(X + Width < rect.X || X > rect.X + rect.Width || Y + Height < rect.Y || Y > rect.Y + rect.Height);
            }

            return false;
        }
    }
}
