using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Network;
using static YnamarServer.Network.NetworkPackets;

namespace YnamarServer.Services
{
    internal class InventoryService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InventoryService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<InventorySlot> AddItemToPlayerInventory(int itemId, int playerId, int quantity)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    int slotId = await GetNextFreeInventorySlotIdAsync(playerId);

                    InventorySlot newInventorySlot = new InventorySlot
                    {
                        InventoryId = playerId,
                        ItemId = itemId,
                        Quantity = quantity,
                        SlotId = slotId
                    };

                    await dbContext.InventorySlots.AddAsync(newInventorySlot);
                    await dbContext.SaveChangesAsync();
                    return newInventorySlot;
                }
                catch (DbUpdateException)
                {
                    return null;
                    // Retry logic: slot was taken, recalc and try again
                }
            }
            ;
        }

        public async Task<int> GetNextFreeInventorySlotIdAsync(int inventoryId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var ids = await dbContext.InventorySlots
                    .Where(x => x.InventoryId == inventoryId)
                    .OrderBy(x => x.SlotId)
                    .Select(x => x.SlotId)
                    .ToListAsync();

                int expectedId = 0;

                foreach (var id in ids)
                {
                    if (id != expectedId)
                        return expectedId;

                    expectedId++;
                }

                return expectedId;
            }
        }

        public void SendInventorySlotDeleteToPlayer(int playerIndex, int inventorySlotId)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SInventorySlotDelete);
            bufferSend.AddInteger(playerIndex);
            bufferSend.AddInteger(inventorySlotId);

            ServerTCP.Instance.SendPacket(playerIndex, bufferSend);
            bufferSend.Dispose();
        }
    }
}
