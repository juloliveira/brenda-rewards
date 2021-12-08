﻿// <auto-generated />
using System;
using Brenda.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Brenda.Data.Migrations
{
    [DbContext(typeof(BrendaContext))]
    partial class BrendaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Brenda.Core.AccountActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Reason")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Reference")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<double>("Value")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("UserId");

                    b.ToTable("AccountActivity");
                });

            modelBuilder.Entity("Brenda.Core.Action", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Image")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("Tag")
                        .IsUnique()
                        .HasName("IX_Action_Tag");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("Brenda.Core.Asset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ActionId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Enable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Resource")
                        .HasColumnType("TEXT")
                        .HasMaxLength(127);

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<string>("Title")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("Brenda.Core.BrendaRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(127)")
                        .HasMaxLength(127);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Description")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Brenda.Core.BrendaUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MobilePhone")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("PointsBalance")
                        .HasColumnType("double");

                    b.Property<string>("PushToken")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Brenda.Core.Campaign", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ActionId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AssetId")
                        .HasColumnType("char(36)");

                    b.Property<double>("Balance")
                        .HasColumnType("double");

                    b.Property<Guid?>("ChallengeId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("DefinitionsId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasMaxLength(127);

                    b.Property<string>("JsonOnGoing")
                        .HasColumnType("TEXT")
                        .HasMaxLength(127);

                    b.Property<double>("Reward")
                        .HasColumnType("double");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<string>("Title")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("AssetId");

                    b.HasIndex("ChallengeId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DefinitionsId");

                    b.HasIndex("Tag")
                        .IsUnique()
                        .HasName("IX_Tag");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("Brenda.Core.CampaignDefinitions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("ValidateGeoLocation")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("ValidationEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ValidationStart")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("CampaignDefinitions");
                });

            modelBuilder.Entity("Brenda.Core.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<double>("Balance")
                        .HasColumnType("double");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Document")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("Document")
                        .IsUnique()
                        .HasName("UX_Customer_Document");

                    b.HasIndex("Tag")
                        .IsUnique()
                        .HasName("UX_Customer_Tag");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Brenda.Core.ErrorMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Field")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("FieldName")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Lang")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Message")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("Field")
                        .IsUnique()
                        .HasName("IX_ErrorMessage_Tag");

                    b.ToTable("ErrorMessages");
                });

            modelBuilder.Entity("Brenda.Core.GeoRestriction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("DefinitionsId")
                        .HasColumnType("char(36)");

                    b.Property<double>("Latitude")
                        .HasColumnType("double");

                    b.Property<double>("Longitude")
                        .HasColumnType("double");

                    b.Property<double>("Radius")
                        .HasColumnType("double");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("DefinitionsId");

                    b.ToTable("GeoRestriction");
                });

            modelBuilder.Entity("Brenda.Core.Quiz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AssetId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.HasIndex("ParentId");

                    b.ToTable("Quiz");
                });

            modelBuilder.Entity("Brenda.Core.Settings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasMaxLength(127);

                    b.Property<string>("Email")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<bool>("HasLogo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LogoAvatar")
                        .HasColumnType("TEXT")
                        .HasMaxLength(127);

                    b.Property<string>("LogoOriginal")
                        .HasColumnType("TEXT")
                        .HasMaxLength(127);

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Site")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Brenda.Core.UrlAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CampaignId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Url")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("UrlAction");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(127)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("ClaimValue")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(127)")
                        .HasMaxLength(127);

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(127)")
                        .HasMaxLength(127);

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(127)")
                        .HasMaxLength(127);

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.Property<string>("Value")
                        .HasColumnType("varchar(127) CHARACTER SET utf8mb4")
                        .HasMaxLength(127);

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Brenda.Core.AccountActivity", b =>
                {
                    b.HasOne("Brenda.Core.Customer", "Customer")
                        .WithMany("AccountStatement")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Brenda.Core.BrendaUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Brenda.Core.Asset", b =>
                {
                    b.HasOne("Brenda.Core.Action", "Action")
                        .WithMany()
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Brenda.Core.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Brenda.Core.BrendaUser", b =>
                {
                    b.HasOne("Brenda.Core.Customer", "Customer")
                        .WithMany("Users")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Brenda.Core.Campaign", b =>
                {
                    b.HasOne("Brenda.Core.Action", "Action")
                        .WithMany()
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Brenda.Core.Asset", "Asset")
                        .WithMany()
                        .HasForeignKey("AssetId");

                    b.HasOne("Brenda.Core.Campaign", "Challenge")
                        .WithMany("Campaigns")
                        .HasForeignKey("ChallengeId");

                    b.HasOne("Brenda.Core.Customer", "Customer")
                        .WithMany("Campaigns")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Brenda.Core.CampaignDefinitions", "Definitions")
                        .WithMany()
                        .HasForeignKey("DefinitionsId");
                });

            modelBuilder.Entity("Brenda.Core.GeoRestriction", b =>
                {
                    b.HasOne("Brenda.Core.CampaignDefinitions", "Definitions")
                        .WithMany("CoordinatesAllowed")
                        .HasForeignKey("DefinitionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Brenda.Core.Quiz", b =>
                {
                    b.HasOne("Brenda.Core.Asset", "Asset")
                        .WithMany("Questions")
                        .HasForeignKey("AssetId");

                    b.HasOne("Brenda.Core.Quiz", "Parent")
                        .WithMany("Options")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Brenda.Core.Settings", b =>
                {
                    b.HasOne("Brenda.Core.Customer", "Customer")
                        .WithOne("Settings")
                        .HasForeignKey("Brenda.Core.Settings", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Brenda.Core.UrlAction", b =>
                {
                    b.HasOne("Brenda.Core.Campaign", "Campaign")
                        .WithMany("UrlActions")
                        .HasForeignKey("CampaignId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Brenda.Core.BrendaRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Brenda.Core.BrendaUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Brenda.Core.BrendaUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Brenda.Core.BrendaRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Brenda.Core.BrendaUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Brenda.Core.BrendaUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
