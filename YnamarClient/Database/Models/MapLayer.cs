using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public Map Map { get; set; } = null!;
    }
}
