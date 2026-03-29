using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.GameLogic.Items.Factory;
using YnamarServer.Network;

namespace YnamarServer.Services
{
    internal class ItemService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ItemService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<List<Item>> LoadAllItems()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.Items.ToListAsync();
            };
        }

        public async Task UseItem(int playerId, int itemId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var inventorySlot = await dbContext.InventorySlots
                        .Include(s => s.Item)
                        .Include(s => s.Inventory)
                            .ThenInclude(i => i.Character)
                                .ThenInclude(c => c.EquippedItems)
                        .FirstOrDefaultAsync(slot =>
                            slot.Inventory.Character.Id == playerId &&
                            slot.ItemId == itemId);

                if (inventorySlot is null)
                    throw new Exception("Item not found in inventory");

                var playerCharacter = inventorySlot.Inventory.Character;
                var item = inventorySlot.Item;

                var handler = ItemUseHandlerFactory.GetHandler(item);

                await handler.UseAsync(playerCharacter, inventorySlot, dbContext);

                await dbContext.SaveChangesAsync();
            }
            ;
        }
    }
}
