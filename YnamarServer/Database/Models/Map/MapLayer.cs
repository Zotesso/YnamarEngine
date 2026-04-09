using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    [ProtoContract]
    public class MapLayer
    {
        [Key]
        public int Id { get; set; }

        [ProtoMember(1)]
        [NotMapped]
        public int MapId { get; set; }

        [ProtoMember(2)]
        public byte LayerLevel { get; set; }

        [ProtoMember(3)]
        [NotMapped]
        public ICollection<Tile> Tile { get; } = new List<Tile>();

        [ProtoMember(4)]
        public ICollection<MapNpc> MapNpc { get; } = new List<MapNpc>();

        [ProtoIgnore]
        [NotMapped]
        public Map? Map { get; set; } = null;

        [ProtoIgnore]
        public MapMetadata? MapMetadata { get; set; } = null;
    }
}
