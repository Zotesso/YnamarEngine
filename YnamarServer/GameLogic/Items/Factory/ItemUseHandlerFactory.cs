using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models;
using YnamarServer.Database.Models.Items;
using YnamarServer.GameLogic.Items.Handlers;
using YnamarServer.Interfaces;
using static YnamarServer.Constants;

namespace YnamarServer.GameLogic.Items.Factory
{
    public static class ItemUseHandlerFactory
    {
        public static IItemUseHandler GetHandler(Item item)
        {
            return item.UsageType switch
            {
                UsageItemType.Equipable => new EquipItemHandler(),
                UsageItemType.Consumable => new ConsumableItemHandler(),
                _ => new NoOpItemHandler()
            };
        }
    }
}
