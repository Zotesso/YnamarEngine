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

namespace YnamarServer.Admin.Services
{
    internal class NpcEditorService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public NpcEditorService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<NpcList?> GetAllNpcsSummaryAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return new NpcList
                {
                    NpcsSummary = await dbContext.Npcs.Select(npc => new NpcSummary
                      {
                        Id = npc.Id,
                        Name = npc.Name
                      }).ToListAsync()
               };
            };
        }

        public async Task<List<NpcBehavior>> GetNpcBehaviorsListAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.NpcBehaviors.ToListAsync();
            };
        }

        public async Task<Npc?> GetNpcSummary(int npcId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.Npcs.Where(m => m.Id == npcId)
                    .Include(p => p.Drops)
                        .ThenInclude(x => x.Item)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<int> SaveNpcAsync(Npc editedNpc)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                using var transaction = await dbContext.Database.BeginTransactionAsync();

                var existing = await dbContext.Npcs.Include(m => m.Drops).FirstOrDefaultAsync(n => n.Id == editedNpc.Id);

                if (existing is null)
                {
                    editedNpc.Id = await GetNextFreeNpcIdAsync();
                    dbContext.Npcs.Add(editedNpc);
                }
                else
                {
                    dbContext.Entry(existing).CurrentValues.SetValues(editedNpc);
                }

                var rows = await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return rows;
            };
        }

        public async Task<int> GetNextFreeNpcIdAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var ids = await dbContext.Npcs
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
