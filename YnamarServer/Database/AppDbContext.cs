using Microsoft.EntityFrameworkCore;
using YnamarServer.Database.Models;

namespace YnamarServer.Database
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TesteEntity> TesteEntities { get; set; }

    }
}
