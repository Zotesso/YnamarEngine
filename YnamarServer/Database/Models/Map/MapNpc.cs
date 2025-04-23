using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace YnamarServer.Database.Models
{
    [ProtoContract]
    internal class MapNpc
    {
        [Key]
        public int Id { get; set; }

        [ProtoMember(1)]
        public int Hp { get; set; }

        [ProtoMember(2)]
        public bool InRespawn { get; set; }
        public MapLayer Layer { get; set; } = null!;

        [ProtoMember(3)]
        public Npc Npc { get; set; } = null!;

        [ProtoMember(4)]
        public int X { get; set; }

        [ProtoMember(5)]
        public int Y { get; set; }

        [ProtoMember(6)]
        public byte Dir { get; set; }
    }
}
