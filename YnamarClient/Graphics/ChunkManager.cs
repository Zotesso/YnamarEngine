using K4os.Compression.LZ4;
using System;
using System.Collections.Generic;
using System.Linq;
using YnamarClient.Database.Models;
using YnamarClient.Database.Protos;
using YnamarClient.Network;

namespace YnamarClient.Graphics
{
    public class ChunkManager
    {
        private readonly Dictionary<(int x, int y), Chunk> _loadedChunks = new();
        public HashSet<(int x, int y)> RequestedChunks = new();

        public int ChunkSize = 32;
        public int ViewDistance = 1;

        public void Update(int playerTileX, int playerTileY)
        {
            int playerChunkX = playerTileX / ChunkSize;
            int playerChunkY = playerTileY / ChunkSize;

            HashSet<(int, int)> needed = new();

            for (int x = -ViewDistance; x <= ViewDistance; x++)
                for (int y = -ViewDistance; y <= ViewDistance; y++)
                {
                    int cx = playerChunkX + x;
                    int cy = playerChunkY + y;

                    if (cx < 0 || cy < 0) continue;

                    needed.Add((cx, cy));

                    if (!_loadedChunks.ContainsKey((cx, cy)))
                    {
                        RequestChunk(cx, cy);
                    }
                }

            // Unload chunks outside view
            var toRemove = _loadedChunks.Keys
                .Where(c => !needed.Contains(c))
                .ToList();

            foreach (var chunkCoord in toRemove)
            {
                UnloadChunk(chunkCoord.x, chunkCoord.y);
            }
        }

        public Chunk FromDto(ChunkDto dto)
        {
            Chunk chunk = new Chunk
            {
                X = dto.X,
                Y = dto.Y,
                Size = dto.Size,
                Layers = new Dictionary<int, ushort[]>()
            };

            foreach (var layer in dto.Layers)
            {
                ushort[] tiles = DecompressTiles(layer.Tiles);

                if (tiles.Length != dto.Size * dto.Size)
                {
                    return null;
                }

                chunk.Layers[layer.LayerId] = tiles;
            }

            return chunk;
        }

        public ushort[] DecompressTiles(byte[] compressed)
        {
            // Decompress bytes
            byte[] raw = LZ4Pickler.Unpickle(compressed);

            // Convert byte[] → ushort[]
            ushort[] tiles = new ushort[raw.Length / 2];

            Buffer.BlockCopy(raw, 0, tiles, 0, raw.Length);

            return tiles;
        }

        public void AddChunk(Chunk chunk)
        {
            _loadedChunks[(chunk.X, chunk.Y)] = chunk;
        }

        public void UnloadChunk(int x, int y)
        {
            if (_loadedChunks.TryGetValue((x, y), out var chunk))
            {
                _loadedChunks.Remove((x, y));
            }
        }

        public IEnumerable<Chunk> GetVisibleChunks()
        {
            return _loadedChunks.Values;
        }

        private void RequestChunk(int x, int y)
        {
            var key = (x, y);

            if (_loadedChunks.ContainsKey(key))
                return;

            if (RequestedChunks.Contains(key))
                return;

            RequestedChunks.Add(key);

            NetworkManager.ClientTcp.SendChunkRequest(x, y);
        }
    }
}
