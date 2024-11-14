using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMailCenter.Core;

public class DmcContext : DbContext
{
    public DbSet<MailServer> MailServers { get; set; }
    public DbSet<MailServerSettings> MailServerSettings { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<EmailReceiver> EmailReceivers { get; set; }

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
            entity.HasMany(e => e.Emails).WithOne().HasForeignKey(e => e.ServerId).OnDelete(DeleteBehavior.Cascade);
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

        modelBuilder.Entity<Email>(entity =>
        {
            entity.ToTable("DmcEmail");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Id).IsUnique();
            entity.HasIndex(e => e.ServerId);
            entity.Property(e => e.Id).HasColumnName("EmailId").IsRequired();
            entity.Property(e => e.Subject).HasColumnName("EmailSubject").IsRequired();
            entity.Property(e => e.Message).HasColumnName("EmailMessage").IsRequired();
            entity.Property(e => e.Status).HasColumnName("EmailStatus").IsRequired().HasDefaultValue(EmailStatus.Concept);
            entity.Property(e => e.Created).HasColumnName("EmailCreated").IsRequired().HasDefaultValue(DateTime.UtcNow);
            entity.Property(e => e.SendRequested).HasColumnName("EmailSendRequested").HasDefaultValue(null);
            entity.Property(e => e.Completed).HasColumnName("EmailCompleted").HasDefaultValue(null);
            entity.Property(e => e.RawServerResponse).HasColumnName("EmailRawServerResponse").HasDefaultValue(null);
            entity.Property(e => e.ServerId).HasColumnName("EmailServerSettingsServerId").IsRequired();
            
            entity.HasMany(e => e.Receivers).WithOne().HasForeignKey(e => e.EmailId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EmailReceiver>(entity =>
        {
            entity.ToTable("DmcEmailReceiver");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Id).IsUnique();
            entity.HasIndex(e => e.EmailId);
            entity.Property(e => e.Id).HasColumnName("EmailReceiverId").IsRequired();
            entity.Property(e => e.EmailId).HasColumnName("EmailReceiverEmailId").IsRequired();
            entity.Property(e => e.ReceiverEmail).HasColumnName("EmailReceiverReceiverEmail").IsRequired();
            entity.Property(e => e.Type).HasColumnName("EmailReceiverType").IsRequired();
        });
    }
}