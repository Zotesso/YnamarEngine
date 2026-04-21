using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.GameLogic.Collision;


namespace YnamarServer.Database.Models
{
    [ProtoContract]
    public class MapNpc
    {
        [Key]
        public int Id { get; set; }

        [ProtoMember(1)]
        public int Hp { get; set; }

        [ProtoMember(2)]
        public int RespawnWait { get; set; }

        [ProtoIgnore]
        public MapLayer? Layer { get; set; } = null;

        [ProtoMember(3)]
        public int NpcId { get; set; }

        [ProtoMember(4)]
        public Npc? Npc { get; set; } = null;

        [ProtoMember(5)]
        public int X { get; set; }

        [ProtoMember(6)]
        public int Y { get; set; }

        [ProtoMember(7)]
        public byte Dir { get; set; }

        [NotMapped]
        public BaseRectangleHitbox Hitbox => new BaseRectangleHitbox
        {
            X = X,
            Y = Y,
            Width = 32,
            Height = 32
        };
    }
}
