using Microsoft.EntityFrameworkCore.Metadata.Internal;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Network;
using YnamarServer.Network.Session;
using static YnamarServer.Network.NetworkPackets;

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

        public static async Task GiveItemAsync(PlayerSession session, int itemId, int quantity)
        {
            InventorySlot inventorySlotToUpdate = await Program.inventoryService.AddItemToPlayerInventory(itemId, session.PlayerId, quantity);
            
            if (inventorySlotToUpdate == null)
            {
                Console.WriteLine($"Failed to add item {itemId} to player {session.PlayerId}'s inventory.");
                return;
            }

            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SInventorySlotUpdate);
            bufferSend.AddInteger(session.Index);
            inventorySlotToUpdate.Item = InMemoryDatabase.Items.Where(i => i.Id == inventorySlotToUpdate.ItemId).First();
            byte[] inventorySlotProtoBuf = bufferSend.SerializeProto<InventorySlot>(inventorySlotToUpdate);

            bufferSend.AddInteger(inventorySlotProtoBuf.Length);
            bufferSend.AddByteArray(inventorySlotProtoBuf);

            ServerTCP.Instance.SendPacket(session.Index, bufferSend);

            bufferSend.Dispose();
        }
    }
}
