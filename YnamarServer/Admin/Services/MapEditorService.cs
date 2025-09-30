using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models;
using YnamarServer.Database;
using YnamarServer.Network;
using Microsoft.EntityFrameworkCore;

namespace YnamarServer.Admin.Services
{
    internal class MapEditorService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MapEditorService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<int> SaveMap(Map editedMap)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                using var transaction = await dbContext.Database.BeginTransactionAsync();

                var existing = await dbContext.Maps
                    .FirstOrDefaultAsync(m => m.Id == editedMap.Id);

                if (existing is not null)
                {
                    dbContext.Maps.Remove(existing);
                    await dbContext.SaveChangesAsync();
                }

                dbContext.Maps.Add(editedMap);
                var rows = await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return rows;
            };
        }
    }
}
