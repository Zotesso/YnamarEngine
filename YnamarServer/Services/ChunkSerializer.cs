
using System.Text.Json;
using YnamarServer.Database.Models;
using YnamarServer.Database.Models.Map;

namespace YnamarServer.Services
{
    public static class ChunkSerializer
    {
        public static Dictionary<string, ushort> tileLookup = new();
        public static Dictionary<ushort, Database.Models.TileDefinition> tileDefinitions = new();
        public static ushort nextId = 1;

        public static void SaveChunk(string path, Database.Models.Map.Chunk chunk)
        {
            using var fs = new FileStream(path, FileMode.Create);
            using var bw = new BinaryWriter(fs);

            // Header
            bw.Write(chunk.X);
            bw.Write(chunk.Y);
            bw.Write((ushort)chunk.Size);
            bw.Write((ushort)chunk.Size);
            bw.Write((byte)chunk.Layers.Count);

            foreach (var layer in chunk.Layers)
            {
                bw.Write((byte)layer.Key); // LayerId

                var tiles = layer.Value; // ushort[]

                bw.Write((ushort)tiles.Length);

                foreach (var tileId in tiles)
                    bw.Write(tileId);
            }
        }

        public static Database.Models.Map.Chunk LoadChunk(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            using var br = new BinaryReader(fs);

            Database.Models.Map.Chunk chunk = new Database.Models.Map.Chunk();

            chunk.X = br.ReadInt32();
            chunk.Y = br.ReadInt32();

            int width = br.ReadUInt16();
            int height = br.ReadUInt16();

            int layerCount = br.ReadByte();

            chunk.Layers = new Dictionary<int, ushort[]>();

            for (int i = 0; i < layerCount; i++)
            {
                int layerId = br.ReadByte();
                int tileCount = br.ReadUInt16();

                ushort[] tiles = new ushort[tileCount];

                for (int t = 0; t < tileCount; t++)
                    tiles[t] = br.ReadUInt16();

                chunk.Layers[layerId] = tiles;
            }

            return chunk;
        }

        public static void BuildAndSaveChunks(Map map)
        {
            int chunkSize = 32;

            int chunkCountX = (int)Math.Ceiling(map.MaxMapX / (float)chunkSize);
            int chunkCountY = (int)Math.Ceiling(map.MaxMapY / (float)chunkSize);

            string basePath = $"maps/{map.Name}/";

            Directory.CreateDirectory(basePath);

            for (int cx = 0; cx < chunkCountX; cx++)
            {
                for (int cy = 0; cy < chunkCountY; cy++)
                {
                    Database.Models.Map.Chunk chunk = new Database.Models.Map.Chunk
                    {
                        X = cx,
                        Y = cy,
                        Size = chunkSize
                    };

                    foreach (var layer in map.Layer)
                    {
                        ushort[] tiles = new ushort[chunkSize * chunkSize];

                        for (int x = 0; x < chunkSize; x++)
                        {
                            for (int y = 0; y < chunkSize; y++)
                            {
                                int worldX = cx * chunkSize + x;
                                int worldY = cy * chunkSize + y;

                                // Outside map bounds
                                if (worldX >= map.MaxMapX || worldY >= map.MaxMapY)
                                {
                                    tiles[y * chunkSize + x] = 0;
                                    continue;
                                }

                                var tile = layer.Tile.FirstOrDefault(t => t.X == worldX && t.Y == worldY);

                                tiles[y * chunkSize + x] = ConvertToTileId(tile);
                            }
                        }

                        chunk.Layers[layer.LayerLevel] = tiles;
                    }

                    string path = $"{basePath}/chunk_{cx}_{cy}.bin";

                    ChunkSerializer.SaveChunk(path, chunk);
                }
            }
        }

        public static ushort ConvertToTileId(Tile tile)
        {
            // Build a unique key
            string key = $"{tile.TilesetNumber}_{tile.TileX}_{tile.TileY}_{tile.Type}_{tile.Data1}_{tile.Data2}_{tile.Data3}";

            if (tileLookup.TryGetValue(key, out ushort existingId))
                return existingId;

            ushort newId = nextId++;

            tileLookup[key] = newId;

            tileDefinitions[newId] = new Database.Models.TileDefinition
            {
                Id = newId,
                Tileset = tile.TilesetNumber,
                TileX = tile.TileX,
                TileY = tile.TileY,
                Type = tile.Type,
                Data1 = (byte)tile.Data1,
                Data2 = (byte)tile.Data2,
                Data3 = (byte)tile.Data3
            };

            return newId;
        }

        public static void SaveTileDefinitions(string mapName, Dictionary<ushort, TileDefinition> tileDefinitions)
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
