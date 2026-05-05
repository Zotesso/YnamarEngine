using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Database.Models.Animation;

namespace YnamarServer.GameLogic.Collision
{
    public class AttackManager
    {
        public List<ActiveAttack> activeAttacks = new();

        public void ProcessFrame(ActiveAttack attack, AnimationFrame frame)
        {
            var player = InMemoryDatabase.Player[attack.PlayerId];

            //frame.Hitboxes = new List<Vector2> { new Vector2(frame.SourceX, frame.SourceY), new Vector2(frame.SourceX + frame.SourceWidth, frame.SourceY), new Vector2(frame.SourceX + frame.SourceWidth, frame.SourceY + frame.SourceHeight), new Vector2(frame.SourceX, frame.SourceY + frame.SourceHeight) };
            // var hitbox = BuildHitbox(frame.Hitboxes.ToArray(), new Vector2(player.X, player.Y), attack.Direction);

            var targets = GetNearbyEntities(player, 32).ToList();
            foreach (var polygon in frame.Polygons)
            {
                var hitbox = BuildHitbox(polygon.Points.Select(p => (Vector2)p).ToArray(), new Vector2(player.X, player.Y), attack.Direction);

                foreach (var entity in targets)
                {
                    if (attack.HitEntities.Contains(entity.Id))
                        continue;

                    if (SAT.Intersects(hitbox, entity.Hitbox))
                    {
                        NpcLogicHandler.NpcAttacked(Program.SessionManager.GetByPlayerId(player.Id), player.Map, entity, 50);
                        attack.HitEntities.Add(entity.Id);
                    }
                }
            }
        }

        public PolygonHitbox BuildHitbox(Vector2[] points, Vector2 origin, float rotation)
        {
            // Apply transform
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = PolygonHitbox.Rotate(points[i], Vector2.Zero, rotation) + origin;
            }

            return new PolygonHitbox(points.Select(p => (PolygonHitbox.Vec2)p).ToArray());
        }

        public List<MapNpc> GetNearbyEntities(Character player, float range)
        {
            var result = new List<MapNpc>();
            float rangeSq = range * range;

            MapRuntime playerMap = InMemoryDatabase.Maps.TryGetValue(player.Map, out var map) ? map : null;

            if (playerMap == null) return result;

            foreach (var npc in playerMap.Npcs)
            {
                float dx = npc.X - player.X;
                float dy = npc.Y - player.Y;

                if (dx * dx + dy * dy <= rangeSq)
                {
                    result.Add(npc);
                }
            }

            return result;
        }
    }
}
