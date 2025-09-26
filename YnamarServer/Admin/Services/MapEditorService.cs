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

                var existing = await dbContext.Maps
                    .Include(m => m.Layer)
                        .ThenInclude(l => l.Tile)
                    .FirstOrDefaultAsync(m => m.Id == editedMap.Id);

                if (existing is null)
                {
                    dbContext.Maps.Add(editedMap);
                }
                else
                {
                    dbContext.Entry(existing).CurrentValues.SetValues(editedMap);

                    foreach (var layer in editedMap.Layer)
                    {
                        var targetLayer = existing.Layer
                            .FirstOrDefault(l => l.LayerLevel == layer.LayerLevel);

                        if (targetLayer == null)
                        {
                            existing.Layer.Add(layer);
                        }
                        else
                        {
                            dbContext.Entry(targetLayer).CurrentValues.SetValues(layer);
                        }
                    }
                }

                return await dbContext.SaveChangesAsync();
            };
        }
    }
}
