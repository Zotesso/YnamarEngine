using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    public class TileDefinition
    {
        public ushort Id { get; set; }

        public int Tileset { get; set; }
        public int TileX { get; set; }
        public int TileY { get; set; }

        public byte Type { get; set; }
        public byte Data1 { get; set; }
        public byte Data2 { get; set; }
        public byte Data3 { get; set; }
    }
}
