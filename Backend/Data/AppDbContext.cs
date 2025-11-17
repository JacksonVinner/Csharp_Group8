using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Queue> Queues { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<SelectionRecord> SelectionRecords { get; set; }
    public DbSet<UserProgress> UserProgresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User配置
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Project配置
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedProjects)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Queue配置
        modelBuilder.Entity<Queue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Project)
                .WithMany(p => p.Queues)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Image配置
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Queue)
                .WithMany(q => q.Images)
                .HasForeignKey(e => e.QueueId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.QueueId, e.ImageGroup });
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // SelectionRecord配置
        modelBuilder.Entity<SelectionRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Queue)
                .WithMany(q => q.SelectionRecords)
                .HasForeignKey(e => e.QueueId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User)
                .WithMany(u => u.SelectionRecords)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.SelectedImage)
                .WithMany(i => i.SelectionRecords)
                .HasForeignKey(e => e.SelectedImageId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => new { e.QueueId, e.UserId, e.ImageGroup }).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // UserProgress配置
        modelBuilder.Entity<UserProgress>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Queue)
                .WithMany(q => q.UserProgresses)
                .HasForeignKey(e => e.QueueId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User)
                .WithMany(u => u.UserProgresses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.QueueId, e.UserId }).IsUnique();
            entity.Property(e => e.LastUpdated).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}

