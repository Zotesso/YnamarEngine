using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Database.Models.Animation;
using YnamarServer.Database.Protos;

namespace YnamarServer.Admin.Services
{
    internal class AnimationEditorService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AnimationEditorService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<AnimationClipList?> GetAllAnimationClipsSummary()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return new AnimationClipList
                {
                    AnimationClipSummaryList = await dbContext.AnimationClips.Select(item => new AnimationClipSummary
                    {
                        Id = item.Id,
                        Name = item.Name
                    }).ToListAsync()
                };
            }
            ;
        }

        public async Task<AnimationClip?> GetAnimationClip(int animationClipId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.AnimationClips.Where(m => m.Id == animationClipId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<int> SaveAnimationClipAsync(AnimationClip editedClip)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                using var transaction = await dbContext.Database.BeginTransactionAsync();

                var existing = await dbContext.AnimationClips.FirstOrDefaultAsync(n => n.Id == editedClip.Id);

                if (existing is null)
                {
                    editedClip.Id = await GetNextFreeAnimationClipIdAsync();
                    dbContext.AnimationClips.Add(editedClip);
                }
                else
                {
                    dbContext.Entry(existing).CurrentValues.SetValues(editedClip);
                }

                var rows = await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return rows;
            }
            ;
        }

        public async Task<int> GetNextFreeAnimationClipIdAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var ids = await dbContext.AnimationClips
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
