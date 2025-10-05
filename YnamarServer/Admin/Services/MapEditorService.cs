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
using Microsoft.AspNetCore.Http.HttpResults;

namespace YnamarServer.Admin.Services
{
    internal class MapEditorService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MapEditorService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<int> SaveMapAsync(Map editedMap)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                using var transaction = await dbContext.Database.BeginTransactionAsync();

                var existing = await dbContext.Maps.Include(m => m.Layer).ThenInclude(l => l.Tile).FirstOrDefaultAsync(m => m.Id == editedMap.Id);

                if (existing is null)
                {
                    dbContext.Maps.Add(editedMap);
                } else
                {
                    dbContext.Entry(existing).CurrentValues.SetValues(editedMap);
                    foreach(var layer in editedMap.Layer) 
                    {
                        var targetLayer = existing.Layer.FirstOrDefault(l => l.LayerLevel == layer.LayerLevel);
                        if (targetLayer is not null)
                        {
                            existing.Layer.Remove(targetLayer);
                            await dbContext.SaveChangesAsync();
                        }

                        existing.Layer.Add(layer);
                    }

                }

                var rows = await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return rows;
            };
        }

        public async Task<Map?> GetMapAsync(int mapNum)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.Maps.Where(m => m.Id == mapNum)
                    .Include(p => p.Layer)
                        .ThenInclude(x => x.Tile)
                    .Include(p => p.Layer)
                        .ThenInclude(x => x.MapNpc)
                            .ThenInclude(mapNpc => mapNpc.Npc)
                    .FirstOrDefaultAsync();
            };
        }
    }
}
