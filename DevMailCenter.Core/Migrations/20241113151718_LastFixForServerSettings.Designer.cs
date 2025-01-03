﻿// <auto-generated />
using System;
using DevMailCenter.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    [DbContext(typeof(DmcContext))]
    [Migration("20241113151718_LastFixForServerSettings")]
    partial class LastFixForServerSettings
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DevMailCenter.Models.MailServer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ServerId");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true)
                        .HasColumnName("ServerActive");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2024, 11, 13, 15, 17, 17, 812, DateTimeKind.Utc).AddTicks(5604))
                        .HasColumnName("ServerCreated");

                    b.Property<DateTime?>("LastUsed")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ServerLastUsed");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ServerModified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ServerName");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("ServerType");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("DmcMailServer", (string)null);
                });

            modelBuilder.Entity("DevMailCenter.Models.MailServerSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("MailServerSettingsId");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2024, 11, 13, 15, 17, 17, 812, DateTimeKind.Utc).AddTicks(8819))
                        .HasColumnName("MailServerSettingsCreated");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("MailServerSettingsKey");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("MailServerSettingsModified");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("char(36)")
                        .HasColumnName("MailServerSettingsServerId");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("MailServerSettingsValue");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("ServerId");

                    b.ToTable("DmcMailServerSettings", (string)null);
                });

            modelBuilder.Entity("DevMailCenter.Models.MailServerSettings", b =>
                {
                    b.HasOne("DevMailCenter.Models.MailServer", null)
                        .WithMany("MailServerSettings")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevMailCenter.Models.MailServer", b =>
                {
                    b.Navigation("MailServerSettings");
                });
#pragma warning restore 612, 618
        }
    }
}
