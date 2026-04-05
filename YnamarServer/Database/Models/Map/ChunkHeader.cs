using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models.Map
{
    struct ChunkHeader
    {
        int ChunkX;
        int ChunkY;
        ushort Width;
        ushort Height;
        byte LayerCount;
    }
}
