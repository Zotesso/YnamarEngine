using ENet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Database.Protos;
using YnamarServer.Network;
using YnamarServer.Network.Session;
using static YnamarServer.Network.NetworkPackets;

namespace YnamarServer.Services
{
    internal class MapService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private static ServerTCP stcp = ServerTCP.Instance;

        public MapService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<MapMetadata> LoadMap(int mapNum)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.MapsMetadata.Where(m => m.Id == mapNum)
                    .Include(p => p.Layer)
                        .ThenInclude(x => x.MapNpc)
                            .ThenInclude(mapNpc => mapNpc.Npc)
                    .FirstAsync();
            };  
        }

        public async Task<List<MapRuntime>> LoadAllMaps()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var maps = await dbContext.MapsMetadata
                    .Include(m => m.Layer)
                        .ThenInclude(l => l.MapNpc)
                            .ThenInclude(n => n.Npc)
                                .ThenInclude(npc => npc.Drops)
                    .ToListAsync();

                return maps.Select(m => new MapRuntime
                {
                    Id = m.Id,
                    Name = m.Name,
                    Width = m.MaxMapX,
                    Height = m.MaxMapY,

                    Npcs = m.Layer
                        .SelectMany(l => l.MapNpc)
                        .ToList()
                }).ToList();
            }
            ;
        }

        public async Task SaveMapNpcRespawnWait(int playerMapNum, int layerIndex, int mapNpcIndex)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {

                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                MapNpc inMemoryMapNpc = InMemoryDatabase.Maps[playerMapNum].Npcs.ElementAt(mapNpcIndex);

                var npc = new MapNpc { Id = inMemoryMapNpc.Id };

                dbContext.Attach(npc);

                dbContext.Entry(npc).Property(x => x.RespawnWait).CurrentValue = inMemoryMapNpc.RespawnWait;
                dbContext.Entry(npc).Property(x => x.RespawnWait).IsModified = true;

                await dbContext.SaveChangesAsync();
            };
        }

        public void SendMapToClient(int index, MapLoadDto map)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SLoadMap);
            bufferSend.AddInteger(index);

            byte[] mapProtoBuf = bufferSend.SerializeProto<MapLoadDto>(map);
            bufferSend.AddInteger(mapProtoBuf.Length);
            bufferSend.AddByteArray(mapProtoBuf);

            stcp.SendPacket(index, bufferSend);

            bufferSend.Dispose();
        }

        public void SendMapChunkToClient(int index, ChunkDto chunk)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SSendChunk);

            byte[] chunkDtoProtoBuf = bufferSend.SerializeProto<ChunkDto>(chunk);
            bufferSend.AddInteger(chunkDtoProtoBuf.Length);
            bufferSend.AddByteArray(chunkDtoProtoBuf);

            stcp.SendPacket(index, bufferSend);

            bufferSend.Dispose();
        }

        public void SendMapNpcToMap(int mapNum, int layerNum, int mapNpcIndex, MapNpc mapNpc)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerUdpPackets.UdpSNpcMove);
            bufferSend.AddInteger(mapNum);
            bufferSend.AddInteger(layerNum);
            bufferSend.AddInteger(mapNpcIndex);

            byte[] mapNpcProtoBuf = bufferSend.SerializeProto<MapNpc>(mapNpc);
            bufferSend.AddInteger(mapNpcProtoBuf.Length);
            bufferSend.AddByteArray(mapNpcProtoBuf);

            NetworkManager.ServerUdp.SendDataToMap(mapNum, bufferSend.ToArray());

            bufferSend.Dispose();
        }
    }
}
