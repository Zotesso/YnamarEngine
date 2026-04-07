using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YnamarServer.Database.Models;

namespace YnamarServer.Services
{
    public class MapRebuilder
    {
        private Dictionary<ushort, TileDefinition> LoadTileDefinitions(string mapPath)
        {
            string path = Path.Combine(mapPath, "tiles.json");

            var json = File.ReadAllText(path);

            var list = JsonSerializer.Deserialize<List<TileDefinition>>(json);

            return list.ToDictionary(t => t.Id);
        }

        public Map Rebuild(string mapPath, int width, int height)
        {
            int chunkSize = 32;

            int chunkCountX = (int)Math.Ceiling(width / (float)chunkSize);
            int chunkCountY = (int)Math.Ceiling(height / (float)chunkSize);

            // Load tile definitions
            var tileDefinitions = LoadTileDefinitions(mapPath);

            // Create empty map
            Map map = new Map
            {
                MaxMapX = width,
                MaxMapY = height,
            };

            // Prepare layers dynamically
            Dictionary<int, MapLayer> layers = new();

            for (int cx = 0; cx < chunkCountX; cx++)
            {
                for (int cy = 0; cy < chunkCountY; cy++)
                {
                    string chunkPath = Path.Combine(mapPath, $"chunk_{cx}_{cy}.bin");

                    if (!File.Exists(chunkPath))
                        continue;

                    var chunk = ChunkSerializer.LoadChunk(chunkPath);

                    foreach (var layerEntry in chunk.Layers)
                    {
                        int layerId = layerEntry.Key;
                        var tileArray = layerEntry.Value;

                        if (!layers.ContainsKey(layerId))
                        {
                            layers[layerId] = new MapLayer
                            {
                                LayerLevel = (byte)layerId
                            };

                            map.Layer.ElementAt(layerId).LayerLevel = (byte)layerId;
                        }

                        for (int x = 0; x < chunkSize; x++)
                        {
                            for (int y = 0; y < chunkSize; y++)
                            {
                                int worldX = cx * chunkSize + x;
                                int worldY = cy * chunkSize + y;

                                if (worldX >= width || worldY >= height)
                                    continue;

                                int index = y * chunkSize + x;
                                ushort tileId = tileArray[index];

                                if (tileId == 0)
                                    continue;

                                var def = tileDefinitions[tileId];

                                map.Layer.ElementAt(layerId).Tile.Add(new Tile
                                {
                                    X = worldX,
                                    Y = worldY,
                                    TilesetNumber = def.Tileset,
                                    TileX = def.TileX,
                                    TileY = def.TileY,
                                    Type = def.Type,
                                    Data1 = def.Data1,
                                    Data2 = def.Data2,
                                    Data3 = def.Data3
                                });
                            }
                        }
                    }
                }
            }

            return map;
        }
    }
}
