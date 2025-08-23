using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    [ProtoContract]
    internal class Npc
    {
        [Key]
        public int Id { get; set; }

        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public int Sprite { get; set; }

        [ProtoMember(3)]
        public int Level { get; set; }

        [ProtoMember(4)]
        public int MaxHp { get; set; }

        [ProtoMember(5)]
        public int Atk { get; set; }

        [ProtoMember(6)]
        public int Def { get; set; }

        [ProtoMember(7)]
        public byte Behavior { get; set; }

        public int RespawnTime { get; set; }

        public ICollection<NpcDrop> Drops { get; set; } = new List<NpcDrop>();
    }
}
