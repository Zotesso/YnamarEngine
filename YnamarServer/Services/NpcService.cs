using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Database.Protos;
using YnamarServer.Network;
using static YnamarServer.Network.NetworkPackets;

namespace YnamarServer.Services
{
	internal class NpcService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
        private static ServerTCP stcp = ServerTCP.Instance;

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

            stcp.SendPacketToMap(mapNum, bufferSend);

            bufferSend.Dispose();
        }

        public void SendMapNpcChunkToClient(int index, int mapNum, Point chunkInitialPosition, Point chunkFinalPosition)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SSendMapNpcChunk);

            List<MapNpc> mapNpcChunk = new List<MapNpc>();
            foreach (MapNpc mapNpc in InMemoryDatabase.Maps[mapNum].Npcs)
            {
                if (mapNpc.X >= chunkInitialPosition.X && mapNpc.X <= chunkFinalPosition.X &&
                    mapNpc.Y >= chunkInitialPosition.Y && mapNpc.Y <= chunkFinalPosition.Y)
                {
                    mapNpcChunk.Add(mapNpc);
                }
            }
            byte[] mapNpcChunkProtoBuf = bufferSend.SerializeProto<List<MapNpc>>(mapNpcChunk);
            bufferSend.AddInteger(mapNpcChunkProtoBuf.Length);
            bufferSend.AddByteArray(mapNpcChunkProtoBuf);

            stcp.SendPacket(index, bufferSend);

            bufferSend.Dispose();
        }
    }
}
