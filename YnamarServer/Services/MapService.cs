using Microsoft.Extensions.DependencyInjection;
using YnamarServer.Database;
using static YnamarServer.Network.NetworkPackets;
using YnamarServer.Network;
using Microsoft.EntityFrameworkCore;
using YnamarServer.Database.Models;

namespace YnamarServer.Services
{
    internal class MapService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private static ServerTCP stcp = new ServerTCP();

        public MapService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<Map> LoadMap(int mapNum)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.Maps.Where(m => m.Id == mapNum)
                    .Include(p => p.Layer)
                        .ThenInclude(x => x.Tile)
                    .Include(p => p.Layer)
                        .ThenInclude(x => x.MapNpc)
                            .ThenInclude(mapNpc => mapNpc.Npc)
                    .FirstAsync();
            };  
        }

        public async Task<List<Map>> LoadAllMaps()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.Maps
                    .Include(p => p.Layer)
                        .ThenInclude(x => x.Tile)
                    .Include(p => p.Layer)
                        .ThenInclude(x => x.MapNpc)
                            .ThenInclude(mapNpc => mapNpc.Npc)
                    .ToListAsync();
            };
        }

        public void SendMapToClient(int index, Map map)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SLoadMap);
            bufferSend.AddInteger(index);

            byte[] mapProtoBuf = bufferSend.SerializeProto<Map>(map);
            bufferSend.AddInteger(mapProtoBuf.Length);
            bufferSend.AddByteArray(mapProtoBuf);

            stcp.SendData(index, bufferSend.ToArray());

            bufferSend.Dispose();
        }
    }
}
