using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using ProtoBuf;

namespace YnamarServer.Database.Models
{
    [ProtoContract]
    internal class Character
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
        public int EXP { get; set; }

        [ProtoMember(5)]
        public int Map { get; set; }

        [ProtoMember(6)]
        public int X { get; set; }

        [ProtoMember(7)]
        public int Y { get; set; }

        [ProtoMember(8)]
        public byte Dir { get; set; }

        [ProtoMember(9)]
        public int XOffset { get; set; }

        [ProtoMember(10)]
        public int YOffset { get; set; }

        [ProtoMember(11)]
        public byte Access { get; set; }

        [ProtoMember(12)]
        public int MaxHP;

        [ProtoMember(13)]
        public int HP;

        public Account Account { get; set; } = null!;
    }
}
