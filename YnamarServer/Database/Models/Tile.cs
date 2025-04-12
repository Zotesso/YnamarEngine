using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    internal class Tile
    {
        [Key]
        public int Id { get; set; }
        public int MapLayerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int TilesetNumber { get; set; }
        public int TileX { get; set; }
        public int TileY { get; set; }
        public byte Type { get; set; }
        public byte Moral { get; set; }
        public int Data1 { get; set; }
        public int Data2 { get; set; }
        public int Data3 { get; set; }

        public MapLayer MapLayer { get; set; } = null!;
    }
}
