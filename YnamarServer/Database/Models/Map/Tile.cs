using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    [ProtoContract]
    public class Tile
    {
        [Key]
        public int Id { get; set; }

        [ProtoMember(1)]
        public int MapLayerId { get; set; }

        [ProtoMember(2)]
        public int X { get; set; }

        [ProtoMember(3)]
        public int Y { get; set; }

        [ProtoMember(4)]
        public int TilesetNumber { get; set; }

        [ProtoMember(5)]
        public int TileX { get; set; }

        [ProtoMember(6)]
        public int TileY { get; set; }

        [ProtoMember(7)]
        public byte Type { get; set; }

        [ProtoMember(8)]
        public byte Moral { get; set; }

        [ProtoMember(9)]
        public int Data1 { get; set; }

        [ProtoMember(10)]
        public int Data2 { get; set; }

        [ProtoMember(11)]
        public int Data3 { get; set; }

        [ProtoIgnore]
        public MapLayer? MapLayer { get; set; } = null;
    }
}
