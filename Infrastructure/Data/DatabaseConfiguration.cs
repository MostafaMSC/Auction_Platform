using AuctionSystem.Domain.Constants;
using AuctionSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuctionSystem.Infrastructure.Data
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.ProjectTitle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.ProjectDescription)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(p => p.Location)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Status)
                .HasConversion<int>()
                .IsRequired();

            // Relationships
            builder.HasOne(p => p.ProjectOwner)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Auctions)
                .WithOne(a => a.Project)
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(p => p.CategoryId);
            builder.HasIndex(p => p.Status);
            builder.HasIndex(p => p.CreatedAt);
        }

    }

    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.ToTable("Auctions");
            
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.MaxDuration)
                .IsRequired();

            builder.Property(a => a.PriceDropInterval)
                .IsRequired();

            // Relationships
            builder.HasOne(a => a.Project)
                .WithMany(p => p.Auctions)
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);


            // Indexes
            builder.HasIndex(a => a.ProjectId);
            builder.HasIndex(a => a.Status);
            builder.HasIndex(a => new { a.StartAt, a.EndAt });
            builder.HasIndex(a => a.CreatedAt);
        }
    }


    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CategoryDescription)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(c => c.CategoryName)
                .IsUnique();
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // Additional user properties beyond Identity
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);




            builder.Property(u => u.VerificationStatus)
                .HasDefaultValue(VerificationStatus.Pending);

            // Indexes
            builder.HasIndex(u => u.Email)
                .IsUnique();
            
        }
    }
}

