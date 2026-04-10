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
        private readonly ConcurrentDictionary<uint, int> _indexByUdpPeerId = new();
        private readonly ConcurrentDictionary<TcpClient, int> _indexByTcp = new();

        private readonly IndexPool _indexPool;
        private readonly Random _random = new();

        public SessionManager(int maxPlayers)
        {
            _indexPool = new IndexPool(maxPlayers);
        }

        public PlayerSession CreateSession(int playerId, int mapId, TcpClient tcp)
        {
            if (!_indexPool.TryRent(out int index))
                throw new Exception("Servidor cheio");

            var session = new PlayerSession
            {
                Index = index,
                PlayerId = playerId,
                CurrentMapId = mapId,
                Tcp = tcp,
                UdpToken = GenerateToken()
            };

            _sessionsByIndex[index] = session;
            _indexByPlayerId[playerId] = index;
            _indexByTcp[tcp] = index;

            return session;
        }

        public void RemoveSession(int index)
        {
            if (_sessionsByIndex.TryRemove(index, out var session))
            {
                _indexByPlayerId.TryRemove(session.PlayerId, out _);
                _indexByTcp.TryRemove(session.Tcp, out _); 
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

        public PlayerSession? GetByUdpPeer(uint peerId)
        {
            if (_indexByUdpPeerId.TryGetValue(peerId, out var index))
                return GetByIndex(index);

            return null;
        }

        public PlayerSession? GetByTcp(TcpClient tcp)
        {
            if (_indexByTcp.TryGetValue(tcp, out var index))
                return GetByIndex(index);

            return null;
        }

        public bool RegisterPeer(uint peerId, int index, long token)
        {
            var session = GetByIndex(index);
            if (session == null) return false;

            if (session.UdpToken != token)
                return false;

            _indexByUdpPeerId[peerId] = index;

            return true;
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
