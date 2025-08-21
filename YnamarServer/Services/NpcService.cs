using static YnamarServer.Network.NetworkPackets;
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

		public void SendNpcAttackedtoMap(int mapNum, int layerNum, MapNpc mapNpc)
		{
			PacketBuffer bufferSend = new PacketBuffer();
			bufferSend.AddInteger((int)ServerPackets.SNpcMove);
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
