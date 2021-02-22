using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.Domain.Seed;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Domain
{
    public class BotContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }
        public DbSet<ServerBotRole> ServerBotRoles { get; set; }
        public DbSet<BotRole> Roles { get; set; }
        public DbSet<ServerUser> ServerUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Raid> Raids { get; set; }
        public DbSet<RaidItem> RaidItems { get; set; }
        public DbSet<RaidLockout> RaidLockouts { get; set; }
        public DbSet<RaidParticipant> RaidParticipants { get; set; }
        public DbSet<SoftResEvent> SoftResEvents { get; set; }
        public DbSet<SoftResUser> SoftResUsers { get; set; }
        public DbSet<ReservedItem> ReservedItems { get; set; }
        public DbSet<RecurringRaidTemplate> RecurringRaidTemplates { get; set; }
        public DbSet<SoftResRaidTemplate> SoftResRaidTemplates { get; set; }
        public DbSet<PaginatedMessage> PaginatedMessages { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<MessageChannel> MessageChannels { get; set; }
        public DbSet<SoftResRaidMonitor> SoftResRaidMonitors { get; set; }
        public BotContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSeeder<RoleDataSeeder, BotRole>()
                .UseSeeder<ItemSeeder, Item>();
            modelBuilder.Entity<Raid>()
                .HasIndex(r => r.Active);
            base.OnModelCreating(modelBuilder);
        }
    }
}