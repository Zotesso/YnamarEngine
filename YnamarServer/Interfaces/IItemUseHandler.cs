using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;

namespace YnamarServer.Interfaces
{
    public interface IItemUseHandler
    {
        Task UseAsync(Character player, InventorySlot slot, AppDbContext dbContext);
    }
}
