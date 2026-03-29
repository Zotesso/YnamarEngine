using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Network.Session
{
    public class IndexPool
    {
        private readonly ConcurrentQueue<int> _available = new();

        public IndexPool(int maxPlayers)
        {
            for (int i = 0; i < maxPlayers; i++)
                _available.Enqueue(i);
        }

        public bool TryRent(out int index) => _available.TryDequeue(out index);

        public void Return(int index) => _available.Enqueue(index);
    }
}
