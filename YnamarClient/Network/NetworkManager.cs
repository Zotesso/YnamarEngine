

namespace YnamarClient.Network
{
	internal class NetworkManager
	{
		public static ClientUDP Client { get; } = new ClientUDP();
		public static ClientTCP ClientTcp { get; } = new ClientTCP();
    }
}
