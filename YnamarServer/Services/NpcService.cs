using Microsoft.Extensions.DependencyInjection;
using static YnamarServer.Network.NetworkPackets;
using YnamarServer.Database.Models;
using YnamarServer.Network;

namespace YnamarServer.Services
{
	internal class NpcService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public NpcService(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
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
	}
}
