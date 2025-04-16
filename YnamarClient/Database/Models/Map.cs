using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YnamarClient.Database.Models
{
    [ProtoContract]
    internal class Map
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int MaxMapX { get; set; }

        [ProtoMember(4)]
        public int MaxMapY { get; set; }

        [ProtoMember(5)]
        public Byte[] LastUpdate { get; set; }

        [ProtoMember(6)]
        public ICollection<MapLayer> Layer { get; } = new List<MapLayer>();
    }
}
