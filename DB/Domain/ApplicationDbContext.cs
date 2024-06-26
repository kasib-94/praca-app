using DB.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace DB.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionAttachment> AuctionAttachments { get; set; }
        public DbSet<AuctionStatus> AuctionStatuses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
