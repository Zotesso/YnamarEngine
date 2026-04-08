using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YnamarServer.Database.Models;

namespace YnamarServer.Services
{
    public class MapBuildContext
    {
        public Dictionary<string, ushort> TileLookup { get; } = new();
        public Dictionary<ushort, Database.Models.TileDefinition> TileDefinitions { get; } = new();

        private ushort _nextId = 1;

        public ushort GetOrCreateTileId(Tile tile)
        {
            string key = $"{tile.TilesetNumber}_{tile.TileX}_{tile.TileY}_{tile.Type}_{tile.Data1}_{tile.Data2}_{tile.Data3}";

            if (TileLookup.TryGetValue(key, out ushort id))
                return id;

            id = _nextId++;

            TileLookup[key] = id;

            TileDefinitions[id] = new Database.Models.TileDefinition
            {
                Id = id,
                Tileset = tile.TilesetNumber,
                TileX = tile.TileX,
                TileY = tile.TileY,
                Type = tile.Type,
                Data1 = (byte)tile.Data1,
                Data2 = (byte)tile.Data2,
                Data3 = (byte)tile.Data3
            };

            return id;
        }

        public void SaveTileDefinitions(string mapName, Dictionary<ushort, TileDefinition> tileDefinitions)
        {
            string folderPath = Path.Combine("maps", mapName);

            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "tiles.json");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var list = tileDefinitions.Values.ToList();

            string json = JsonSerializer.Serialize(list, options);

            File.WriteAllText(filePath, json);
        }
    }
}
