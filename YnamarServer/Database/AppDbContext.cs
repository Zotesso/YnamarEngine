using Microsoft.EntityFrameworkCore;
using YnamarServer.Database.Models;

namespace YnamarServer.Database
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public virtual DbSet<Account> Accounts { get; set; }

    }
}
