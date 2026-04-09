using Microsoft.AspNetCore.Http.HttpResults;
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
using YnamarServer.Services;

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

                var existing = await dbContext.MapsMetadata.Include(m => m.Layer)
                    .ThenInclude(l => l.MapNpc)
                    .FirstOrDefaultAsync(m => m.Id == editedMap.Id);
                    
                MapMetadata editedMapMetadata = new MapMetadata
                {
                    Id = editedMap.Id,
                    Name = editedMap.Name,
                    MaxMapX = editedMap.MaxMapX,
                    MaxMapY = editedMap.MaxMapY,
                    Version = 1,
                    FilePath = $"maps/{editedMap.Name}/",
                };

                foreach (var layer in editedMap.Layer)
                {
                    editedMapMetadata.Layer.Add(layer);
                }

                if (existing is null)
                {
                    dbContext.MapsMetadata.Add(editedMapMetadata);
                } else
                {
                    foreach (var layer in editedMap.Layer)
                    {
                        var targetLayer = existing.Layer.FirstOrDefault(l => l.LayerLevel == layer.LayerLevel);
                        if (targetLayer is not null)
                        {
                            existing.Layer.Remove(targetLayer);
                            await dbContext.SaveChangesAsync();
                        }

                        existing.Layer.Add(layer);
                    }

                    dbContext.Entry(existing).CurrentValues.SetValues(editedMapMetadata);
                }


                var rows = await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                var context = new MapBuildContext();

                ChunkSerializer.BuildAndSaveChunks(editedMap, context);

                context.SaveTileDefinitions(editedMap.Name, context.TileDefinitions);

                return rows;
            };
        }

        public async Task<Map?> GetMapAsync(int mapNum)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var mapMetaData = await dbContext.MapsMetadata.Include(m => m.Layer)
                    .ThenInclude(l => l.MapNpc)
                    .Where(m => m.Id == mapNum)
                    .FirstOrDefaultAsync();

                if (mapMetaData == null)
                {
                    return null;
                }

                return new MapRebuilder().Rebuild(mapMetaData, mapMetaData.FilePath);
            }
            ;
        }
    }
}
