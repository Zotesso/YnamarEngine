using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarClient.Network;
using static YnamarClient.Network.NetworkPackets;

namespace YnamarClient.Services
{
    internal class ItemService
    {
        public static void handleItemUsed(int itemId)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInteger((int)ClientTcpPackets.CItemUsed);
            buffer.AddInteger(Types.Players[Globals.playerIndex].Id);
            buffer.AddInteger(itemId);
            NetworkManager.ClientTcp.SendData(buffer.ToArray());
            buffer.Dispose();
        }
    }
}
