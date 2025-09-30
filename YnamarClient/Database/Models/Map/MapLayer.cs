using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YnamarClient.Database.Models;

namespace YnamarClient.Database.Models
{
    [ProtoContract]
    internal class MapLayer
    {
        [Key]
        public int Id { get; set; }

        [ProtoMember(1)]
        public int MapId { get; set; }

        [ProtoMember(2)]
        public byte LayerLevel { get; set; }

        [ProtoMember(3)]
        public ICollection<Tile> Tile { get; } = new List<Tile>();

        [ProtoMember(4)]
        public ICollection<MapNpc> MapNpc { get; } = new List<MapNpc>();

        [ProtoIgnore]
        public Map? Map { get; set; } = null;
    }
}
