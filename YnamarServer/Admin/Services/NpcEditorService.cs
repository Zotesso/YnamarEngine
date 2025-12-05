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
    }
}
