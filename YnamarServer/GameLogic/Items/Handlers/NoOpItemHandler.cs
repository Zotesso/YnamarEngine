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
    public class NoOpItemHandler : IItemUseHandler
    {
        public Task UseAsync(Character character, InventorySlot slot, AppDbContext context)
        {
            return Task.CompletedTask;
        }
    }
}
