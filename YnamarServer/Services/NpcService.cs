using Microsoft.Extensions.DependencyInjection;
using static YnamarServer.Network.NetworkPackets;
using YnamarServer.Database.Models;
using YnamarServer.Network;
using System.Collections.Generic;
using YnamarServer.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace YnamarServer.Services
{
	internal class NpcService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
        private static ServerTCP stcp = new ServerTCP();

        public NpcService(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}

        public async Task<List<Npc>> LoadAllNpcs()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.Npcs
                    .Include(p => p.Drops)
                    .ToListAsync();
            };
        }

        public void SendNpcAttackedtoMap(int mapNum, int layerNum, MapNpc mapNpc)
		{
			PacketBuffer bufferSend = new PacketBuffer();
			bufferSend.AddInteger((int)ServerUdpPackets.UdpSNpcAttacked);
			bufferSend.AddInteger(mapNum);
			bufferSend.AddInteger(layerNum);
			bufferSend.AddInteger(mapNpc.Id);

			byte[] mapNpcProtoBuf = bufferSend.SerializeProto<MapNpc>(mapNpc);
			bufferSend.AddInteger(mapNpcProtoBuf.Length);
			bufferSend.AddByteArray(mapNpcProtoBuf);

            NetworkManager.ServerUdp.SendDataToMap(mapNum, bufferSend.ToArray());

            bufferSend.Dispose();
		}

        public void SendNpcKilledToMap(int mapNum, int layerNum, MapNpc mapNpc)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SNpcKilled);
            bufferSend.AddInteger(mapNum);
            bufferSend.AddInteger(layerNum);
            bufferSend.AddInteger(mapNpc.Id);

            byte[] mapNpcProtoBuf = bufferSend.SerializeProto<MapNpc>(mapNpc);
            bufferSend.AddInteger(mapNpcProtoBuf.Length);
            bufferSend.AddByteArray(mapNpcProtoBuf);

            stcp.SendDataToMap(mapNum, bufferSend.ToArray());

            bufferSend.Dispose();
        }
    }
}
