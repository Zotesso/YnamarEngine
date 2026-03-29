using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Network.Session
{
    public class SessionManager
    {
        private readonly ConcurrentDictionary<int, PlayerSession> _sessionsByIndex = new();
        private readonly ConcurrentDictionary<int, int> _indexByPlayerId = new();
        private readonly ConcurrentDictionary<EndPoint, int> _indexByUdp = new();

        private readonly IndexPool _indexPool;
        private readonly Random _random = new();

        public SessionManager(int maxPlayers)
        {
            _indexPool = new IndexPool(maxPlayers);
        }

        public PlayerSession CreateSession(int playerId, TcpClient tcp)
        {
            if (!_indexPool.TryRent(out int index))
                throw new Exception("Servidor cheio");

            var session = new PlayerSession
            {
                Index = index,
                PlayerId = playerId,
                Tcp = tcp,
                UdpToken = GenerateToken()
            };

            _sessionsByIndex[index] = session;
            _indexByPlayerId[playerId] = index;

            return session;
        }

        public void RemoveSession(int index)
        {
            if (_sessionsByIndex.TryRemove(index, out var session))
            {
                _indexByPlayerId.TryRemove(session.PlayerId, out _);

                if (session.UdpEndpoint != null)
                    _indexByUdp.TryRemove(session.UdpEndpoint, out _);

                _indexPool.Return(index);
            }
        }

        public PlayerSession? GetByIndex(int index)
    => _sessionsByIndex.TryGetValue(index, out var s) ? s : null;

        public PlayerSession? GetByPlayerId(int playerId)
        {
            if (_indexByPlayerId.TryGetValue(playerId, out var index))
                return GetByIndex(index);

            return null;
        }

        public PlayerSession? GetByUdp(EndPoint endpoint)
        {
            if (_indexByUdp.TryGetValue(endpoint, out var index))
                return GetByIndex(index);

            return null;
        }

        public bool RegisterUdpEndpoint(IPEndPoint endpoint, int index, long token)
        {
            var session = GetByIndex(index);
            if (session == null) return false;

            if (session.UdpToken != token)
                return false; // 🚨 spoof detectado

            session.UdpEndpoint = endpoint;
            _indexByUdp[endpoint] = index;

            return true;
        }

        public PlayerSession? ValidateUdpPacket(IPEndPoint endpoint)
        {
            return GetByUdp(endpoint);
        }

        private static long GenerateToken()
        {
            Span<byte> buffer = stackalloc byte[8];

            long value;
            do
            {
                RandomNumberGenerator.Fill(buffer);
                value = BitConverter.ToInt64(buffer);
            }
            while (value == 0);

            return value;
        }
    }
}
