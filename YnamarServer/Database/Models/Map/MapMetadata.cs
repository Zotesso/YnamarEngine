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
    public class MapMetadata
    {
        [Key]
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int MaxMapX { get; set; }

        [ProtoMember(4)]
        public int MaxMapY { get; set; }

        [ProtoMember(5)]
        public int Version { get; set; }

        [ProtoMember(6)]
        public string FilePath { get; set; }

        [ProtoMember(7)]
        public ICollection<MapLayer> Layer { get; } = new List<MapLayer>();
    }
}
