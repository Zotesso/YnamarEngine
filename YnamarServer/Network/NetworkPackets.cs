using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Network
{
    internal class NetworkPackets
    {
        public enum ServerPackets
        {
            SJoinGame = 1,
            SPlayerData,
            SPlayerMove,
            SLoadMap,
            SNpcKilled,
            SInventorySlotUpdate,
            SInventorySlotDelete,
            SUdpHandshake,
            SSendChunk,
        }

        public enum ClientTcpPackets
        {
            CLogin = 1,
            CRegister,
            CPlayerMove,
            CLoadMap,
            CItemUsed,
            CRequestChunk,
        }

        public enum ClientUdpPackets
        {
            UdpCHandshake = 100,
            UdpCAttack,
        }

        public enum ServerUdpPackets
        {
            UdpSNpcAttacked = 100,
            UdpSNpcMove,
            UdpSPlayerAttacking,
        }
    }
}
