using System;
using System.Linq;
using Brenda.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brenda.Data
{
    public class BrendaContext : IdentityDbContext<BrendaUser, BrendaRole, Guid>
    {
        public BrendaContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CreateIndexes(builder);

            MapValueObjects(builder);

            builder.Entity<BrendaUser>(entity => {
                entity.Property(m => m.Email).HasMaxLength(127);
                entity.Property(m => m.NormalizedEmail).HasMaxLength(127);
                entity.Property(m => m.NormalizedUserName).HasMaxLength(127);
                entity.Property(m => m.UserName).HasMaxLength(127);
                entity.Property(m => m.MobilePhone);
            });
            builder.Entity<BrendaRole>(entity => {
                entity.Property(m => m.Id).HasMaxLength(127);
                entity.Property(m => m.Name).HasMaxLength(127);
                entity.Property(m => m.NormalizedName).HasMaxLength(127);
            });
            builder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.ProviderKey).HasMaxLength(127);
            });
            builder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.RoleId).HasMaxLength(127);
            });
            builder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.LoginProvider).HasMaxLength(110);
                entity.Property(m => m.Name).HasMaxLength(127);
            });

            

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                if (property.Name.Equals("Tag"))
                {
                    property.SetMaxLength(8);
                    property.IsNullable = false;
                }
                else
                    property.SetMaxLength(127);
            }

            builder.Entity<Asset>().Property(x => x.Resource).HasColumnType("TEXT");

            builder.Entity<Campaign>(entity =>
            {
                entity.Property(x => x.Description).HasColumnType("TEXT");
                entity.Property(x => x.JsonOnGoing).HasColumnType("TEXT");

                entity
                .HasMany(x => x.Campaigns)
                .WithOne(x => x.Challenge)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
            });
            
            builder.Entity<Customer>(entity => 
            {
                entity
                    .HasOne(x => x.Settings)
                    .WithOne(x => x.Customer);
            });

            builder.Entity<Settings>(entity => 
            {
                entity.Property(x => x.Description).HasColumnType("TEXT");
                entity.Property(x => x.LogoOriginal).HasColumnType("TEXT");
                entity.Property(x => x.LogoAvatar).HasColumnType("TEXT");
            });
        }

        private void MapValueObjects(ModelBuilder builder)
        {
            //builder.Entity<CampaignDefinitions>();
        }

        private void CreateIndexes(ModelBuilder builder)
        {
            builder.Entity<Customer>()
                .HasIndex(x => x.Tag)
                .HasName("UX_Customer_Tag")
                .IsUnique();

            builder.Entity<Customer>()
                .HasIndex(x => x.Document)
                .HasName("UX_Customer_Document")
                .IsUnique();

            builder.Entity<Campaign>()
                .HasIndex(x => x.Tag)
                .HasName("IX_Tag")
                .IsUnique();

            builder.Entity<Core.Action>()
                .HasIndex(x => x.Tag)
                .HasName("IX_Action_Tag")
                .IsUnique();

            builder.Entity<ErrorMessage>()
                .HasIndex(x => x.Field)
                .HasName("IX_ErrorMessage_Tag")
                .IsUnique();
        }

        public DbSet<ErrorMessage> ErrorMessages { get; set; }

        public DbSet<Asset> Assets { get; set; }
        
        public DbSet<Core.Action> Actions { get; set; }

        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<Customer> Customers { get; set; }
    }
}
