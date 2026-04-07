using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    public class TileDefinition
    {
        public ushort Id;

        public int Tileset;
        public int TileX;
        public int TileY;

        public byte Type;
        public byte Data1;
        public byte Data2;
        public byte Data3;
    }
}
