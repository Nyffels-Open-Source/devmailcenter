using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace devmailcenter.db;

public class DmcContext : DbContext
{
    public DbSet<MailServer> MailServers { get; set; }
    public DbSet<MailServerSettings> MailServerSettings { get; set; }

    public DmcContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO Add connectionstring from settings
        optionsBuilder.UseMySQL("server=localhost;database=devmailcenter;user=root;password=DevDevMailCenter!MySQL");
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
            entity.Property(e => e.Modified).HasColumnName("ServerModified");
            entity.Property(e => e.LastUsed).HasColumnName("ServerLastUsed");

            entity.HasMany(e => e.Settings).WithOne().HasForeignKey(e => e.ServerId);
        });

        modelBuilder.Entity<MailServerSettings>(entity =>
        {
            entity.ToTable("DmcMailServerSettings");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Id).IsUnique();
            entity.HasIndex(e => e.ServerId, "ix_DmcMailServer");
            entity.Property(e => e.Id).HasColumnName("MailServerSettingsId").IsRequired();
            entity.Property(e => e.ServerId).HasColumnName("MailServerSettingsServerId").IsRequired();
            entity.Property(e => e.Key).HasColumnName("MailServerSettingsKey").IsRequired();
            entity.Property(e => e.Value).HasColumnName("MailServerSettingsValue").IsRequired();
            entity.Property(e => e.Created).HasColumnName("MailServerSettingsCreated").IsRequired().HasDefaultValue(DateTime.UtcNow);
            entity.Property(e => e.Modified).HasColumnName("MailServerSettingsModified");

            entity.HasOne(e => e.server).WithMany().HasForeignKey(e => e.ServerId);
        });
    }
}