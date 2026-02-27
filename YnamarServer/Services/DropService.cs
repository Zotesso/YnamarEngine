namespace YnamarServer.Services
{
    public static class DropService
    {
        private const int PRECISION = 1_000_000;
        private const int SCALE = 1000;

        public static bool Roll(int baseDropRate, int finalMultiplier = SCALE)
        {
            if (baseDropRate <= 0)
                return false;

            long finalRate = (long)baseDropRate * finalMultiplier / SCALE;

            if (finalRate > PRECISION)
                finalRate = PRECISION;

            return ServerRng.Next(PRECISION) < finalRate;
        }

        public static int CombineMultipliers(params int[] multipliers)
        {
            long result = SCALE;

            foreach (var m in multipliers)
                result = result * m / SCALE;

            return (int)result;
        }
    }
}
