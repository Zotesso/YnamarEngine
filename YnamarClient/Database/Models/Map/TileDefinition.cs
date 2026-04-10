using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient.Database.Models
{
    [ProtoContract]
    public class TileDefinition
    {
        [ProtoMember(1)]
        public ushort Id { get; set; }

        [ProtoMember(2)]
        public int Tileset { get; set; }

        [ProtoMember(3)]
        public int TileX { get; set; }

        [ProtoMember(4)]
        public int TileY { get; set; }

        [ProtoMember(5)]
        public byte Type { get; set; }

        [ProtoMember(6)]
        public byte Moral { get; set; }

        [ProtoMember(7)]
        public byte Data1 { get; set; }

        [ProtoMember(8)]
        public byte Data2 { get; set; }

        [ProtoMember(9)]
        public byte Data3 { get; set; }
    }
}
