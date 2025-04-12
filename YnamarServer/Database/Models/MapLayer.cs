using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    internal class MapLayer
    {
        [Key]
        public int Id { get; set; }
        public int MapId { get; set; }
        public byte LayerLevel { get; set; }
        public ICollection<Tile> Tile { get; } = new List<Tile>();
        public Map Map { get; set; } = null!;
    }
}
