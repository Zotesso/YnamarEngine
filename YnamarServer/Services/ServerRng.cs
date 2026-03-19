using System.Security.Cryptography;

namespace YnamarServer.Services
{
    public static class ServerRng
    {
        private static readonly RandomNumberGenerator _seedGenerator
        = RandomNumberGenerator.Create();

        private static readonly ThreadLocal<Random> _rng
            = new ThreadLocal<Random>(CreateRng);

        private static Random CreateRng()
        {
            Span<byte> bytes = stackalloc byte[4];
            _seedGenerator.GetBytes(bytes);
            int seed = BitConverter.ToInt32(bytes);
            return new Random(seed);
        }

        public static int Next(int max)
        {
            return _rng.Value!.Next(max);
        }
    }
}
