using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient.Network
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
        }

        public enum ClientTcpPackets
        {
            CLogin = 1,
            CRegister,
            CPlayerMove,
            CLoadMap
        }
    }
}
