using YnamarServer.Database.Models.Animation;

namespace YnamarServer.GameLogic.Collision
{
    public class ActiveAttack
    {
        public int PlayerId;
        public byte Direction;
        public float StartTime;
        public AnimationClip Animation;
        public HashSet<int> HitEntities = new();
        public HashSet<int> ProcessedFrames = new();
    }
}
