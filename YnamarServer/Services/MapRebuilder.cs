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
        public Dictionary<ushort, TileDefinition> LoadTileDefinitions(string mapPath)
        {
            string path = Path.Combine(mapPath, "tiles.json");

            var json = File.ReadAllText(path);

            var list = JsonSerializer.Deserialize<List<TileDefinition>>(json);

            return list.ToDictionary(t => t.Id);
        }

        public Map Rebuild(MapMetadata mapMetadata, string mapPath)
        {
            int chunkSize = 32;

            int chunkCountX = (int)Math.Ceiling(mapMetadata.MaxMapX / (float)chunkSize);
            int chunkCountY = (int)Math.Ceiling(mapMetadata.MaxMapY / (float)chunkSize);

            // Load tile definitions
            var tileDefinitions = LoadTileDefinitions(mapPath);
            var layerGrids = new Dictionary<int, Tile[,]>();

            // Create empty map
            Map map = new Map
            {
                Id = mapMetadata.Id,
                MaxMapX = mapMetadata.MaxMapX,
                MaxMapY = mapMetadata.MaxMapY,
                Name = mapMetadata.Name,
            };

            foreach (var (layer, i) in mapMetadata.Layer.Select((value, i) => (value, i)))
            {
                map.Layer.Add(layer);
                layerGrids[i] = new Tile[mapMetadata.MaxMapX, mapMetadata.MaxMapY];
            }

            // Prepare layers dynamically
            //Dictionary<int, MapLayer> layers = new();

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

                        //if (!layers.ContainsKey(layerId))
                        //{
                        //    layers[layerId] = new MapLayer
                        //    {
                        //        LayerLevel = (byte)layerId
                        //    };

                        //    map.Layer.ElementAt(layerId).LayerLevel = (byte)layerId;
                        //}

                        for (int x = 0; x < chunkSize; x++)
                        {
                            for (int y = 0; y < chunkSize; y++)
                            {
                                int worldX = cx * chunkSize + x;
                                int worldY = cy * chunkSize + y;

                                if (worldX >= mapMetadata.MaxMapX || worldY >= mapMetadata.MaxMapY)
                                    continue;

                                int index = y * chunkSize + x;
                                ushort tileId = tileArray[index];

                                if (tileId == 0)
                                    continue;

                                var def = tileDefinitions[tileId];

                                layerGrids[layerId][worldX, worldY] = new Tile
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
                                };

                                //map.Layer.ElementAt(layerId).Tile.Add(new Tile
                                //{
                                //    X = worldX,
                                //    Y = worldY,
                                //    TilesetNumber = def.Tileset,
                                //    TileX = def.TileX,
                                //    TileY = def.TileY,
                                //    Type = def.Type,
                                //    Data1 = def.Data1,
                                //    Data2 = def.Data2,
                                //    Data3 = def.Data3
                                //});
                            }
                        }
                    }
                }
            }

            foreach (var (layerId, grid) in layerGrids)
            {

                for (int x = 0; x < mapMetadata.MaxMapX; x++)
                {
                    for (int y = 0; y < mapMetadata.MaxMapY; y++)
                    {
                        var tile = grid[x, y];

                        if (tile != null)
                            map.Layer.ElementAt(layerId).Tile.Add(tile);
                    }
                }
            }

            return map;
        }
    }
}
