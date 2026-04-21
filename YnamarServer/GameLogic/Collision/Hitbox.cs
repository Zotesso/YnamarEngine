namespace YnamarServer.GameLogic.Collision
{
    public abstract class Hitbox
    {
        public abstract bool Intersects(Hitbox other);
    }
}
