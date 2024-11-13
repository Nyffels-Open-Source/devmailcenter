using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMailCenter.Core;

public class DmcContext : DbContext
{
    public DbSet<MailServer> MailServers { get; set; }
    public DbSet<MailServerSettings> MailServerSettings { get; set; }

    public DmcContext(DbContextOptions<DmcContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MailServer>(entity =>
        {
            entity.ToTable("DmcMailServer");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Id).IsUnique();
            entity.Property(e => e.Id).HasColumnName("ServerId").IsRequired();
            entity.Property(e => e.Name).HasColumnName("ServerName").IsRequired();
            entity.Property(e => e.Type).HasColumnName("ServerType").IsRequired();
            entity.Property(e => e.Active).HasColumnName("ServerActive").IsRequired().HasDefaultValue(true);
            entity.Property(e => e.Created).HasColumnName("ServerCreated").IsRequired().HasDefaultValue(DateTime.UtcNow);
            entity.Property(e => e.Modified).HasColumnName("ServerModified").IsRequired(false).HasDefaultValue(null);
            entity.Property(e => e.LastUsed).HasColumnName("ServerLastUsed").IsRequired(false).HasDefaultValue(null);

            entity.HasMany(e => e.MailServerSettings).WithOne().HasForeignKey(e => e.ServerId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MailServerSettings>(entity =>
        {
            entity.ToTable("DmcMailServerSettings");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Id).IsUnique();
            entity.HasIndex(e => e.ServerId);
            entity.Property(e => e.Id).HasColumnName("MailServerSettingsId").IsRequired();
            entity.Property(e => e.ServerId).HasColumnName("MailServerSettingsServerId").IsRequired();
            entity.Property(e => e.Key).HasColumnName("MailServerSettingsKey").IsRequired();
            entity.Property(e => e.Value).HasColumnName("MailServerSettingsValue").IsRequired();
            entity.Property(e => e.Created).HasColumnName("MailServerSettingsCreated").IsRequired().HasDefaultValue(DateTime.UtcNow);
            entity.Property(e => e.Modified).HasColumnName("MailServerSettingsModified").IsRequired(false).HasDefaultValue(null);
        });
    }
}