using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using YnamarServer.Database.Models;
using YnamarServer.Database.Models.Animation;
using YnamarServer.Database.Models.Items;

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
        public virtual DbSet<NpcBehavior> NpcBehaviors { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventorySlot> InventorySlots { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<AnimationClip> AnimationClips { get; set; }
        public virtual DbSet<AnimationFrame> AnimationFrames { get; set; }
        public virtual DbSet<PlayerEquipament> PlayerEquipaments { get; set; }


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

            modelBuilder.Entity<MapNpc>()
                .HasOne(mn => mn.Npc)
                .WithMany()
                .HasForeignKey(mn => mn.NpcId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Npc>()
                .Property(n => n.Id)
                .ValueGeneratedNever();

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

            modelBuilder.Entity<PlayerEquipament>()
                .HasKey(pe => new { pe.CharacterId, pe.Slot });

            modelBuilder.Entity<PlayerEquipament>()
                .HasOne(pe => pe.Character)
                .WithMany(c => c.EquippedItems)
                .HasForeignKey(pe => pe.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
