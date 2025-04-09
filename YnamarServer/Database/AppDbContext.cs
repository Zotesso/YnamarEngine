using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using YnamarServer.Database.Models;

namespace YnamarServer.Database
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(e => e.Character)
                .WithOne(e => e.Account)
                .HasForeignKey<Character>();
        }
    }
}
