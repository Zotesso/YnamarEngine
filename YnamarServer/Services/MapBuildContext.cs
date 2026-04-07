using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models;
using YnamarServer.Database.Models.Map;

namespace YnamarServer.Services
{
    public class MapBuildContext
    {
        public Dictionary<string, ushort> TileLookup { get; } = new();
        public Dictionary<ushort, Database.Models.Map.TileDefinition> TileDefinitions { get; } = new();

        private ushort _nextId = 1;

        public ushort GetOrCreateTileId(Tile tile)
        {
            string key = $"{tile.TilesetNumber}_{tile.TileX}_{tile.TileY}_{tile.Type}_{tile.Data1}_{tile.Data2}_{tile.Data3}";

            if (TileLookup.TryGetValue(key, out ushort id))
                return id;

            id = _nextId++;

            TileLookup[key] = id;

            TileDefinitions[id] = new Database.Models.Map.TileDefinition
            {
                Id = id,
                Tileset = tile.TilesetNumber,
                TileX = tile.TileX,
                TileY = tile.TileY,
                Type = tile.Type,
                Data1 = tile.Data1,
                Data2 = tile.Data2,
                Data3 = tile.Data3
            };

            return id;
        }
    }
}
