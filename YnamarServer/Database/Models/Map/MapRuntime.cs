using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    public class MapRuntime
    {
        public int Id;
        public string Name;

        public int Width;
        public int Height;

        // Only runtime data
        public List<MapNpc> Npcs = new();

        // Players inside this map
        public HashSet<int> Players = new();

        // Optional: loaded chunks cache
        public Dictionary<(int x, int y), Chunk> LoadedChunks = new();
    }
}
