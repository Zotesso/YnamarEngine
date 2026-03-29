using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Interfaces;

namespace YnamarServer.GameLogic.Items.Handlers
{
    public class EquipItemHandler : IItemUseHandler
    {
        public async Task UseAsync(Character character, InventorySlot slot, AppDbContext context)
            {
            var existing = character.EquippedItems
                .FirstOrDefault(e => e.Slot == slot.Item.Type);

            if (existing != null)
            {
                context.PlayerEquipaments.Remove(existing);
            } else
            {

                character.EquippedItems.Add(new PlayerEquipament
                {
                    ItemId = slot.Item.Id,
                    Slot = slot.Item.Type
                });
            }
            //context.InventorySlots.Remove(slot);
        }
    }
}
