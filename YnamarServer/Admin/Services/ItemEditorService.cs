using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models;
using YnamarServer.Database;
using YnamarServer.Database.Protos;
using Microsoft.EntityFrameworkCore;
using YnamarServer.Database.Models.Items;

namespace YnamarServer.Admin.Services
{
    internal class ItemEditorService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ItemEditorService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<ItemList?> GetAllItemsSummaryAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return new ItemList
                {
                    ItemsSummary = await dbContext.Items.Select(item => new ItemSummary
                      {
                        Id = item.Id,
                        Name = item.Name
                      }).ToListAsync()
               };
            };
        }

        public async Task<List<ItemType>> GetNpcItemTypeListAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.ItemTypes.ToListAsync();
            };
        }

        public async Task<Item?> GetItemSummary(int itemId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.Items.Where(m => m.Id == itemId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<int> SaveItemAsync(Item editedItem)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                using var transaction = await dbContext.Database.BeginTransactionAsync();

                var existing = await dbContext.Items.FirstOrDefaultAsync(n => n.Id == editedItem.Id);

                if (existing is null)
                {
                    editedItem.Id = await GetNextFreeItemIdAsync();
                    dbContext.Items.Add(editedItem);
                }
                else
                {
                    dbContext.Entry(existing).CurrentValues.SetValues(editedItem);
                }

                var rows = await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return rows;
            };
        }

        public async Task<int> GetNextFreeItemIdAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var ids = await dbContext.Items
                .Select(n => n.Id)
                .OrderBy(id => id)
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
    }
}
