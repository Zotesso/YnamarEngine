using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    public class Chunk
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Size { get; set; } = 32;

        // LayerId -> flattened tile array
        public Dictionary<int, ushort[]> Layers { get; set; } = new();

        // Optional (runtime only)
        public bool IsDirty { get; set; } = false;
    }
}
