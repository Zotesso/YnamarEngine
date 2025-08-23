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
            SNpcMove,
            SNpcKilled,
        }

        public enum ClientTcpPackets
        {
            CLogin = 1,
            CRegister,
            CPlayerMove,
            CLoadMap,
        }

        public enum ClientUdpPackets
        {
            UdpCAttack = 100,
        }

        public enum ServerUdpPackets
        {
            UdpSNpcAttacked = 100,
        }
    }
}
