using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.ValueObjects;
using AuctionSystem.Domain.Constants;

namespace AuctionSystem.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<VerificationDoc> VerificationDocs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!; // ← هذا مطلوب

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all configurations
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configure soft delete query filters
            ConfigureSoftDelete(builder);

            // Configure value object conversions
            ConfigureValueObjects(builder);

            ConfigureBids(builder);
            ConfigureProjects(builder);
            ConfigureAuction(builder);

        }

        private void ConfigureSoftDelete(ModelBuilder builder)
        {
            // Apply soft delete query filters to all soft deletable entities
            builder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            builder.Entity<Project>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Auction>().HasQueryFilter(a => !a.IsDeleted);

        }

        private void ConfigureValueObjects(ModelBuilder builder)
        {
            // Configure Money value object conversions
            builder.Entity<Project>()
                .Property(p => p.EstimatedBudget)
                .HasConversion(
                    money => money.Amount,
                    amount => new Money(amount, "IQD"))
                .HasColumnName("EstimatedBudgetAmount")
                .HasPrecision(18, 2);

            builder.Entity<Auction>()
                .Property(a => a.StartingPrice)
                .HasConversion(
                    money => money.Amount,
                    amount => new Money(amount, "IQD"))
                .HasColumnName("StartingPriceAmount")
                .HasPrecision(18, 2);

            builder.Entity<Auction>()
                .Property(a => a.CurrentPrice)
                .HasConversion(
                    money => money.Amount,
                    amount => new Money(amount, "IQD"))
                .HasColumnName("CurrentPriceAmount")
                .HasPrecision(18, 2);

            builder.Entity<Auction>()
                .Property(a => a.MinPrice)
                .HasConversion(
                    money => money.Amount,
                    amount => new Money(amount, "IQD"))
                .HasColumnName("MinPriceAmount")
                .HasPrecision(18, 2);

            builder.Entity<Auction>()
                .Property(a => a.TargetPrice)
                .HasConversion(
                    money => money.Amount,
                    amount => new Money(amount, "IQD"))
                .HasColumnName("TargetPriceAmount")
                .HasPrecision(18, 2);
builder.Entity<Auction>()
    .Property(a => a.PriceDropInterval)
    .HasColumnType("time"); // store as SQL Server time

            builder.Entity<Auction>()
                .Property(a => a.PriceDropAmount)
                .HasConversion(
                    money => money.Amount,
                    amount => new Money(amount, "IQD"))
                .HasColumnName("PriceDropAmountValue")
                .HasPrecision(18, 2);

            builder.Entity<Bid>()
                .Property(b => b.Amount)
                .HasConversion(
                    money => money.Amount,
                    amount => new Money(amount, "IQD"))
                .HasColumnName("BidAmount")
                .HasPrecision(18, 2);
            builder.Entity<RefreshToken>(entity =>
                        {
                            entity.HasKey(rt => rt.Id);
                            entity.Property(rt => rt.Token).IsRequired().HasMaxLength(500);
                            entity.Property(rt => rt.UserId).IsRequired();
                            entity.HasIndex(rt => rt.Token).IsUnique();
                            entity.HasIndex(rt => new { rt.UserId, rt.Token });

                            entity.HasOne<User>()
                                .WithMany()
                                .HasForeignKey(rt => rt.UserId)
                                .OnDelete(DeleteBehavior.Cascade);
                        });
        }
        private void ConfigureBids(ModelBuilder builder)
        {
            builder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId);




        }
        private void ConfigureProjects(ModelBuilder builder)
        {
            builder.Entity<Project>()
                .HasOne(p => p.ProjectOwner)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.ProjectOwnerId);



        }

        private void ConfigureAuction(ModelBuilder builder)
        {
            builder.Entity<Auction>()
                .HasOne(a => a.Project)
                .WithMany(p => p.Auctions)
                .HasForeignKey(a => a.ProjectId);

            
        }
        

    }
}