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
        public virtual DbSet<Map> Maps { get; set; }
        public virtual DbSet<MapNpc> MapNpc { get; set; }
        public virtual DbSet<Npc> Npcs { get; set; }
        public virtual DbSet<NpcDrop> NpcDrops { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventorySlot> InventorySlots { get; set; }
        public virtual DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(e => e.Character)
                .WithOne(e => e.Account)
                .HasForeignKey<Character>();

            modelBuilder.Entity<Character>()
                .HasOne(c => c.Inventory)
                .WithOne(i => i.Character)
                .HasForeignKey<Inventory>(i => i.InventoryId);

            // Inventory ↔ InventorySlot (one-to-many)
            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.Slots)
                .WithOne(s => s.Inventory)
                .HasForeignKey(s => s.InventoryId);

            // InventorySlot ↔ Item (many-to-one, optional)
            modelBuilder.Entity<InventorySlot>()
                .HasOne(s => s.Item)
                .WithMany()
                .HasForeignKey(s => s.ItemId)
                .IsRequired(false);

            modelBuilder.Entity<NpcDrop>()
                .HasOne(d => d.Npc)
                .WithMany(n => n.Drops)
                .HasForeignKey(d => d.NpcId)
                .OnDelete(DeleteBehavior.Cascade);

            // NpcDrop ↔ Item (one-way, required)
            modelBuilder.Entity<NpcDrop>()
                .HasOne(d => d.Item)
                .WithMany()   // one-way association
                .HasForeignKey(d => d.ItemId)
                .IsRequired(true);
        }
    }
}
