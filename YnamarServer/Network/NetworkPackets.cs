﻿using System;
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
            SLoadMap
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
